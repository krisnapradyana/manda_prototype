using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Modules;
using System;
using DG.Tweening;

public class IntroUIControl : MonoBehaviour
{
    [Header("MainAttributes")]
    [SerializeField] RectTransform _popupPivot;
    [SerializeField] RectTransform _arenaPanel;
    [SerializeField] TMP_Text _popupText;
    [SerializeField] CanvasScaler _canvasScaler;

    [Header("Modifier attributes")]
    [SerializeField] float textPosYOffset;

    [Header("Start Menu")]
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _startButton;
    [SerializeField] TMP_Text _characterSelectTitle;

    public event Action onStartGame;

    private void OnDestroy()
    {
        onStartGame = null;
    }

    private void Start()
    {
        _startButton.onClick.AddListener(() =>
        {
            Debug.Log("Start Explore game pressed");
            onStartGame?.Invoke();
            _startButton.interactable = false;
            _canvasGroup.DOFade(0, 4.5f);
            _canvasGroup.blocksRaycasts = false;
        });
    }

    public void ToggleTextVisibility(GameObject targetObject, bool visibility)
    {
        _popupPivot.anchoredPosition = AdditionalModule.WorldToScreenSpace(targetObject.transform.position * _canvasScaler.scaleFactor, Camera.main, _arenaPanel);
        _popupPivot.anchoredPosition = new Vector2(_popupPivot.anchoredPosition.x, _popupPivot.anchoredPosition.y + textPosYOffset);
        _popupPivot.gameObject.SetActive(visibility);
        _popupText.text = targetObject.name;
    }

    public void EnableTitleCharSlc()
    {
        _characterSelectTitle.gameObject.SetActive(true);
    }
}
