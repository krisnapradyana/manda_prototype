using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] Animator[] _introCharacterAnimator;
    [SerializeField] CinemachineVirtualCamera _virtualCamera;

    private void Start()
    {
        for (int i = 0; i < _introCharacterAnimator.Length; i++)
        {
            _introCharacterAnimator[i].SetFloat("AnimId", i);
        }

        StartCoroutine(DelayMoveCamera());
    }

    IEnumerator DelayMoveCamera()
    {
        yield return new WaitForSeconds(2f);
        _virtualCamera.Priority = 2;
        yield return new WaitForSeconds(5f);
        //move scene
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
