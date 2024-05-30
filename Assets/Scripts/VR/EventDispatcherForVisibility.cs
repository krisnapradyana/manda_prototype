using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForVisibility : MonoBehaviour
{
    public UnityEvent invisibleEvents;
    public UnityEvent visibleEvents;

    private void OnBecameInvisible()
    {
        invisibleEvents.Invoke();
    }
    private void OnBecameVisible()
    {
        visibleEvents.Invoke();
    }
}
