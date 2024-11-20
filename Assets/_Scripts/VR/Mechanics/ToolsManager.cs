using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class ToolsManager : MonoBehaviour
{
    private GeneralAttributes generalAttributes;

    private GameObject selectedTool;

    [SerializeField] private Transform upperPoint, lowerPoint;
    [SerializeField] private float spawnSpeed;
    [SerializeField] private GameObject interactableWatch;
    [SerializeField] private bool shouldMove;

    //[SerializeField] private KeyCode keyCode;

    private void Awake()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();     
    }

    private void Start()
    {
        for (int i = 0; i < generalAttributes.toolsOption.Length; i++)
        {
            generalAttributes.toolsOption[i].toolToSpawn.SetActive(false);
        }

        if (upperPoint == null)
        {
            upperPoint = generalAttributes.forwardDir.transform;
            upperPoint.position = new Vector3(upperPoint.transform.position.x, upperPoint.transform.position.y + 0.6f, upperPoint.transform.position.z);
        }

        if (lowerPoint == null)
        {
            lowerPoint = generalAttributes.forwardDir.transform;
            lowerPoint.position = new Vector3(lowerPoint.transform.position.x, lowerPoint.transform.position.y - 0.4f, lowerPoint.transform.position.z);
        }
    }

    private void Update()
    {
        if (shouldMove)
        {
            CheckDistance();
        }
    }

    public void ActivateTools(int spawnID)
    {
        // Check for valid spawnID and ensure movement isn't already in progress
        if (spawnID < 0 || spawnID >= generalAttributes.toolsOption.Length || shouldMove)
        {
            Debug.LogWarning("There's no tool at that ID, or tool movement is already in progress.");
            return;
        }

        // Check if the selected tool is inactive and exists in the array
        if (generalAttributes.toolsOption[spawnID] != null && generalAttributes.toolsOption[spawnID].toolToSpawn != null && !generalAttributes.toolsOption[spawnID].toolToSpawn.activeSelf)
        {
            selectedTool = generalAttributes.toolsOption[spawnID].toolToSpawn;

            // Loop through tools and toggle the selected tool on, others off
            for (int i = 0; i < generalAttributes.toolsOption.Length; i++)
            {
                if (i == spawnID)
                {
                    shouldMove = true;
                    selectedTool.transform.position = upperPoint.position;
                    selectedTool.SetActive(true);
                }
                else if (generalAttributes.toolsOption[i].toolToSpawn != null)
                {
                    generalAttributes.toolsOption[i].toolToSpawn.SetActive(false);
                    generalAttributes.toolsOption[i].toolToSpawn.transform.localPosition = Vector3.zero;
                    generalAttributes.toolsOption[i].toolToSpawn.transform.localRotation = Quaternion.Euler(0, -90, 90);
                }
            }
        }
    }

    private void CheckDistance()
    {
        selectedTool.transform.position = Vector3.MoveTowards(selectedTool.transform.position, lowerPoint.position, spawnSpeed * Time.deltaTime);

        float distanceleft = Vector3.Distance(selectedTool.transform.position, lowerPoint.position);
        if (distanceleft <= 0.1)
        {
            shouldMove = false;
            selectedTool.transform.position = lowerPoint.position;
        }
    }
}
