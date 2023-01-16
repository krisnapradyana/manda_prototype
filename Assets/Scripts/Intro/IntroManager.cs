using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using Singletons;

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

    [HideInInspector] GameObject _currenlySelected;

    public GameCentralSystem _gameDataContainer { get; private set; }
    bool _hasSelected;

    private void OnDestroy()
    {

    }

    private void Start()
    {
        _gameDataContainer = FindObjectOfType<GameCentralSystem>();

        foreach (var item in _introCharacter)
        {
            item.onInteractObject += (obj) =>
            {
                Debug.Log("Selected character");

                //_gameDataContainer.SelectCharacter(obj.GetComponent<SelectableCharacter>().CharacterId);
                //SceneManager.LoadScene(1, LoadSceneMode.Single);
                _currenlySelected = item.gameObject;
                _editingGround.transform.position = _currenlySelected.transform.position;
                _charactersCamera[item.CharacterId].transform.parent = _editingGround.transform;
                _currenlySelected.transform.parent = _editingGround.transform;
                _characterSelectGround.SetActive(false);
                _editingGround.SetActive(true);
                _startingCamera.Priority = 0;
                _focusCamera.Priority = 0;
                _charactersCamera[item.CharacterId].GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = 1.8f; 
                AssignCameraPriority(item.CharacterId, _charactersCamera);
            };

            item.onHoverObject += (obj) =>
            {
                HoverInfo(obj.gameObject, true);
            };

            item.onExitHoverObject += (obj) =>
            {
                HoverInfo(obj.gameObject, false);
            };
        }

        _uiControl.InitUIIntro(this);
        _uiControl.OnStartGame += () => StartCoroutine(DelayMoveCamera());

        _gameDataContainer.SetGameState(GameState.intro);
    }

    IEnumerator DelayMoveCamera()
    {
        _focusCamera.Priority = 2;
        _mainPlatform.DORotate(Vector3.zero, 5f).OnComplete(() =>
        {
            _uiControl.EnableTitleCharSlc();
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
