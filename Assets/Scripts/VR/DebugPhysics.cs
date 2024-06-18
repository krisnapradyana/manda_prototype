using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPhysics : MonoBehaviour
{
    public GameObject[] objectsToDebug;
    public float logInterval = 1.0f; // Interval in seconds to update the properties display
    private string[] debugInfo;
    private Vector2 scrollPosition;

    private void Start()
    {
        // Initialize the debug information array
        debugInfo = new string[objectsToDebug.Length];
        // Start the coroutine to update properties at regular intervals
        StartCoroutine(UpdateProperties());
    }

    private IEnumerator UpdateProperties()
    {
        while (true)
        {
            for (int i = 0; i < objectsToDebug.Length; i++)
            {
                GameObject obj = objectsToDebug[i];
                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    debugInfo[i] = GetObjectProperties(obj, rb);
                }
                else
                {
                    debugInfo[i] = $"No Rigidbody component found on {obj.name}";
                }
            }
            yield return new WaitForSeconds(logInterval);
        }
    }

    private string GetObjectProperties(GameObject obj, Rigidbody rb)
    {
        string colliderInfo = GetColliderInfo(obj);
        string externalForcesInfo = GetExternalForcesInfo(obj);

        return $"Object: {obj.name}\n" +
               $"Position: {obj.transform.position}\n" +
               $"Velocity: {rb.velocity}\n" +
               $"Use Gravity: {rb.useGravity}\n" +
               $"Is Kinematic: {rb.isKinematic}\n" +
               $"Gravity Scale: {Physics.gravity}\n" +
               $"Collider Info: {colliderInfo}\n" +
               $"Rigidbody Constraints: {rb.constraints}\n" +
               $"Interpolation: {rb.interpolation}\n" +
               $"Collision Detection: {rb.collisionDetectionMode}\n" +
               $"{externalForcesInfo}\n" +
               "---------------------------";
    }

    private string GetColliderInfo(GameObject obj)
    {
        if (obj.TryGetComponent(out Collider collider))
        {
            if (collider is BoxCollider boxCollider)
            {
                return $"Box Collider Size: {boxCollider.size}";
            }
            else if (collider is SphereCollider sphereCollider)
            {
                return $"Sphere Collider Radius: {sphereCollider.radius}";
            }
            else if (collider is CapsuleCollider capsuleCollider)
            {
                return $"Capsule Collider Radius: {capsuleCollider.radius}, Height: {capsuleCollider.height}";
            }
            else if (collider is MeshCollider meshCollider)
            {
                return $"Mesh Collider: {meshCollider.sharedMesh.name}";
            }
            else
            {
                return "Unknown Collider Type";
            }
        }
        return "No Collider Found";
    }

    private string GetExternalForcesInfo(GameObject obj)
    {
        // Implement any logic here to determine if external forces are being applied
        // For now, we will return a placeholder
        return "External Forces: None";
    }

    private void OnGUI()
    {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(Screen.height));
        for (int i = 0; i < debugInfo.Length; i++)
        {
            GUILayout.Label(debugInfo[i]);
        }
        GUILayout.EndScrollView();
    }
}
