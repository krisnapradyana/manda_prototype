using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
//using System.Random;

//attach this script to the prefab
//it is intended to manage one item/resouce variety of mesh and number of child
public class Farming_ItemBehaviour : MonoBehaviour
{
    private GeneralAttributes generalAttributes;
    [SerializeField] private int minForce, maxForce;
    [SerializeField] GameObject targetGameObject;
    [SerializeField] private float minimumDistance;

    public GameObject[] prefabVarieties;
    private int selectedPrefabID;

    private Rigidbody rb;
    private bool isWorking;
    [SerializeField] private int waitTime;
    private int endTime;
    private bool isCoolingdown;
    private bool canMoveToTarget;
    private bool shouldMoveToTarget;
    [SerializeField] private float speed = 5;

    private void Start()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();

        if (generalAttributes.isRightHanded)
        {
            targetGameObject = generalAttributes.leftHandAnchor;
        }
        else
        {
            targetGameObject = generalAttributes.rightHandAnchor;
        }

        for (int i = 0; i < prefabVarieties.Length; i++)
        {
            prefabVarieties[i].SetActive(false);
        }

        gameObject.SetActive(false);
        isWorking = true;
    }

    void Update()
    {
        if (isCoolingdown)
        {
            HandleCooldown();
        }

        if (canMoveToTarget && !shouldMoveToTarget)
        {
            CheckTargetDistance();
        }

        if (shouldMoveToTarget)
        {
            MoveToTarget();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject.activeSelf && collision.gameObject.CompareTag("Ground"))
        {
            StartCooldown();

            canMoveToTarget = true;
        }

        if (gameObject.activeSelf && collision.gameObject.CompareTag("Player"))
        {
            StartScaleDown();
        }
    }

    private void StartScaleDown()
    {
        throw new NotImplementedException();
    }

    private void OnEnable()
    {
        if (isWorking)
        {
            selectedPrefabID = UnityEngine.Random.Range(0, prefabVarieties.Length);
            var selectedObject = prefabVarieties[selectedPrefabID];
            selectedObject.SetActive(true);

            rb = selectedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
                int RandomedForce = UnityEngine.Random.Range(minForce, maxForce);

                transform.rotation = Quaternion.Euler(30, UnityEngine.Random.Range(0, 359), 0);
                rb.AddForce(transform.up * RandomedForce, ForceMode.Impulse);
            }
        }
    }

    private void OnDisable()
    {
        if (isWorking)
        {
            rb.isKinematic = true;
            prefabVarieties[selectedPrefabID].SetActive(true);
            gameObject.SetActive(false);

            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            rb = null;
        }
    }

    private void StartCooldown()
    {
        endTime = (int)GeneralAttributes.CurrentTime + waitTime;
        isCoolingdown = true;
    }

    void HandleCooldown()
    {
        if (GeneralAttributes.CurrentTime >= endTime && !shouldMoveToTarget)
        {
            isCoolingdown = false;

            gameObject.SetActive(false);
        }
        else if (GeneralAttributes.CurrentTime >= endTime && shouldMoveToTarget)
        {
            isCoolingdown = false;
        }
    }

    void CheckTargetDistance()
    {
        float distance = Vector3.Distance(transform.position, targetGameObject.transform.position);
        //Debug.Log(distance);
        if (distance <= minimumDistance)
        {
            shouldMoveToTarget = true;
        }
    }
    void MoveToTarget()
    {
        if (transform.position != targetGameObject.transform.position && targetGameObject != null)
        {
            float step = speed * Time.deltaTime;
            Quaternion startRot = transform.rotation;
            Quaternion endRot = targetGameObject.transform.rotation;


            // Move the object towards the target's position
            transform.position = Vector3.MoveTowards(transform.position, targetGameObject.transform.position, step);
            transform.rotation = Quaternion.RotateTowards(startRot, endRot, step);
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, step);
        }

        else if (transform.position == targetGameObject.transform.position)
        {
            CompleteMove();
        }
    }

    void CompleteMove()
    {
        canMoveToTarget = false;
        shouldMoveToTarget = false;

        OnDisable();
    }
}
