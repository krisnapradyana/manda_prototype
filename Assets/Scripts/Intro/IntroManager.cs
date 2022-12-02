using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Transform _mainPlatform;
    [SerializeField] SelectableCharacter[] _introCharacter;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    public GameDataContainer gameDataContainer;
    bool _hasSelected;

    private void OnDestroy()
    {

    }

    private void Start()
    {
        gameDataContainer = FindObjectOfType<GameDataContainer>();

        foreach (var item in _introCharacter)
        {   
            item.onSelectCharacter += () =>
            {
                gameDataContainer.SelectedCharacterIndex = item.CharacterId;
            };

            item.onHoverObject += (obj) =>
            {
               
            };

            item.onExitHoverObject += (obj) =>
            {

            };
        }

        StartCoroutine(DelayMoveCamera());
    }

    IEnumerator DelayMoveCamera()
    {
        yield return new WaitForSeconds(2f);
        _virtualCamera.Priority = 2;
        _mainPlatform.DORotate(Vector3.zero, 5);

        yield return new WaitUntil(() => _hasSelected == true);
        yield return new WaitForSeconds(3f);
        //move scene
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
