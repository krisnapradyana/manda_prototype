using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabler : MonoBehaviour
{
    [Header("Disabler")]
    [SerializeField] GameObject[] objectToDisableForEditor;
    [SerializeField] GameObject[] objectToDisableForBuildOnly;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform != RuntimePlatform.LinuxEditor)
        {
            DisableOnEditor();
        }

        else
        {
            DisableOnEditor();
            DisableOndeloy();
        }
    }

    void DisableOnEditor()
    {
        foreach (GameObject target in objectToDisableForEditor)
        {
            target.SetActive(false);
        }
    }

    void DisableOndeloy()
    {
        foreach (GameObject target in objectToDisableForBuildOnly)
        {
            target.SetActive(false);
        }
    }
}
