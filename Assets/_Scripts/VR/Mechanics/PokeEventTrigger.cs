using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PokeEventTrigger : MonoBehaviour
{
    [SerializeField] GameObject fingerToFolow;

    private void Update()
    {
        transform.position = fingerToFolow.transform.position;
    }
}
