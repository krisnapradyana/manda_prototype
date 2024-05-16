using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerSpawner : MonoBehaviour
{
    public GeneralAttributes generalAttributes;

    public void SpawnPanel()
    {

        if (generalAttributes.panelParent != null && generalAttributes.panelControl != null)
        {
            Vector3 targetPosition = generalAttributes.centerEyeObject.transform.position;
            float targetYRotation = generalAttributes.centerEyeObject.transform.eulerAngles.y;
            Debug.LogWarning($"name: {generalAttributes.centerEyeObject.name} - posX: {targetPosition.x}, posY: {targetPosition.y}, posZ: {targetPosition.z} - rotY: {targetYRotation}");

            generalAttributes.panelParent.transform.position = targetPosition;
            generalAttributes.panelParent.transform.eulerAngles = new Vector3(generalAttributes.panelParent.transform.eulerAngles.x, targetYRotation, generalAttributes.panelParent.transform.eulerAngles.z);
            Debug.LogWarning($"name: {generalAttributes.panelParent.name} - posX: {generalAttributes.panelParent.transform.position.x}, posY: {generalAttributes.panelParent.transform.position.y}, posZ: {generalAttributes.panelParent.transform.position.z} - rotY: {generalAttributes.panelParent.transform.eulerAngles.y}");
        }
        else
        {
            Debug.LogError("Panel GameObject Cannot Be Found");

        }
    }
}
  