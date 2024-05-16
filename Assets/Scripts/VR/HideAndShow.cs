using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class GameObjectVisibility
{
    public GameObject gameObject;
    public bool isVisible;

    public UnityEvent showEvents;
    public UnityEvent hideEvents;
}

public class HideAndShow : MonoBehaviour
{
    public GameObjectVisibility[] objectsToShowHide;

    void Start()
    {
        for (int i = 0; i < objectsToShowHide.Length; i++)
        {
            if (objectsToShowHide[i].isVisible == true)
            {
                objectsToShowHide[i].gameObject.SetActive(true);
            }
            else
            {
                objectsToShowHide[i].gameObject.SetActive(false);
            }
        }
    }

    public void ToggleObjectVisibility(int index)
    {
        if (index >= 0 && index < objectsToShowHide.Length)
        {
            // from displayed to hide
            if (objectsToShowHide[index].isVisible == true)
            {
                objectsToShowHide[index].gameObject.SetActive(false);
                objectsToShowHide[index].isVisible = false;

                objectsToShowHide[index].hideEvents.Invoke();
            }

            // from hidden to display
            else if (objectsToShowHide[index].isVisible == false)
            {
                objectsToShowHide[index].gameObject.SetActive(true);
                objectsToShowHide[index].isVisible = true;

                objectsToShowHide[index].showEvents.Invoke();
            }
        }
        else
        {
            Debug.LogWarning("Invalid index!");
        }
    }
}
