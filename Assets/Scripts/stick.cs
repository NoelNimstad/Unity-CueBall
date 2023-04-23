using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stick : MonoBehaviour
{
    [SerializeField] private GameObject cueBall;
    [SerializeField] private float offsetSpeedChange;
    [SerializeField] private float offsetScalar;
    [SerializeField] private float minimumOffset = 2.5f;
    [SerializeField] private float maximumOffset = 10f;
    private float offset = 2.5f;
    private Vector3 previousPosition = Vector3.zero; // store previous position for speed calculations
    private float theta;
    private Vector3 oldMousePosition = Vector3.zero;

    private void Update()
    {
        bool mouseMoved = UpdateRotation();
        if(mouseMoved) UpdatePosition();
        UpdateOffset();
        CheckForClicks();
    }

    private void UpdatePosition()
    {
        transform.position = cueBall.transform.position + new Vector3(Mathf.Cos(theta), Mathf.Sin(theta), 0) * -offset;
    }

    private bool UpdateRotation()
    { 
        Vector3 mousePosition = Input.mousePosition; // get mouse position

        if(mousePosition != oldMousePosition) // to not have to perform any processor intensive calculations without need
        {
            mousePosition.z = Camera.main.nearClipPlane;  // ensure that the applied object doesn't get a strange z value
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition); // make mouse get similair position coordinates to other objects in scene

            theta = Mathf.Atan2(cueBall.transform.position.y - mouseWorldPosition.y, cueBall.transform.position.x - mouseWorldPosition.x); // calculate angle between mouse and ball
            Vector3 rotation = new Vector3(0, 0, theta * 180 / Mathf.PI - 90); // convert the rotation to degrees from radians and offset it
            transform.rotation = Quaternion.Euler(rotation); // turn the rotation from a vector a quaternion

            oldMousePosition = mousePosition; // update old mouse position

            return true;
        } else return false;
    }

    private void UpdateOffset()
    {
        float offsetChange = ((Input.GetMouseButton(0) ? 1 : 0) - (Input.GetMouseButton(1) ? 1 : 0)) * offsetSpeedChange;
        if(offsetChange != 0)
        {
            offset = Mathf.Max(minimumOffset, Mathf.Min(maximumOffset, offset + offsetChange));
        }
    }

    private void CheckForClicks()
    {
        if(Input.GetMouseButtonDown(2))
        {
            Vector2 direction = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            cueBall ball = cueBall.GetComponent<cueBall>();
            Vector2 appliedForce = direction * (offset * offsetScalar);
            ball.Shoot(appliedForce, direction);
        }
    }
}
