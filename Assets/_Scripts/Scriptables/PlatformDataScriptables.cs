using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlatformData", menuName = "ScriptableObjects/PlatformDataScriptables", order = 1)]
public class PlatformDataScriptables : ScriptableObject
{
    public int platformID;
    public string platformName;
    [TextArea]
    public string platformDescription;
}
