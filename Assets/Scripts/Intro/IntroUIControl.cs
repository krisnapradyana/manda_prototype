using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Modules;

public class IntroUIControl : MonoBehaviour
{
    [Header("MainAttributes")]
    [SerializeField] RectTransform _popupPivot;
    [SerializeField] RectTransform _arenaPanel;
    [SerializeField] TMP_Text _popupText;
    [SerializeField] CanvasScaler _canvasScaler;

    [Header("Modifier attributes")]
    [SerializeField] float textPosYOffset;
    Vector3 objectPosition;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void ToggleTextVisibility(GameObject targetObject, bool visibility)
    {
        ///var targetObjectPos = Camera.main.WorldToViewportPoint(targetObject.transform.position);
        ///targetObjectPos.z = 10f;
        ///targetObjectPos.y = targetObjectPos.y + textPosYOffset;
        ///_popupPivot.anchoredPosition = targetObjectPos;

        _popupPivot.anchoredPosition = AdditionalModule.WorldToScreenSpace(targetObject.transform.position * _canvasScaler.scaleFactor, Camera.main, _arenaPanel);
        _popupPivot.anchoredPosition = new Vector2(_popupPivot.anchoredPosition.x, _popupPivot.anchoredPosition.y + textPosYOffset);
        _popupPivot.gameObject.SetActive(visibility);
        _popupText.text = targetObject.name;
    }
}
