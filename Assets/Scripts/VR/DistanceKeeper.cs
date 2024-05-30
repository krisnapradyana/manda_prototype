using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceKeeper : MonoBehaviour
{
    public Transform handObj, mapObj;
    private float distanceBetweenObject;
    [SerializeField, Range(0, 50)] private float distanceThreshold;
    public bool shouldMove;
    [SerializeField, Range(0, 10)] private float baseSpeed;
    [SerializeField, Range(0, 5)] private float accelerationFactor;

    private void Update()
    {
        shouldMove = checkDistance();
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
        Vector3 newTarget = new Vector3(handObj.position.x, (handObj.position.y + distanceThreshold), handObj.position.z);

        mapObj.position = Vector3.MoveTowards(mapObj.position, newTarget, step);
    }

    private bool checkDistance()
    {
        Vector3 A = handObj.position;
        Vector3 B = mapObj.position;

        distanceBetweenObject = Vector3.Distance(A, B);

        return distanceBetweenObject >= distanceThreshold || distanceBetweenObject/2 <= distanceThreshold;
    }

    private void OnDrawGizmos()
    {
        if (handObj != null && mapObj != null)
        {
            // Draw a line between handObj and mapObj
            Gizmos.color = Color.red;
            Gizmos.DrawLine(handObj.position, mapObj.position);
        }
    }
}
