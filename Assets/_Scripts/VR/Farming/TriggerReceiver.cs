using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerReceiver : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;
    private Farming_TargetObject targetObject;
    private void Awake()
    {
        targetObject = parentObject.GetComponent<Farming_TargetObject>();
    }

    public void TriggerTransporter(int HitterStrength)
    {
        Debug.Log("Triggered");
        targetObject.OnHit(HitterStrength);
    }
}
