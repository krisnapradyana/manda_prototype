using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gameplay;
using Singletons;

public class TriggerListener : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] UnityAction eventAction;
    [SerializeField] string targetTag;
    Coroutine executeRoutine;
    bool hasTouched;
    CharacterBehaviour characterBehaviour;
    MainUI mainUI;

    private void Start()
    {
        characterBehaviour = GetComponent<CharacterBehaviour>();
        mainUI = GetComponent<MainUI>();
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
            Debug.Log(mainUI.GetSelectedObject().name + " IS SELECTED");
            Debug.Log(mainUI.GetSelectedObject().name + " POSITION CHANGED");
        }
    }

    private void Update()
    {
        mainUI.transform.position = mainUI.GetSelectedObject().transform.position;
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
