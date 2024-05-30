using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRRespawnOnFall : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    [SerializeField] private float moveSpeed = 5f, rotSpeed = 15f;

    //[SerializeField] private float YThreshold;
    [SerializeField] private int objectIndex;
    [SerializeField] private bool rotateOnReturn;
    [SerializeField] private UnityEvent onFallEvent;
    private Rigidbody rb;
    private GameObject objectToRetrieve;

    private Coroutine moveCoroutine;
    private Coroutine rotateCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor)
        {
            SnapToSocket();
        }
        else
        {
            transform.position = generalAttributes.xrPrevPos[objectIndex].position;
            transform.rotation = generalAttributes.xrPrevPos[objectIndex].rotation;
        }
    }

    /// <summary>
    /// is not used anymore, now using trigger with another object
    /// </summary>
    //private void Update()
    //{
    //    if (transform.position.y < YThreshold)
    //    {
    //        SnapToSocket();
    //    }
    //}

    public void SnapToSocket()
    {
        onFallEvent.Invoke();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StopAnyCoroutines(); // Stop any existing coroutines

        moveCoroutine = StartCoroutine(MoveToPrev());
        if (rotateOnReturn)
        {   
            rotateCoroutine = StartCoroutine(RotateToPrev());
        }
    }

    public void StopAnyCoroutines()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }

        if (rotateCoroutine != null)
        {
            StopCoroutine(rotateCoroutine);
            rotateCoroutine = null;
        }
    }

    IEnumerator MoveToPrev()
    {
        while (transform.position != generalAttributes.xrPrevPos[objectIndex].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, generalAttributes.xrPrevPos[objectIndex].position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotateToPrev()
    {
        while (transform.rotation != generalAttributes.xrPrevPos[objectIndex].rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, generalAttributes.xrPrevPos[objectIndex].rotation, rotSpeed);
            yield return null;
        }
    }
}
