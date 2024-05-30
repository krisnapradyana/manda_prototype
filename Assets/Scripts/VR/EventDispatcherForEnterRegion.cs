using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForEnterRegion : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    [SerializeField] private Vector3 minVector, maxVector;

    public UnityEvent EnterRegionEvent;
    public UnityEvent ExitRegionEvent;

  
    private void Update()
    {
        Vector3 targetPosition = generalAttributes.playerChar.transform.position;

        
        if (IsWithinRegion(targetPosition))
        {
            EnterRegionEvent.Invoke();
        }
        else
        {
            ExitRegionEvent.Invoke();
        }
    }

    bool IsWithinRegion(Vector3 position)
    {
        return position.x >= minVector.x && position.x <= maxVector.x &&
               position.y >= minVector.y && position.y <= maxVector.y &&
               position.z >= minVector.z && position.z <= maxVector.z;
    }
}
