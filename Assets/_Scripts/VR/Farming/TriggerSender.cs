using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSender : MonoBehaviour
{
    [SerializeField] private GameObject parentObject;

    private Farming_Tools targetTools;
    private GameObject lastCollision;

    private void Start()
    {
        targetTools = parentObject.GetComponent<Farming_Tools>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == targetTools.targetTag)
        {
            targetTools.invokeTarget(lastCollision);
        }
    }
}
