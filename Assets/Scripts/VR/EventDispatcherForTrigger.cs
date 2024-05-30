using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForTrigger : MonoBehaviour
{
    [SerializeField] private GameObject interactableObject;

    public UnityEvent triggerEventEnter;
    public UnityEvent triggerEventExit;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == interactableObject)
        {
            triggerEventEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == interactableObject)
        {
            triggerEventExit.Invoke();
        }
    }
}
