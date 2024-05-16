using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public void DebugNormal(string message)
    {
        Debug.Log(message);
    }
    public void DebugWarning(string message)
    {
        Debug.LogWarning(message);
    }
    public void DebugError(string message)
    {
        Debug.LogError(message);
    }
}
