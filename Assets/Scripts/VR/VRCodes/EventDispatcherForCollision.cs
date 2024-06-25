using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] listOfInteractables;

    public UnityEvent collisionEnterEvent;
    public UnityEvent collisionExitEvent;

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < listOfInteractables.Length; i++)
        {
            if (collision.gameObject == listOfInteractables[i])
            {
                collisionEnterEvent.Invoke();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        foreach (GameObject interactableObject in listOfInteractables)
        {
            if (collision.gameObject == interactableObject)
            {
                collisionExitEvent.Invoke();
            }
        }
    }
}
