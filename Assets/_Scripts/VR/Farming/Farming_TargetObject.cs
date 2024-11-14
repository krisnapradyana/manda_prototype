using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Farming_TargetObject : MonoBehaviour
{
    private GeneralAttributes generalAttributes;
    private Farming_CollectiblesManager collectiblesManager;

    private int objectHitPoint;
    [SerializeField] private int defaultHitPoint = 2, objectStrength = 2;

    [SerializeField] private int minLoot, maxLoot;
    private int numberOfLoot;

    [SerializeField] private int minCooldown, maxCooldown;
    private int waitTime, endTime;
    private bool isCoolingdown;

    public bool canBeShown;
    private Vector3 previousPos;
    [SerializeField] private bool shouldRandomizePos;
    private float newPosX, newPosY, newPosZ;
    private Transform minPosRef, maxPosRef;

    public UnityEvent hitSuccessEvent, hitFailureEvent, hitDestroyEvent;

    private GameObject[] variantVarieties;
    private GameObject selectedVariant;

    private void Awake()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();

        if (gameObject.tag == "RockTag")
        {
            collectiblesManager = generalAttributes.RockPrefab_Parent.GetComponent<Farming_CollectiblesManager>();
        }
        else if (gameObject.tag == "LogTag")
        {
            collectiblesManager = generalAttributes.LogPrefab_Parent.GetComponent<Farming_CollectiblesManager>();
        }
    }

    void Start()
    {
        variantVarieties = GetAllChildObjects(transform);
        objectHitPoint = defaultHitPoint;

        for (int i = 0; i < variantVarieties.Length; i++)
        {
            variantVarieties[i].SetActive(false);
        }

        minPosRef = generalAttributes.MinThreshold;
        maxPosRef = generalAttributes.MaxThreshold;

        OnSpawn();
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
        if (hitterStrength >= objectStrength && objectHitPoint > 0)
        {
            objectHitPoint--;

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
            selectedVariant.SetActive(true);

            RandomizeNewTransfrom();

        }
        else
        {
            Debug.LogWarning("variant is not found, check it again!");
        }
    }

    private void RandomizeNewTransfrom()
    {

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
