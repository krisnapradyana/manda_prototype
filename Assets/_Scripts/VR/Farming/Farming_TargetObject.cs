using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Farming_TargetObject : MonoBehaviour
{
    [Header("General Settings")]
    private GeneralAttributes generalAttributes;
    [SerializeField] private Farming_CollectiblesManager collectiblesManager;

    [Header("Object Attributes")]
    private int objectHitPoint;
    [SerializeField] private int defaultHitPoint = 2, objectStrength = 2;

    [Header("Loot Settings")]
    private int numberOfLoot;
    [SerializeField] private int minLoot, maxLoot;

    [Header("Cooldown Settings")]
    private bool isCoolingdown;
    [SerializeField] private int minCooldown, maxCooldown;
    private int waitTime, endTime;

    [Header("Randomize NewTransform")]
    [SerializeField] private bool shouldRandomizeTransform = false;
    [Space(8)]
    [SerializeField] private bool shouldRandomizePos;
    private Transform minPosRef, maxPosRef;
    [Space(6)]
    [SerializeField] private bool shouldRandomizeSize;
    [SerializeField] private float minSize = 0.8f, maxSize = 2.2f;
    [Space(6)]
    [SerializeField] private bool shouldRandomizeRotation;
    [SerializeField] private float minYRotation = 0f, maxYRotation = 359f;

    [Header("Events")]
    public UnityEvent hitSuccessEvent, hitFailureEvent, hitDestroyEvent;

    [Header("Variants")]
    [SerializeField] private GameObject[] variantVarieties;
    [SerializeField] private GameObject selectedVariant;

    private Vector3 previousPos;

    private void Awake()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();
    }

    void Start()
    {
        variantVarieties = GetAllChildObjects(transform);
        objectHitPoint = defaultHitPoint;

        for (int i = 0; i < variantVarieties.Length; i++)
        {
            variantVarieties[i].SetActive(false);
        }

        minPosRef = generalAttributes.minThreshold;
        maxPosRef = generalAttributes.maxThreshold;

        OnSpawn();

        Debug.Log($"Current time: {GeneralAttributes.CurrentTime}");
    }

    void Update()
    {
        if (isCoolingdown)
        {
            OnCooldown();
        }
    }

    public void OnHit(int hitterStrength)
    {
        Debug.Log($"Transported HS: {hitterStrength}");
        if (hitterStrength >= objectStrength && objectHitPoint > 0)
        {
            objectHitPoint--;
            Debug.Log("Invoked");

            if (objectHitPoint > 0)
            {
                hitSuccessEvent.Invoke();
                //play audio, add effect, etc
            }
            else
            {
                hitDestroyEvent.Invoke();

                OnBrustResource();
            }
        }

        else
        {
            hitFailureEvent.Invoke();
            //item strength is insufficient
            //play audio, add effect, etc
        }
    }

    private void OnSpawn()
    {
        if (variantVarieties != null)
        {
            int i = UnityEngine.Random.Range(0, variantVarieties.Length);
            selectedVariant = variantVarieties[i];

            objectHitPoint = defaultHitPoint;

            if (shouldRandomizeTransform)
            {
                if (shouldRandomizeRotation)
                {
                    OnRotate();
                }
                if (shouldRandomizeSize)
                {
                    OnResize();
                }
                if (shouldRandomizePos && minPosRef != null && maxPosRef != null)
                {
                    OnReposition();
                }
            }

            selectedVariant.SetActive(true);
        }
        else
        {
            Debug.LogWarning("variant is not found, check it again!");
        }
    }

    private void OnRotate()
    {
        float newYRotation = UnityEngine.Random.Range(minYRotation, maxYRotation);
        transform.rotation = Quaternion.Euler(0f, newYRotation, 0f);
    }

    private void OnResize()
    {
        float newSize = UnityEngine.Random.Range(minSize, maxSize);
        transform.localScale = Vector3.one * newSize;
    }

    private void OnReposition()
    {
        float newX = UnityEngine.Random.Range(minPosRef.position.x, maxPosRef.position.x);
        float newY = UnityEngine.Random.Range(minPosRef.position.y, maxPosRef.position.y);
        float newZ = UnityEngine.Random.Range(minPosRef.position.z, maxPosRef.position.z);
        transform.position = new Vector3(newX, newY, newZ);
    }

    public void OnBrustResource()
    {
        previousPos = transform.position;

        for (int i = 0; i < variantVarieties.Length; i++)
        {
            variantVarieties[i].SetActive(false);
        }

        waitTime = UnityEngine.Random.Range(minCooldown, maxCooldown);
        numberOfLoot = UnityEngine.Random.Range(minLoot, maxLoot);

        collectiblesManager.CheckPrefabsAvailability();
        collectiblesManager.FetchFromPool(numberOfLoot, gameObject);

        endTime = (int)GeneralAttributes.CurrentTime + waitTime;
        isCoolingdown = true;
    }

    void OnCooldown()
    {
        if (GeneralAttributes.CurrentTime >= endTime)
        {
            isCoolingdown = false;

            OnSpawn();
        }
    }

    GameObject[] GetAllChildObjects(Transform parent)
    {
        List<GameObject> childrenList = new List<GameObject>();

        foreach (Transform child in parent)
        {
            childrenList.Add(child.gameObject);

            // Recursively add nested children
            childrenList.AddRange(GetAllChildObjects(child));
        }

        return childrenList.ToArray();
    }
}
