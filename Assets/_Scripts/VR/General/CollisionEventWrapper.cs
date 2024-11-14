using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEventWrapper : MonoBehaviour
{
    [Header("Detection Settings")]
    [Tooltip("Use this to specify detection by tag or by attached GameObjects.")]
    public bool detectByTag = true;

    [Tooltip("Specify the tags that will trigger the collision event.")]
    public string[] detectionTags;

    [Tooltip("Specify the GameObjects that will trigger the collision event.")]
    public GameObject[] detectionObjects;

    [Header("Collision Events")]
    [Tooltip("Triggered when a valid object enters the collider.")]
    public UnityEvent onCollisionEnter;

    [Tooltip("Triggered when a valid object exits the collider.")]
    public UnityEvent onCollisionExit;

    private void OnCollisionEnter(Collision collision)
    {
        if (IsDetected(collision.gameObject))
        {
            onCollisionEnter.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (IsDetected(collision.gameObject))
        {
            onCollisionExit.Invoke();
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
