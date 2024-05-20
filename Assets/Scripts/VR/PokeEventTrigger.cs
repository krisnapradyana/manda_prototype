using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PokeEventTrigger : MonoBehaviour
{
    [SerializeField] GameObject fingerToFolow;
    [SerializeField] GameObject pokeDebug;
    [SerializeField] GameObject pokeDebugParent;
    [SerializeField] GameObject targetTouch;   

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        Debug.Log("Checking Tag");
        if (other.CompareTag("LandToTouch"))
        {
            //PerformRaycast();
            Debug.Log(other.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null)
            return;

        Debug.Log(other.gameObject.name);
    }

    private void Update()
    {
        transform.position = fingerToFolow.transform.position;
    }

    void PerformRaycast()
    {
        // Get the transform of the controller or hand

        Transform rayOrigin = transform;

        // Create a ray from the controller's position and direction
        Ray ray = new Ray(rayOrigin.position, rayOrigin.forward);
        RaycastHit hitInfo;

        // Perform the raycast
        if (Physics.Raycast(ray, out hitInfo))
        {
            // Log the position where the ray hit
            Debug.Log("Poke Location: " + hitInfo.point);

            pokeDebug.transform.position = hitInfo.point;            
        }
    }
}
