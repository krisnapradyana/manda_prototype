using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Singletons;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Transform _mainPlatform;
    [SerializeField] IntroUIControl _uiControl;
    [SerializeField] SelectableCharacter[] _introCharacter;
    [SerializeField] CinemachineVirtualCamera _startingCamera;
    [SerializeField] CinemachineVirtualCamera _focusCamera;
    [SerializeField] CameraCore[] _charactersCamera;
    [SerializeField] GameObject _characterSelectGround;
    [SerializeField] GameObject _editingGround;
    [SerializeField] GameObject _editingPanel;
    [SerializeField] Button _confirmEditButton;
    [SerializeField] float _orthographicSize;
    [HideInInspector] GameObject _currenlySelected;

    public GameCentralSystem _gameDataContainer { get; private set; }
    int _selectedCharID;
    bool _hasSelected;
    bool _allowSelect;

    private void OnDestroy()
    {

    }

    private void Start()
    {
        _gameDataContainer = FindObjectOfType<GameCentralSystem>();
        Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 4;
        foreach (var item in _introCharacter)
        {
            item.onInteractObject += (obj) =>
            {
                Debug.Log("Selected character");
                if (!_allowSelect)
                {
                    return;
                }
                _selectedCharID = item.CharacterId;
                _currenlySelected = item.gameObject;
                _editingGround.transform.position = _currenlySelected.transform.position;
                _charactersCamera[item.CharacterId].transform.parent = _editingGround.transform;
                _currenlySelected.transform.parent = _editingGround.transform;
                _characterSelectGround.SetActive(false);
                _editingGround.SetActive(true);
                _startingCamera.Priority = 0;
                _focusCamera.Priority = 0;
                _charactersCamera[item.CharacterId].GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = _orthographicSize;
                _editingPanel.SetActive(true);
                _allowSelect = false;
                AssignCameraPriority(item.CharacterId, _charactersCamera);
            };

            item.onHoverObject += (obj) =>
            {
                if (!_allowSelect)
                {
                    return;
                }
                HoverInfo(obj.gameObject, true);
            };

            item.onExitHoverObject += (obj) =>
            {
                HoverInfo(obj.gameObject, false);
            };
        }

        _uiControl.InitUIIntro(this);
        _uiControl.OnStartGame += () => StartCoroutine(DelayMoveCamera());
        _confirmEditButton.onClick.AddListener(() =>
        {
            _gameDataContainer.SelectCharacter(_selectedCharID);
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        });

        _gameDataContainer.SetGameState(GameState.intro);
    }

    IEnumerator DelayMoveCamera()
    {
        _focusCamera.Priority = 2;
        _mainPlatform.DORotate(Vector3.zero, 5f).OnComplete(() =>
        {
            _uiControl.EnableTitleCharSlc();
            _allowSelect = true;
            Camera.main.GetComponent<CinemachineBrain>().m_DefaultBlend.m_Time = 1;
        });

        yield return new WaitUntil(() => _hasSelected == true);
        yield return new WaitForSeconds(1.5f);
    }

    void HoverInfo(GameObject targetObject, bool visibility)
    {
        _uiControl.ToggleTextVisibility(targetObject, visibility);
    }

    public void AssignCameraPriority(int comparedId, CameraCore[] collectionList)//, bool saveLastId = true)
    {
        foreach (var item in collectionList)
        {
            if (item.CameraId == comparedId)
            {
                item.SetCameraPriority(1);
            }
        }
    }
}
