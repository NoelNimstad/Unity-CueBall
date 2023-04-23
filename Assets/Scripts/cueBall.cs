// ladies and gents, we did it, realistic cue ball physics

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cueBall : MonoBehaviour
{
    [SerializeField] private float frictionCoefficient; // friction coefficient of a billiard table is usual 0.05
    [SerializeField] private float mass; // mass in kg
    [SerializeField] private float speedScalar = 0.01f;
    private float weight = 0f; // force that pushus back from the ground (weight = normal force since they are on a flat surface)
    private Vector2 frictionForce = Vector2.zero; // counter acting force on the ball f = μN, this is also the acceleration
    private Vector2 cueForce = Vector2.zero; // force applied by the cue (17N - 35N)
    private Vector2 velocity = Vector2.zero;
    private float movingTime = 0f;
    private bool isMoving = false;

    private void Start()
    {
        weight = mass * 9.81f; // convert the mass of the ball to its normal force
    }

    private void Update()
    {
        if(isMoving)
        {
            Move();
        }
    }

    private void Move()
    {
        movingTime += Time.deltaTime;

        // F = m * a => a = F / m
        velocity = cueForce + frictionForce * 9.18f * movingTime;
        
        if(Mathf.Abs(velocity.x) <= 0.05f)
        {
            isMoving = false;
            movingTime = 0f;
        }

        transform.position += new Vector3(velocity.x, velocity.y) * speedScalar;
    }

    public void Shoot(Vector2 appliedForce, Vector2 direction)
    {
        Debug.Log(appliedForce.magnitude);
        frictionForce = -direction * weight * frictionCoefficient;
        cueForce = appliedForce;
        isMoving = true;
    }
}
