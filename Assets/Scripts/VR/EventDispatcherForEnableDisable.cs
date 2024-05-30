using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForEnableDisable : MonoBehaviour
{
    public UnityEvent destroyEvents;
    public UnityEvent disableEvents;
    public UnityEvent enableEvents;

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
}
