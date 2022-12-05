using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntroUIControl : MonoBehaviour
{
    [Header("MainAttributes")]
    [SerializeField] public TMP_Text _popupText;

    Vector3 objectPosition;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ToggleTextVisibility(GameObject targetObject, bool visibility)
    {
        var targetPosition = targetObject.transform.position;
        var calculatedPosition = Camera.main.WorldToScreenPoint(targetPosition);

        Debug.Log(calculatedPosition);
    }
}
