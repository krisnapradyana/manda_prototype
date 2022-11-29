using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIControl : MonoBehaviour
{
    [SerializeField] public TMP_Text _popupText0;
    [SerializeField] public TMP_Text _popupText1;
    [SerializeField] private RectTransform _popupPivot0;
    [SerializeField] private RectTransform _popupPivot1;

    Vector3 mousePosition;

    private void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10f;
        _popupPivot0.anchoredPosition = _popupPivot1.anchoredPosition = mousePosition;
    }

    public void ToggleHoverInfo(bool visibility, GameObject passedObject = null)
    {
        GameObject hoveredObject= null;
        if (passedObject != null)
        {
            hoveredObject = passedObject;
            _popupText0.text = _popupText1.text = hoveredObject.name;
        }
        else Debug.LogWarning("No Passed object");

        _popupPivot0.gameObject.SetActive(false);
        _popupPivot1.gameObject.SetActive(false);
        if (mousePosition.x > Screen.width/2)
        {
            _popupPivot1.gameObject.SetActive(visibility);
        }
        else
        {
            _popupPivot0.gameObject.SetActive(visibility);
        }
    }
}
