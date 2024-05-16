using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomEventDispatcjer : MonoBehaviour
{
    public UnityEvent destroyEvents;
    public UnityEvent disableEvents;
    public UnityEvent enableEvents;
    public UnityEvent invisibleEvents;
    public UnityEvent visibleEvents;

    private void OnDestroy()
    {
        destroyEvents.Invoke();
    }
    private void OnDisable()
    {
        disableEvents.Invoke();
    }
    private void OnEnable()
    {
        enableEvents.Invoke();
    }
    private void OnBecameInvisible()
    {
        invisibleEvents.Invoke();
    }
    private void OnBecameVisible()
    {
        visibleEvents.Invoke();
    }
}
