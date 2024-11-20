using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Farming_Tools : MonoBehaviour
{
    private GeneralAttributes generalAttributes;

    public string targetTag = "TargetTag";
    [Range(1,10)]
    public int toolsStrength;

    private GameObject lastCollision;
    private Vector3 originalSize;
    private Rigidbody rigidbody;

    [SerializeField] private int waitTime;
    private int endTime;
    private bool isCoolingdown;
    private GameObject targetObject;

    private void Awake()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();
    }

    private void OnDisable()
    {
        rigidbody.isKinematic = true;
    }

    private void OnEnable()
    {
        if (generalAttributes != null)
        {
            ResetTransform();
        }
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isCoolingdown)
        {
            HandleCooldown();
        }
    }     

    private void ResetTransform()
    {
        if (generalAttributes == null)
        {
            Debug.LogError("generalAttributes is null");
            return;
        }

        if (generalAttributes.centerEyeAnchor == null)
        {
            Debug.LogError("centerEyeAnchor in generalAttributes is null");
            return;
        }

        transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.Euler(0, generalAttributes.centerEyeAnchor.transform.rotation.eulerAngles.y, 0);
        transform.localRotation = Quaternion.identity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            lastCollision = other.gameObject;
            invokeTarget(lastCollision);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            //Debug.Log("Trigger Exited by: " + other.name);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            endTime = (int)GeneralAttributes.CurrentTime + waitTime;
            isCoolingdown = true;
        }
    }

    void HandleCooldown()
    {
        if (GeneralAttributes.CurrentTime >= endTime)
        {
            isCoolingdown = false;

            transform.parent.gameObject.SetActive(false);
            transform.parent.gameObject.transform.localPosition = Vector3.zero;
            transform.parent.gameObject.transform.localRotation = Quaternion.Euler(0, -90, 90);
        }
    }

    public void invokeTarget(GameObject gameObject)
    {
        // Call the OnHit function from the FarmableData script
        TriggerReceiver objectData = gameObject.GetComponent<TriggerReceiver>();
        if (objectData != null)
        {
            objectData.TriggerTransporter(toolsStrength); // Pass the objectStrength to the OnHit function
        }
    }
}
