using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class UIControl : MonoBehaviour
{
    public event Action onReturnInspectPressed;

    [Header("UI Interaction")]
    [SerializeField] Button _returnButton;

    [Header("Basic Attributes")]
    [SerializeField] public TMP_Text _popupText0;
    [SerializeField] public TMP_Text _popupText1;
    [SerializeField] private RectTransform _popupPivot0;
    [SerializeField] private RectTransform _popupPivot1;

    Vector3 mousePosition;
    float halfScreenWidth;

    private void OnDestroy()
    {
        onReturnInspectPressed = null;
    }

    private void Start()
    {
        halfScreenWidth = Screen.width / 2;
        RegisterUIEvents();
    }

    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10f;
        _popupPivot0.anchoredPosition = _popupPivot1.anchoredPosition = mousePosition;
    }

    void RegisterUIEvents()
    {
        _returnButton.onClick.AddListener(() =>
        {
            onReturnInspectPressed?.Invoke();
        });
    }

    public void ToggleHoverInfo(bool visibility, GameObject passedObjectData = null)
    {
        GameObject hoveredObject= null;
        if (passedObjectData != null)
        {
            hoveredObject = passedObjectData;
            _popupText0.text = _popupText1.text = hoveredObject.name;
        }
        else Debug.LogWarning("No Passed object");

        _popupPivot0.gameObject.SetActive(false);
        _popupPivot1.gameObject.SetActive(false);
        if (mousePosition.x > halfScreenWidth)
        {
            _popupPivot1.gameObject.SetActive(visibility);
        }
        else
        {
            _popupPivot0.gameObject.SetActive(visibility);
        }
    }

    public void ToggleExitButtonVisibility(bool visibility)
    {
        _returnButton.gameObject.SetActive(visibility);
    }
}
