using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gameplay;
using Singletons;
using Unity.VisualScripting;

public class TriggerListener : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] UnityAction eventAction;
    [SerializeField] string targetTag;
    Coroutine executeRoutine;
    bool hasTouched;
    CharacterBehaviour characterBehaviour;
    VRUI mainUI;
    RectTransform uiRect;

    private void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
        mainUI = GetComponent<VRUI>();
        //uiRect = mainUI.GetComponent<RectTransform>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            if (hasTouched) { return; }
            Debug.Log("Colliding with " + other.gameObject.transform.name);
            characterBehaviour.onInteractObject.Invoke(characterBehaviour);
            
            hasTouched = true;
            executeRoutine = StartCoroutine(IEWaitToExecute());
        }
    }

    private void Update()
    {
        //mainUI.GetComponent<RectTransform>().position = mainUI.GetSelectedObject().transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == targetTag)
        {
            hasTouched = false;
            Debug.Log("Exiting Collision");
        }

        if (executeRoutine != null)
        {
            StopCoroutine(executeRoutine);
        }

    }

    IEnumerator IEWaitToExecute()
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Executed Event after Delay");
    }

    IEnumerator IEShowLoadBar()
    {
        yield return null;
    }
}
