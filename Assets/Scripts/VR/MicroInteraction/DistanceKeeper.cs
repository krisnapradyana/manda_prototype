using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceKeeper : MonoBehaviour
{
    public GeneralAttributes generalAttributes;

    private float distanceBetweenObject;
    [SerializeField, Range(0, 50)] private float distanceThreshold;
    public bool shouldMove;
    [SerializeField, Range(0, 5)] private float baseSpeed;
    [SerializeField, Range(0, 10)] private float accelerationFactor;

    private void Update()
    {
        shouldMove = checkDistance();
        
        if (shouldMove)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        float movementSpeed = baseSpeed + (distanceBetweenObject * accelerationFactor);
        float step = movementSpeed * Time.deltaTime;
        Vector3 newTarget = new Vector3(generalAttributes.pivotPos.position.x, (generalAttributes.pivotPos.position.y + distanceThreshold), generalAttributes.pivotPos.position.z);

        generalAttributes.playerCharPlatform.transform.position = Vector3.MoveTowards(generalAttributes.playerCharPlatform.transform.position, newTarget, step);
    }

    private bool checkDistance()
    {
        Vector3 A = generalAttributes.pivotPos.position;
        Vector3 B = generalAttributes.playerCharPlatform.transform.position;

        distanceBetweenObject = Vector3.Distance(A, B);

        return distanceBetweenObject >= distanceThreshold || distanceBetweenObject/2 <= distanceThreshold;
    }
}
