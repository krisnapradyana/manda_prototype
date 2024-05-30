using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Gameplay;
using Singletons;
using TMPro;
using Pathfinding;
using UnityEngine.WSA;

public class VRUI : MonoBehaviour
{
    [Header("Parameters")]
    public GameCentralSystem _centralSystem;
    [field: SerializeField] GameObject _canvas;
    [field: SerializeField] Canvas _uiCanvas;
    [SerializeField] Button _tapToContinue;
    [SerializeField] GameObject Head;
    [SerializeField] int currentdialogPos;

    #region privates
    private string _displayedDialog;
    [SerializeField] TMP_Text _dialogText;
    [SerializeField] TMP_Text _dialogSpeakerText;
    #endregion

    private void Update()
    {
        gameObject.transform.LookAt(Head.transform);
    }

    internal void SetCurrentSelectedObject(GameObject gameObject)
    {
        throw new NotImplementedException();
    }

    internal void ShowDialogWindow(string speaker, Transform transform, Vector3 position, DialogData dialogData)
    {
        _dialogSpeakerText.text = speaker;
        StartCoroutine(StartPerTextDialogIE(dialogData, 0.05f));
    }

    IEnumerator StartPerTextDialogIE(DialogData loadedDialog, float duration = .1f)
    {
        Debug.Log($"num of chat: {loadedDialog.dialogText.Length}");
        for (int i = 0; i < loadedDialog.dialogText.Length; i++)
        {
            _centralSystem.IsCharacterSpeak = true;
            _tapToContinue.onClick.RemoveAllListeners();
            bool forcedComplete = false;
            var builder = new StringBuilder();
            var charCounter = 0;
            bool partDialogComplete = false;

            _tapToContinue.onClick.AddListener(() => { forcedComplete = true; });
            while (!forcedComplete)
            {
                for (int j = 0; j < loadedDialog.dialogText[i].Length; j++)
                {
                    builder.Append(loadedDialog.dialogText[i][j]);
                    _displayedDialog = builder.ToString();
                    _dialogText.text = _displayedDialog;
                    charCounter++;

                    if (_displayedDialog.Length >= loadedDialog.dialogText[i].Length)
                    {
                        forcedComplete = true;
                    }

                    if (forcedComplete)
                    {
                        break;
                    }
                    yield return new WaitForSeconds(duration);
                }
                yield return null;
            }

            _dialogText.text = loadedDialog.dialogText[i];
            //_dialogIndex++;
            if (i >= loadedDialog.dialogText.Length - 1)
            {
                Debug.Log("Ended Dialog");
                _tapToContinue.onClick.RemoveAllListeners();
                _tapToContinue.onClick.AddListener((UnityAction)(() => {
                    partDialogComplete = true;
                    _centralSystem.IsCharacterSpeak = false;
                    //_dialogParent.SetActive(false);
                    //_yesDialogButton.gameObject.SetActive(false);
                    //_noDialogButton.gameObject.SetActive(false);
                }));
                yield return new WaitUntil(() => partDialogComplete == true);
                yield break;
            }
            else if (i < loadedDialog.dialogText.Length)
            {
                Debug.Log("Continue dialog");
                _tapToContinue.onClick.RemoveAllListeners();
                _tapToContinue.onClick.AddListener(() =>
                {
                    partDialogComplete = true;
                });
                yield return new WaitUntil(() => partDialogComplete == true);
                continue;
            }
        }
    }
}
