using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamLookAtWithIgnore : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    public bool shouldLookAt = true;
    [SerializeField] private bool ignoreX, ignoreY, ignoreZ;

    private void Start()
    {
        shouldLookAt = true;
    }

    void Update()
    {
        if (shouldLookAt)
        {
            Vector3 newTarget = GenerateNewTarget(generalAttributes.centerEyeObject);
            transform.LookAt(newTarget);
        }
    }

    private Vector3 GenerateNewTarget(GameObject targetToLookAt)
    {
        float newX = ignoreX ? transform.position.x : targetToLookAt.transform.position.x;
        float newY = ignoreY ? transform.position.y : targetToLookAt.transform.position.y;
        float newZ = ignoreZ ? transform.position.z : targetToLookAt.transform.position.z;

        return new Vector3(newX, newY, newZ);
    }
    public void toggleShouldLookAt(bool newValue)
    {
        shouldLookAt = newValue;
    }
}
