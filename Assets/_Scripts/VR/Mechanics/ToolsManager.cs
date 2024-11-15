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

    private void WatchController(bool moveWatch, bool rotateWatch, bool scaleWatch)
    {
        if (moveWatch)
        {
            //interactableWatch.transform.position = watchPlacement.transform.position;
        }

        if (rotateWatch)
        {
            //interactableWatch.transform.rotation = watchPlacement.transform.rotation;
        }

        if (scaleWatch)
        {
            //interactableWatch.transform.localScale = watchPlacement.transform.localScale;
        }
    }

    public void ActivateTools(int index)
    {
        Debug.LogWarning($"Trying to activate tool at index {index}");

        if (index >= 0 && index < generalAttributes.toolsOption.Length)
        {
            Debug.LogWarning($"Tool Option Found: {generalAttributes.toolsOption[index]}");
            if (generalAttributes.toolsOption[index].toolToSpawn != null)
            {
                Debug.LogWarning($"Spawning tool: {generalAttributes.toolsOption[index].toolToSpawn.name}");
            }
            else
            {
                Debug.LogError($"Tool to spawn at index {index} is null.");
            }
        }
        else
        {
            Debug.LogError($"Index {index} is out of bounds. Available range: 0 - {generalAttributes.toolsOption.Length - 1}");
        }
    }



    private IEnumerator MoveToTargetCoroutine(GameObject tool, Vector3 target, float speed)
    {
        //Rigidbody rb = tool.GetComponent<Rigidbody>();
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            tool.transform.position = Vector3.MoveTowards(tool.transform.position, target, speed * Time.deltaTime);
        }

        transform.position = target;
        yield return null;
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
