using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceKeeper : MonoBehaviour
{
    public Transform targetObj;
    private float distanceBetweenObject;
    [SerializeField, Range(0, 50)] private float distanceThreshold;
    public bool shouldMove;

    [SerializeField, Range(0, 10)] private float baseSpeed;
    [SerializeField, Range(0, 15)] private float accelerationFactor;

    private void Update()
    {
        shouldMove = CheckDistance();
        if (shouldMove)
        {
            MoveToTarget();
        }
        else
        {
            OnDrawGizmos();
        }
    }

    private void MoveToTarget()
    {
        float movementSpeed = baseSpeed + (distanceBetweenObject * accelerationFactor);
        float step = movementSpeed * Time.deltaTime;
        Vector3 newTarget = new Vector3(targetObj.position.x, (targetObj.position.y + distanceThreshold), targetObj.position.z);

        transform.position = Vector3.MoveTowards(transform.position, newTarget, step);
    }

    private bool CheckDistance()
    {
        distanceBetweenObject = Vector3.Distance(targetObj.transform.position, transform.transform.position);

        return distanceBetweenObject >= distanceThreshold || distanceBetweenObject / 2 <= distanceThreshold;
    }

    private void OnDrawGizmos()
    {
        if (targetObj != null)
        {
            // Draw a line between handObj and mapObj
            Gizmos.color = Color.red;
            Gizmos.DrawLine(targetObj.position, transform.position);
        }
    }
}
