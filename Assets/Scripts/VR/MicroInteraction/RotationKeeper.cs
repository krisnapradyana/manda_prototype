using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationKeeper : MonoBehaviour
{
    public Transform headObj, menuObj;
    [SerializeField] private Vector3 initialRotValue;

    private float angleDifference;
    [SerializeField, Range(0, 45)] private float angleThreshold;
    public bool shouldRotate;
    [SerializeField, Range(0, 10)] private float baseSpeed;
    [SerializeField, Range(0, 5)] private float accelerationFactor;

    private void Awake()
    {
        initialRotValue = menuObj.rotation.eulerAngles;
    }

    private void Update()
    {
        shouldRotate = CheckAngleDifference();
        if (shouldRotate)
        {
            RotateTowardsTarget();
        }
        else
        {
            //OnDrawGizmos();
        }
    }

    private void RotateTowardsTarget()
    {
        float rotationSpeed = baseSpeed + (Mathf.Abs(angleDifference) * accelerationFactor);
        Quaternion targetRotation = Quaternion.Euler(initialRotValue.x, headObj.eulerAngles.y, initialRotValue.z);
        menuObj.rotation = Quaternion.RotateTowards(menuObj.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckAngleDifference()
    {
        float headYRotation = headObj.eulerAngles.y;
        float menuYRotation = menuObj.eulerAngles.y;
        
        angleDifference = CalculateShortestAngleDifference(headYRotation + initialRotValue.y, menuYRotation);

        return Mathf.Abs(angleDifference) >= angleThreshold;
    }

    private float CalculateShortestAngleDifference(float angle1, float angle2)
    {
        float diff = (angle1 - angle2 + 180) % 360 - 180;
        return diff < -180 ? diff + 360 : diff;
    }

    private void OnDrawGizmos()
    {
        if (headObj != null && menuObj != null)
        {
            // Draw a line between headObj and menuObj
            Gizmos.color = Color.red;
            Gizmos.DrawLine(headObj.position, menuObj.position);
        }
    }
}
