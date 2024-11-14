using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventWrapper : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("Use this to specify detection by tag or by attached GameObjects.")]
    public bool detectByTag = true;

    [Tooltip("Specify the tags that will trigger the event.")]
    public string[] detectionTags;

    [Tooltip("Specify the GameObjects that will trigger the event.")]
    public GameObject[] detectionObjects;

    [Header("Trigger Events")]
    [Tooltip("Triggered when a valid object enters the trigger.")]
    public UnityEvent onTriggerEnter;

    [Tooltip("Triggered when a valid object exits the trigger.")]
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (IsDetected(other.gameObject))
        {
            onTriggerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsDetected(other.gameObject))
        {
            onTriggerExit.Invoke();
        }
    }

    private bool IsDetected(GameObject other)
    {
        if (detectByTag)
        {
            foreach (string tag in detectionTags)
            {
                if (other.CompareTag(tag)) return true;
            }
        }
        else
        {
            foreach (GameObject obj in detectionObjects)
            {
                if (other == obj) return true;
            }
        }
        return false;
    }
}
