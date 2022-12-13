using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using Modules;
using DG.Tweening;

namespace Gameplay
{
    public class GameplayUIControl : MonoBehaviour
    {
        public event Action onReturnInspectPressed;
        [HideInInspector] GameHandler _gameHandler;

        [Header("UI Interaction")]
        [SerializeField] Button _returnButton;

        [Header("Basic Attributes")]
        [SerializeField] private CanvasScaler _scaler;
        [SerializeField] private TMP_Text _popupText0;
        [SerializeField] private TMP_Text _popupText1;
        [SerializeField] private TMP_Text _playerName;
        [SerializeField] private RectTransform _screenArea;
        [SerializeField] private RectTransform _popupPivot0;
        [SerializeField] private RectTransform _popupPivot1;

        [Header("Inspect Detail UI")]
        [SerializeField] private Transform _inspectWindow;
        [SerializeField] private TMP_Text _objectName;
        [SerializeField] private TMP_Text _objectDesction;
        [SerializeField] private Button _levelUpButton;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private TMP_Text _levelBtnText;

        [Header("Positional")]
        [SerializeField] Transform _inspectUIShowPosition;
        [SerializeField] Transform _inspectUIHidePosition;

        [field: SerializeField] public RectTransform MousePivot;

        Vector3 mousePosition;
        float halfScreenWidth;

        private void OnDestroy()
        {
            onReturnInspectPressed = null;
        }

        private void Awake()
        {
            _gameHandler = FindObjectOfType<GameHandler>();
        }

        private void Start()
        {
            RegisterUIEvents();
        }

        private void Update()
        {
            var screenMousePos = AdditionalModule.GetWorldPoint();
            MousePivot.anchoredPosition = _popupPivot0.anchoredPosition = _popupPivot1.anchoredPosition = AdditionalModule.WorldToScreenSpace(screenMousePos * _scaler.scaleFactor, Camera.main, _screenArea);
        }

        public void SetPlayerUI()
        {
            _playerName.text = _gameHandler._dataContainer.PlayerName;
        }

        void RegisterUIEvents()
        {
            _returnButton.onClick.AddListener(() =>
            {
                onReturnInspectPressed?.Invoke();
            });

            _levelUpButton.onClick.AddListener(() =>
            {
                _gameHandler.InspectedObject.IncreaseLevel(1, () => _levelUpButton.interactable = false);
            });
        }

        /// <summary>
        /// To toggle false simply by not include any parameters
        /// </summary>
        /// <param name="passedObjectData"></param>
        public GameplayUIControl ToggleHoverInfo(GameObject passedObjectData = null)
        {
            bool visibility = false;
            halfScreenWidth = Screen.width / 2 * _scaler.scaleFactor;
            if (passedObjectData == null)
            {
                visibility = false;
            }
            else
            {
                visibility = true;
            }

            if (passedObjectData != null)
            {
                _popupText0.text = _popupText1.text = passedObjectData.name;
            }
            else Debug.LogWarning("No Passed object");

            _popupPivot0.gameObject.SetActive(false);
            _popupPivot1.gameObject.SetActive(false);
            MousePivot.gameObject.SetActive(false);
            if (mousePosition.x > halfScreenWidth)
            {
                _popupPivot1.gameObject.SetActive(visibility);
            }
            else
            {
                _popupPivot0.gameObject.SetActive(visibility);
            }

            return this;
        }

        public GameplayUIControl SetUpdateObjectDescription(string title, string desc, string objectLevel)
        {
            _inspectWindow.gameObject.SetActive(true);
            _objectName.text = title;
            _objectDesction.text = desc;
            _level.text = objectLevel;

            return this;
        }

        public void SetLevelUpButton(int currentLevel, int maxLevel)
        {
            if (currentLevel > maxLevel)
            {
                _levelUpButton.interactable = false;
                _levelBtnText.text = "Level Max";
            }
            else
            {
                _levelUpButton.interactable = true;
                _levelBtnText.text = "Level Up!";
            }
        }

        public void ToggleInspectVisibility(bool visibility)
        {
            _returnButton.gameObject.SetActive(visibility);
            _inspectWindow.gameObject.SetActive(visibility);
        }

        public IEnumerator ShowOrHidePosition(bool isShow, Action onComplete)
        {
            Tween uiAnimation;
            if (isShow)
            {
                uiAnimation = _inspectWindow.DOLocalMoveX(_inspectUIShowPosition.localPosition.x, 1f).SetEase(Ease.OutBack);
                yield return uiAnimation.WaitForCompletion();
                _returnButton.gameObject.SetActive(isShow);
                onComplete?.Invoke();
            }
            else
            {
                uiAnimation = _inspectWindow.DOLocalMoveX(_inspectUIHidePosition.localPosition.x, 1f).SetEase(Ease.OutBack);
                _returnButton.gameObject.SetActive(isShow);
                yield return uiAnimation.WaitForCompletion();
                onComplete?.Invoke();
            }
        }
    }
}