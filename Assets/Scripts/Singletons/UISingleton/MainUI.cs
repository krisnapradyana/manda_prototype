using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Text;

namespace Singletons
{
    public class MainUI : MonoBehaviour
    {
        public static MainUI Instance;

        [Header("Main Attributes")]
        private GameCentralSystem _centralSystem;
        [SerializeField] TMP_Text _titleText;
        [SerializeField] TMP_Text _descriptionText;
        [SerializeField] Button _yesButton;
        [SerializeField] Button _noButton;
        [SerializeField] Button _confirmButton;
        //[SerializeField] Button[] _buttons; //array version of 'Yes No Confirm' for popup buttons

        [Header("UI Components")]
        [SerializeField] GameObject _blockBg;
        [SerializeField] RectTransform _uiWindow;

        [Header("Popup Anim Attributes")]
        [SerializeField] float _animDuration;
        [SerializeField] Ease _animEase;

        [Header("Dialog Attributes")]
        [SerializeField] GameObject _dialogParent;
        [SerializeField] GameObject _renderPortrait;
        [SerializeField] Camera _dialogRenderCamera;
        [SerializeField] Image _dialogBox;
        [SerializeField] TMP_Text _dialogText;
        [SerializeField] TMP_Text _dialogSpeakerText;
        [SerializeField] Button _tapToContinue;
        [SerializeField] Button _yesDialogButton;
        [SerializeField] Button _noDialogButton;
        [SerializeField] float _talkDurationPerChar;
        [SerializeField] float _characterPortraitYOffset;
        public bool _isSpeaking;
        private string _displayedDialog;
        private int _dialogIndex;
        private Transform _currentDialogFocusCam;
        private DialogData _currentLoadedDialog;

        [Header("Fade Screen")]
        [SerializeField] Image _blackScreen;

        private Tween _animTween;

        private void Awake()
        {
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

        private void Start()
        {
            _centralSystem = FindObjectOfType<GameCentralSystem>();
        }

        public MainUI SetupPopupUI(string titleText, string descriptionText, bool yesButtonEnabled = false, bool noButtonEnabled = false, bool confirmButtonEnabled = false)
        {
            _titleText.text = titleText;
            _descriptionText.text = descriptionText;

            _yesButton.gameObject.SetActive(yesButtonEnabled);
            _noButton.gameObject.SetActive(noButtonEnabled);
            _confirmButton.gameObject.SetActive(confirmButtonEnabled);
            return this;
        }

        public MainUI SetupPopupUI(PopupUIData uiData, bool yesButtonEnabled = false, bool noButtonEnabled = false, bool confirmButtonEnabled = false)
        {
            _titleText.text = uiData.popupTitle;
            _descriptionText.text = uiData.popupDataDesctiptions;

            _yesButton.gameObject.SetActive(yesButtonEnabled);
            _noButton.gameObject.SetActive(noButtonEnabled);
            _confirmButton.gameObject.SetActive(confirmButtonEnabled);
            return this;
        }

        public MainUI SetupUIEvents(Action yesAction = null, Action noAction = null, Action confirmAction = null)
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

        public MainUI FadeScreen(bool isDarken, float duration,TweenCallback onStart = null , TweenCallback onComplete = null)
        {
            if (isDarken)
            {
                _blackScreen.DOFade(1, duration).OnStart(onStart).OnComplete(onComplete);
            }
            else
            {
                _blackScreen.DOFade(0, duration).OnStart(onStart).OnComplete(onComplete);
            }
            return this;
        }

        public MainUI ToggleBlockScreen(bool isBlock)
        {
            _blackScreen.raycastTarget = isBlock;
            return this;
        }

        public IEnumerator ShowPopupIE(Action callback)
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

        public void ShowDialogWindow(string speaker, Transform lookTarget, Vector3 cameraPos, DialogData dialogData, Action yesAct = null, Action noAct = null, bool allowPortait = true)
        {
            if (_isSpeaking)
            {
                return;
            }

            _renderPortrait.SetActive(allowPortait);       
            _currentLoadedDialog = dialogData;

            if (_dialogIndex >= _currentLoadedDialog.dialogText.Length)
            {
                _dialogIndex = 0;
            }

            _dialogSpeakerText.text = speaker;
            _dialogParent.SetActive(true);
            _dialogRenderCamera.transform.position = cameraPos;
            _currentDialogFocusCam = lookTarget;
            _dialogRenderCamera.transform.LookAt( new Vector3(_currentDialogFocusCam.position.x, _currentDialogFocusCam.transform.position.y + _characterPortraitYOffset, _currentDialogFocusCam.transform.position.z));

            StartCoroutine(StartPerTextDialogIE(dialogData, yesAct, noAct, allowPortait, _talkDurationPerChar));
        }

        IEnumerator StartPerTextDialogIE(DialogData loadedDialog, Action yesAct, Action noAct, bool portraitVisibility, float duration = .1f)
        {
            _isSpeaking = true;
            _tapToContinue.onClick.RemoveAllListeners();
            bool forcedComplete = false;
            var builder = new StringBuilder();
            var charCounter = 0;

            _tapToContinue.onClick.AddListener(() => { forcedComplete = true; });

            while (!forcedComplete)
            {
                for (int i = 0; i < loadedDialog.dialogText[_dialogIndex].Length; i++)
                {
                    builder.Append(loadedDialog.dialogText[_dialogIndex][i]);
                    _displayedDialog = builder.ToString();
                    _dialogText.text = _displayedDialog;
                    charCounter++;

                    if (forcedComplete)
                    {
                        break;
                    }

                    if (charCounter >= loadedDialog.dialogText[_dialogIndex].Length)
                    {
                        _isSpeaking = false;
                        _dialogIndex++;
                        if (_dialogIndex >= loadedDialog.dialogText.Length)
                        {
                            Debug.Log("Ended Dialog");
                            _tapToContinue.onClick.RemoveAllListeners();
                            if (loadedDialog.isQuestion)
                            {
                                _yesDialogButton.gameObject.SetActive(true);
                                _noDialogButton.gameObject.SetActive(true);

                                _yesDialogButton.onClick.AddListener(() => { yesAct?.Invoke(); });
                                _noDialogButton.onClick.AddListener(() => { noAct?.Invoke(); });
                                yield break;
                            }
                            _tapToContinue.onClick.AddListener(() => { _dialogParent.SetActive(false);});
                            yield break;
                        }
                        else if(_dialogIndex < loadedDialog.dialogText.Length)
                        {
                            Debug.Log("Continue dialog");
                            _tapToContinue.onClick.RemoveAllListeners();
                            _tapToContinue.onClick.AddListener(() => { ShowDialogWindow(_dialogSpeakerText.text, _currentDialogFocusCam, _dialogRenderCamera.transform.position, _currentLoadedDialog, yesAct, noAct, portraitVisibility); });
                            yield break;
                        }
                    }
                    yield return new WaitForSeconds(duration);
                }

                yield return null;
            }

            _dialogText.text = loadedDialog.dialogText[_dialogIndex];
            _isSpeaking = false;
            _dialogIndex++;
            if (_dialogIndex >= loadedDialog.dialogText.Length)
            {
                Debug.Log("Ended Dialog");
                _tapToContinue.onClick.RemoveAllListeners();
                if (loadedDialog.isQuestion)
                {
                    _yesDialogButton.gameObject.SetActive(true);
                    _noDialogButton.gameObject.SetActive(true);

                    _yesDialogButton.onClick.AddListener(() => { 
                        yesAct.Invoke(); _dialogParent.SetActive(false);
                        _yesDialogButton.gameObject.SetActive(false);
                        _noDialogButton.gameObject.SetActive(false);
                    });
                    _noDialogButton.onClick.AddListener(() => {  
                        noAct.Invoke(); _dialogParent.SetActive(false);
                        _yesDialogButton.gameObject.SetActive(false);
                        _noDialogButton.gameObject.SetActive(false);
                    });
                    yield break;
                }
                _tapToContinue.onClick.AddListener(() => { _dialogParent.SetActive(false);});
                yield break;
            }
            else if (_dialogIndex < loadedDialog.dialogText.Length)
            {
                Debug.Log("Continue dialog");
                _tapToContinue.onClick.RemoveAllListeners();
                _tapToContinue.onClick.AddListener(() => { ShowDialogWindow(_dialogSpeakerText.text, _currentDialogFocusCam, _dialogRenderCamera.transform.position, _currentLoadedDialog, yesAct, noAct, portraitVisibility); });
                yield break;
            }
        }
    }
}