using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using Modules;

namespace Gameplay
{
    public class GameplayUIControl : MonoBehaviour
    {
        public event Action onReturnInspectPressed;

        [Header("UI Interaction")]
        [SerializeField] Button _returnButton;

        [Header("Basic Attributes")]
        [SerializeField] private CanvasScaler _scaler;
        [SerializeField] private TMP_Text _popupText0;
        [SerializeField] private TMP_Text _popupText1;
        [SerializeField] private RectTransform _screenArea;
        [SerializeField] private RectTransform _popupPivot0;
        [SerializeField] private RectTransform _popupPivot1;

        [field: SerializeField] public RectTransform MousePivot;

        Vector3 mousePosition;
        float halfScreenWidth;

        private void OnDestroy()
        {
            onReturnInspectPressed = null;
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

        void RegisterUIEvents()
        {
            _returnButton.onClick.AddListener(() =>
            {
                onReturnInspectPressed?.Invoke();
            });
        }

        /// <summary>
        /// To toggle false simply by not include any parameters
        /// </summary>
        /// <param name="passedObjectData"></param>
        public GameplayUIControl ToggleHoverInfo(GameObject passedObjectData = null)
        {
            bool visibility = false;
            halfScreenWidth = Screen.width / 2;
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

        public void ToggleExitButtonVisibility(bool visibility)
        {
            _returnButton.gameObject.SetActive(visibility);
        }
    }
}