using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRRespawnOnFall : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    [SerializeField] private float moveSpeed = 5f, rotSpeed = 15f;

    [SerializeField] private float YThreshold;
    [SerializeField] private bool rotateOnReturn;
    [SerializeField] private UnityEvent onFallEvent;
    private Rigidbody rb;
    private GameObject objectToRetrieve;

    private void Start()
    {
        rb = generalAttributes.playerChar.GetComponent<Rigidbody>();

        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor)
        {
            SnapToSocket();
        }
        else
        {
            generalAttributes.xrPrevPos.position = generalAttributes.initialPoint.position;
            generalAttributes.xrPrevPos.rotation = generalAttributes.initialPoint.rotation;
        }
    }

    private void Update()
    {
        if (transform.position.y < YThreshold)
        {
            SnapToSocket();
        }
    }

    public void SnapToSocket()
    {
        onFallEvent.Invoke();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        StartCoroutine(MoveToPrev());
        if (rotateOnReturn)
        {
            StartCoroutine(RotateToPrev());
        }
    }

    IEnumerator MoveToPrev()
    {
        while (transform.position != generalAttributes.xrPrevPos.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, generalAttributes.xrPrevPos.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator RotateToPrev()
    {
        while (transform.rotation != generalAttributes.xrPrevPos.rotation)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, generalAttributes.xrPrevPos.rotation, rotSpeed);
            yield return null;
        }
    }
}
