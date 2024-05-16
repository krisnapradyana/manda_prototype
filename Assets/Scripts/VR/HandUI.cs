using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandUI : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    [SerializeField] private bool rotateX, rotateY, rotateZ;

    [Header("Read Only")]
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private Quaternion targetRot;
    public bool rightControllerIsVisible, leftControllerIsVisible, panelIsCentered;

    public void SetBoolLeft(bool value)
    {
        leftControllerIsVisible = value;
    }
    public void SetBooleanRight(bool value)
    {
        rightControllerIsVisible = value;
    }


    private void Update()
    {
        CheckTarget();

        if (!panelIsCentered)
        {
            Debug.LogWarning("Not Centered");
            generalAttributes.panelParent.transform.position = targetPos;
            //generalAttributes.panelParent.transform.LookAt(generalAttributes.TopAnchor.transform);
            generalAttributes.panelParent.transform.rotation = targetRot;
            //ApplyRotation();

            //generalAttributes.panelParent.transform.LookAt(generalAttributes.centerEyeObject.transform);

            //MoveTowards(targetPos);
            //RotateTowards(targetYRot);

            //if (ReachedPosition(targetPos) && ReachedRotation(targetYRot))
            //{
            //    panelIsCentered = true;
            //}
        }
        else
        {
            Debug.LogWarning("Centered");
            CheckPositionToTarget();
        }
    }

    private void CheckTarget()
    {
        // using controller tracking
        if (leftControllerIsVisible == true)
        {
            targetPos = generalAttributes.pivotLeftController.transform.position;
            targetRot = generalAttributes.pivotLeftController.transform.rotation;
            //targetYRot = generalAttributes.centerEyeObject.transform.rotation.y;
        }

        // using hand tracking
        else
        {
            targetPos = generalAttributes.pivotLeftHand.transform.position;
            targetRot = generalAttributes.pivotLeftHand.transform.rotation;
            //targetYRot = generalAttributes.centerEyeObject.transform.rotation.y;
        }
    }

    private void CheckPositionToTarget()
    {
        if (!ReachedPosition(targetPos) || !ReachedRotation(targetRot))
        {
            Debug.LogWarning("Different");
            panelIsCentered = false;
        }
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, generalAttributes.panelParent.transform.position) < 0.1f;
    }

    private bool ReachedRotation(Quaternion targetRotation)
    {
        float tolerance = 0.1f;
        return Quaternion.Angle(generalAttributes.panelParent.transform.rotation, targetRotation) < tolerance;
    }

    private void ApplyRotation()
    {
        Vector3 currentRotation = generalAttributes.panelParent.transform.rotation.eulerAngles;
        Vector3 targetRotation = targetRot.eulerAngles;

        float newX = rotateX ? targetRotation.x : currentRotation.x;
        float newY = rotateY ? targetRotation.y : currentRotation.y;
        float newZ = rotateZ ? targetRotation.z : currentRotation.z;

        generalAttributes.panelParent.transform.rotation = Quaternion.Euler(newX, newY, newZ);
    }

}
