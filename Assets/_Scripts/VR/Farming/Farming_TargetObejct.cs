using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class Farming_TargetObejct : MonoBehaviour
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

    private Vector3 previousPos;
    [SerializeField] private bool shouldRandomizePos;
    private float newPosX, newPosY, newPosZ;
    [SerializeField] private Transform minThreshold, maxThreshold;

    public UnityEvent hitSuccessEvent, hitFailureEvent, hitDestroyEvent;

    private MeshRenderer meshRenderer;
    private Collider collider;

    private void Awake()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();

        if (gameObject.tag == "RockTag")
        {
            collectiblesManager = generalAttributes.RockPrefabParent.GetComponent<Farming_CollectiblesManager>();
        }
        else if (gameObject.tag == "LogTag")
        {
            collectiblesManager = generalAttributes.LogPrefabParent.GetComponent<Farming_CollectiblesManager>();
        }
    }

    void Start()
    {
        previousPos = transform.position;
        objectHitPoint = defaultHitPoint;
        if (collectiblesManager == null)
        {
            Debug.LogError("Farming_CollectiblesManager not found!");
        }

        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError($"No MeshRenderer at {gameObject.name}!");
        }

        collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError($"No Collider at {gameObject.name}!");
        }
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

    public void OnBrustResource()
    {
        previousPos = transform.position;

        ToggleVisibility(false);
        waitTime = Random.Range(minCooldown, maxCooldown);
        numberOfLoot = Random.Range(minLoot, maxLoot);

        collectiblesManager.CheckPrefabsAvailability();
        collectiblesManager.FetchFromPool(numberOfLoot, gameObject);

        endTime = (int)GeneralAttributes.CurrentTime + waitTime;
        isCoolingdown = true;
    }

    public void OnRespawn()
    {
        if (shouldRandomizePos)
        {
            if (minThreshold != null && maxThreshold != null)
            {
                // Randomize X, Y, Z within the defined threshold
                newPosX = Random.Range(minThreshold.position.x, maxThreshold.position.x);
                newPosY = Random.Range(minThreshold.position.y, maxThreshold.position.y);
                newPosZ = Random.Range(minThreshold.position.z, maxThreshold.position.z);

                // Set the new position
                transform.position = new Vector3(newPosX, newPosY, newPosZ);
            }
        }
        else
        {
            transform.position = previousPos;
        }

        objectHitPoint = defaultHitPoint;
        ToggleVisibility(true);
    }


    void OnCooldown()
    {
        if (GeneralAttributes.CurrentTime >= endTime)
        {
            isCoolingdown = false;

            OnRespawn();
        }
    }

    void ToggleVisibility(bool value)
    {
        meshRenderer.enabled = value;
        collider.enabled = value;
    }
}
