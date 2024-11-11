using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Farming_Tools : MonoBehaviour
{
    public string targetTag = "TargetTag";
    [Range(1,10)]
    public int toolsStrength;
    public GameObject lastCollision;

    Vector3 originalSize;
    Rigidbody rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();


        OnDisable();
    }

    private void OnDisable()
    {
        Debug.LogWarning($"{gameObject.name} kinematic: {rigidbody.isKinematic}");
        ResetTransform();
        rigidbody.isKinematic = true;
    }   

    private void OnEnable()
    {
        ResetTransform();
    }

    private void ResetTransform()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        //transform.localScale = originalSize;
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

    void invokeTarget(GameObject gameObject)
    {
        // Call the OnHit function from the FarmableData script
        Farming_TargetObejct objectData = gameObject.GetComponent<Farming_TargetObejct>();
        if (objectData != null)
        {
            objectData.OnHit(toolsStrength); // Pass the objectStrength to the OnHit function
        }
    }

    public void ToggleKinematic(bool value)
    {
        if (rigidbody.isKinematic != value)
        {
            rigidbody.isKinematic = value;
        }
    }

}
