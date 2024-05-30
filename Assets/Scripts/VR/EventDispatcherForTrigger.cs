using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] listOfInteractables;

    public UnityEvent triggerEventEnter;
    public UnityEvent triggerEventExit;


    private void OnTriggerEnter(Collider other)
    {
        foreach (GameObject interactableObject in listOfInteractables)
        {
            if (other.gameObject == interactableObject)
            {
                triggerEventEnter.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (GameObject interactableObject in listOfInteractables)
        {
            if (other.gameObject == interactableObject)
            {
                triggerEventExit.Invoke();
            }
        }
    }
}
