using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Transform _mainPlatform;
    [SerializeField] IntroUIControl _uiControl;
    [SerializeField] SelectableCharacter[] _introCharacter;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    public GameDataContainer gameDataContainer { get; private set; }
    bool _hasSelected;

    private void OnDestroy()
    {

    }

    private void Start()
    {
        gameDataContainer = FindObjectOfType<GameDataContainer>();

        foreach (var item in _introCharacter)
        {
            item.onInteractObject += (obj) =>
            {
                Debug.Log("Selected character");
                gameDataContainer.SelectCharacter(obj.GetComponent<SelectableCharacter>().CharacterId);
                SceneManager.LoadScene(1, LoadSceneMode.Single);
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
    }

    IEnumerator DelayMoveCamera()
    {
        yield return new WaitForSeconds(2f);
        _virtualCamera.Priority = 2;
        _mainPlatform.DORotate(Vector3.zero, 5).OnComplete(() =>
        {
            _uiControl.EnableTitleCharSlc();
        });

        yield return new WaitUntil(() => _hasSelected == true);
        yield return new WaitForSeconds(3f);
    }

    void HoverInfo(GameObject targetObject, bool visibility)
    {
        _uiControl.ToggleTextVisibility(targetObject, visibility);
    }
}
