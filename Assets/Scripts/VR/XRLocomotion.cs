using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRLocomotion : MonoBehaviour
{
    public GeneralAttributes generalAttributes;

    [SerializeField] private float strideDistance, strideDelay;
    [SerializeField] private float rotDegree, rotDelay;
    [SerializeField] private Coroutine moveToFront, moveToBack, rotToRight, rotToLeft;



    private void Awake()
    {
        generalAttributes.xrPrevPos = generalAttributes.xrOrigin.transform;
    }

    public void MoveForward()
    {
        StopMoving();
        moveToFront = StartCoroutine(MoveXR(1));
    }

    public void MoveBackward()
    {
        StopMoving();
        moveToBack = StartCoroutine(MoveXR(-1));
    }

    public void TurnRight()
    {
        StopTurning();
        rotToRight = StartCoroutine(RotateXR(1));
    }

    public void TurnLeft()
    {
        StopTurning();
        rotToLeft = StartCoroutine(RotateXR(-1));
    }

    public void StopMoving()
    {
        Debug.Log("StopMoving");
        if (moveToFront != null)
        {
            StopCoroutine(moveToFront);
            moveToFront = null;
            generalAttributes.xrPrevPos = generalAttributes.xrOrigin.transform;
        }

        if (moveToBack != null)
        {
            StopCoroutine(moveToBack);
            moveToBack = null;
            generalAttributes.xrPrevPos = generalAttributes.xrOrigin.transform;
        }
    }

    public void StopTurning()
    {
        Debug.Log("StopTurning");
        if (rotToRight != null)
        {
            StopCoroutine(rotToRight);
            rotToRight = null;
            generalAttributes.xrPrevPos = generalAttributes.xrOrigin.transform;
        }

        if (rotToLeft != null)
        {
            StopCoroutine(rotToLeft);
            rotToLeft = null;
            generalAttributes.xrPrevPos = generalAttributes.xrOrigin.transform;
        }
    }

    private IEnumerator MoveXR(int modifierValue)
    {
        while (true)
        {
            generalAttributes.xrOrigin.transform.Translate(Vector3.forward * (strideDistance * modifierValue) * Time.deltaTime);

            yield return new WaitForSeconds(strideDelay);
        }
    }

    private IEnumerator RotateXR(int modifierValue)
    {
        while (true)
        {
            float snapAngle = rotDegree * modifierValue;
            generalAttributes.xrOrigin.transform.Rotate(Vector3.up * snapAngle);

            yield return new WaitForSeconds(rotDelay);
        }
    }
}
