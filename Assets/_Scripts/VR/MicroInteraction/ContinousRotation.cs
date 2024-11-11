using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousRotation : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up;
    public float rotationSpeed = 12f;

    private Coroutine rotationCoroutine;

    private void OnEnable()
    {
        StartRotation();
    }
    private void OnDisable()
    {
        StopRotation();
    }

    public void StartRotation()
    {
        if (rotationCoroutine == null)
        {
            rotationCoroutine = StartCoroutine(RotateContinuously());
        }
    }
    public void StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
            rotationCoroutine = null;
        }
    }

    private IEnumerator RotateContinuously()
    {
        while (true)
        {
            transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
