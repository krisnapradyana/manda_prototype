using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventDispatcherForEnterRegion : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    [SerializeField] private Vector3 minVector, maxVector;
    [SerializeField] private bool hasEntered;

    public UnityEvent EnterRegionEvent;
    public UnityEvent ExitRegionEvent;

  
    private void Update()
    {
        foreach (GameObject targetableObject in generalAttributes.playerChar)
        {
            Vector3 targetPosition = targetableObject.transform.position;

            if (IsWithinRegion(targetPosition) && !hasEntered)
            {
                EnterRegionEvent.Invoke();
                hasEntered = true;
            }
            else if (!IsWithinRegion(targetPosition) && hasEntered)
            {
                ExitRegionEvent.Invoke();
                hasEntered = false;
            }
        }
    }

    bool IsWithinRegion(Vector3 position)
    {
        return position.x >= minVector.x && position.x <= maxVector.x &&
               position.y >= minVector.y && position.y <= maxVector.y &&
               position.z >= minVector.z && position.z <= maxVector.z;
    }
}
