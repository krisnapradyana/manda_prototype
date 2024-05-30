using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForCollision : MonoBehaviour
{
    [SerializeField] private GameObject interactableObject;

    public UnityEvent collisionEnterEvent;
    public UnityEvent collisionExitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == interactableObject)
        {
            collisionEnterEvent.Invoke();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == interactableObject)
        {
            collisionExitEvent.Invoke();
        }
    }
}
