using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "ScriptableObjects/DialogData", order = 1)]
public class DialogData : ScriptableObject
{
    [TextArea] public string[] dialogText;
    public bool isQuestion;
}
