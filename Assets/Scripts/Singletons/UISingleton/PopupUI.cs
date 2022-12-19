using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

namespace Singletons
{
    public class PopupUI : MonoBehaviour
    {
        public static PopupUI Instance;

        [Header("Main Attributes")]
        [SerializeField] TMP_Text _titleText;
        [SerializeField] TMP_Text _descriptionText;
        [SerializeField] Button _yesButton;
        [SerializeField] Button _noButton;
        [SerializeField] Button _confirmButton;

        [Header("UI Components")]
        [SerializeField] GameObject _blockBg;
        [SerializeField] RectTransform _uiWindow;

        [Header("Popup Anim Attributes")]
        [SerializeField] float _animDuration;
        [SerializeField] Ease _animEase;

        private Tween _animTween;
        [SerializeField] Button[] _buttons;

        private void Awake()
        {
            _buttons = new Button[]{ _yesButton, _noButton, _confirmButton };

            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        public PopupUI SetupPopupUI(string titleText, string descriptionText, bool yesButtonEnabled = false, bool noButtonEnabled = false, bool confirmButtonEnabled = false)
        {
            _titleText.text = titleText;
            _descriptionText.text = descriptionText;

            _yesButton.gameObject.SetActive(yesButtonEnabled);
            _noButton.gameObject.SetActive(noButtonEnabled);
            _confirmButton.gameObject.SetActive(confirmButtonEnabled);
            return this;
        }

        public PopupUI SetupPopupUI(PopupUIData uiData, bool yesButtonEnabled = false, bool noButtonEnabled = false, bool confirmButtonEnabled = false)
        {
            _titleText.text = uiData.popupTitle;
            _descriptionText.text = uiData.popupDataDesctiptions;

            _yesButton.gameObject.SetActive(yesButtonEnabled);
            _noButton.gameObject.SetActive(noButtonEnabled);
            _confirmButton.gameObject.SetActive(confirmButtonEnabled);
            return this;
        }

        public PopupUI SetupUIEvents(Action yesAction = null, Action noAction = null, Action confirmAction = null)
        {
            _yesButton.onClick.RemoveAllListeners();
            _noButton.onClick.RemoveAllListeners();
            _confirmButton.onClick.RemoveAllListeners();

            if (yesAction != null)
            {
                _yesButton.onClick.AddListener(() => yesAction());
            }
            if (noAction != null)
            {
                _noButton.onClick.AddListener(() => noAction());
            }
            if (confirmAction != null)
            {
                _confirmButton.onClick.AddListener(() => confirmAction());
            }
            return this;
        }

        public IEnumerator ShowPopup(Action callback)
        {
            AddCloseEvent();
            if (_animTween != null)
            {
                Debug.Log("Anim is still playing");
                yield break;
            }

            ToggleBGVisibility(true);
            _animTween = _uiWindow.DOScale(Vector3.one, _animDuration).SetEase(_animEase);
            yield return _animTween.WaitForCompletion();
            _animTween = null;
            callback?.Invoke();
        }

        private void ToggleBGVisibility(bool visibility)
        {
            _blockBg.SetActive(visibility);
            if (visibility == false)
            {
                _uiWindow.transform.localScale = Vector3.zero;
            }
        }

        private void AddCloseEvent()
        {
            _yesButton.onClick      .AddListener(() => ToggleBGVisibility(false));
            _noButton.onClick       .AddListener(() => ToggleBGVisibility(false));
            _confirmButton.onClick  .AddListener(() => ToggleBGVisibility(false));
        }
    }
}
