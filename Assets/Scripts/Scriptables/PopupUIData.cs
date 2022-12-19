using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PopupData", menuName = "ScriptableObjects/PopupDialogData", order = 1)]
public class PopupUIData : ScriptableObject
{
    public string popupTitle;
    [TextArea] public string popupDataDesctiptions;
}
