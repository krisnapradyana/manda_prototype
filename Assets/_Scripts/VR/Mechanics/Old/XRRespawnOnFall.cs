//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Events;

//public class XRRespawnOnFall : MonoBehaviour
//{
//    public GeneralAttributes generalAttributes;
//    public CamLookAtWithIgnore camLook;

//    [SerializeField] private float moveSpeed = 5f, rotSpeed = 15f;

//    //[SerializeField] private float YThreshold;
//    [SerializeField] private float invokeDelay;
//    public int objectIndex;
//    [SerializeField] private bool rotateOnReturn;
//    [SerializeField] private UnityEvent onFallEvent;
//    private Rigidbody rb;

//    private Coroutine moveCoroutine;
//    private Coroutine rotateCoroutine;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();

//        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor)
//        {
//            SnapToSocket();
//        }
//        else
//        {
//            transform.position = generalAttributes.xrPrevPos[objectIndex].position;
//            transform.rotation = generalAttributes.xrPrevPos[objectIndex].rotation;
//        }

//        Debug.Log($"Initial Position for {gameObject.name}: {transform.position}");
//        Debug.Log($"Initial Velocity for {gameObject.name}: {rb.velocity}");
//    }

//    public void SnapToSocket()
//    {
//        StopAnyCoroutines(); // Stop any existing coroutines

//        moveCoroutine = StartCoroutine(MoveToPrev());
//        if (rotateOnReturn)
//        {
//            rotateCoroutine = StartCoroutine(RotateToPrev());
//        }
//    }

//    public void StopAnyCoroutines()
//    {
//        if (moveCoroutine != null)
//        {
//            StopCoroutine(moveCoroutine);
//            moveCoroutine = null;
//        }

//        if (rotateCoroutine != null)
//        {
//            StopCoroutine(rotateCoroutine);
//            rotateCoroutine = null;
//        }
//    }

//    IEnumerator MoveToPrev()
//    {
//        rb.velocity = Vector3.zero;
//        rb.angularVelocity = Vector3.zero;

//        yield return new WaitForSeconds(invokeDelay);

//        onFallEvent.Invoke();

//        while (transform.position != generalAttributes.xrPrevPos[objectIndex].position)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, generalAttributes.xrPrevPos[objectIndex].position, moveSpeed * Time.deltaTime);
//            yield return null;
//        }

//        camLook.toggleShouldLookAt(true);
//    }

//    IEnumerator RotateToPrev()
//    {
//        while (transform.rotation != generalAttributes.xrPrevPos[objectIndex].rotation)
//        {
//            transform.rotation = Quaternion.RotateTowards(transform.rotation, generalAttributes.xrPrevPos[objectIndex].rotation, rotSpeed);
//            yield return null;
//        }
//    }
//}
