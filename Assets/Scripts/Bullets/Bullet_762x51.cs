using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_762x51 : MonoBehaviour
{
    private Vector2 wind;
    private float speed;
    private float mass = 0.01f;
    private float gravity;
    private Vector3 startPosition;
    private Vector3 startForward;    

    private bool isInitialized = false;
    private float startTime = -1f;

    public void Initialize(Transform startPoint, float speed, float gravity, Vector2 wind)
    {
        this.startPosition = startPoint.position;
        this.startForward = startPoint.forward;
        this.speed = speed;
        this.gravity = gravity;
        this.wind = wind;
        isInitialized = true;
        startTime = -1f;
    }

    private Vector3 FindPointOnParabola(float time)
    {
        Vector3 movementVector = (startForward * time * speed);
        Vector3 windVector = new Vector3(wind.x, 0, wind.y) * time * time;
        Vector3 gravityVector = Vector3.down * mass * (gravity * time * time);
        return startPosition + movementVector + gravityVector + windVector;
    }

    private bool CastRayBetweenPoints(Vector3 startPoint, Vector3 endPoint, out RaycastHit hit)
    {
        return Physics.Raycast(startPoint, endPoint - startPoint, out hit, (endPoint - startPoint).magnitude);
    }

    private void OnHit(RaycastHit hit)
    {
        ShootableObjects shootableObjects = hit.transform.GetComponent<ShootableObjects>();
        if (shootableObjects)
        {
            shootableObjects.OnHit(hit);            
        }
        Destroy(gameObject, 1);
    }
    private void FixedUpdate()
    {
        if (!isInitialized)
        {
            return;
        }
        if (startTime < 0)
        {
            startTime = Time.time;
        }

        RaycastHit hit;
        float currentTime = Time.time - startTime;
        float prevTime = currentTime - Time.fixedDeltaTime;
        float nextTime = currentTime + Time.fixedDeltaTime;

        Vector3 currentPoint = FindPointOnParabola(currentTime);              
        Vector3 nextPoint = FindPointOnParabola(nextTime);

        if (prevTime > 0)
        {
            Vector3 prevPoint = FindPointOnParabola(prevTime);
            if (CastRayBetweenPoints(prevPoint, nextPoint, out hit))
            {
                OnHit(hit);
            }
        }

        if (CastRayBetweenPoints(currentPoint, nextPoint, out hit))
        {
            OnHit(hit);
        }
    }
    private void Update()
    {
        if (!isInitialized || startTime <0)
        {
            return;
        }
        float currentTime = Time.time - startTime;
        Vector3 currentPoint = FindPointOnParabola(currentTime);
        transform.position = currentPoint;
    }
}
