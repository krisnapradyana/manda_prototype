using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Modules;
using System;
using DG.Tweening;
using Singletons;

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
    [SerializeField] GameObject _selectCharacterPanel;
    [SerializeField] TMP_Text _characterSelectTitle;
    [SerializeField] TMP_InputField _nameInputField;

    public event Action OnStartGame;
    IntroManager IntroManager { get; set; }
    MainUI popupUI { get; set; }

    private void OnDestroy()
    {
        OnStartGame = null;
    }

    private void Start()
    {
        popupUI = FindObjectOfType<MainUI>();
    }
    
    public void InitUIIntro(IntroManager manager)
    {
        IntroManager = manager;

        _startButton.onClick.AddListener(() =>
        {
            Debug.Log("Start Explore game pressed");
            if (string.IsNullOrEmpty(IntroManager._gameDataContainer.PlayerName))
            {
                popupUI.SetupPopupUI("Notice", "Name is left blank, please fill name.", confirmButtonEnabled: true);
                StartCoroutine(popupUI.ShowPopupIE(null));
                return;
            }

            OnStartGame?.Invoke();
            _startButton.interactable = false;
            _canvasGroup.DOFade(0, 4.5f);
            _canvasGroup.blocksRaycasts = false;
        });

        _nameInputField.onValueChanged.AddListener((input) => IntroManager._gameDataContainer.SetPlayerName(input));
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
        _selectCharacterPanel.gameObject.SetActive(true);
    }
}
