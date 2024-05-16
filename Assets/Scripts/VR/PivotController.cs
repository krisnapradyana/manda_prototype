using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PivotController : MonoBehaviour
{
    #region Parameters
    [Header("Parameters")]
    [SerializeField] GameObject leftControllerPivot;
    [SerializeField] GameObject leftHandPivot;
    [SerializeField, Range(0, 100)] int yOffset;
    [SerializeField] float yRot;
    [SerializeField] float xPos, zPos;
    [SerializeField] float scaleWait;
    #endregion

    Coroutine thumbUpRoutine;
    Coroutine thumbDownRoutine;

    GameObject sceneRoot;

    private void Start()
    {
        sceneRoot = gameObject.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (leftControllerPivot.activeSelf)
        {
            this.transform.position = new Vector3(leftControllerPivot.transform.position.x, leftControllerPivot.transform.position.y + ((float)yOffset / 100), leftControllerPivot.transform.position.z);
        }
        else
        {
            this.transform.position = new Vector3(leftHandPivot.transform.position.x, leftHandPivot.transform.position.y + ((float)yOffset / 100), leftHandPivot.transform.position.z);
        }
    }

    /// <summary>
    /// Assigned in Inspector inside event of Thumb Up
    /// </summary>
    public void ScaleUp()
    {
        thumbUpRoutine = StartCoroutine(IEScaleWorldUp());
    }

    /// <summary>
    /// Assigned in Inspector inside event of Thumb Up
    /// </summary>
    public void ScaleDown()
    {
        thumbDownRoutine = StartCoroutine(IEScaleWorldDown());
    }

    public void StopGesture()
    {
        if (thumbUpRoutine != null)
        {
            StopCoroutine(thumbUpRoutine);
        }

        if (thumbDownRoutine != null)
        {
            StopCoroutine(thumbDownRoutine);
        }
    }

    IEnumerator IEScaleWorldUp()
    {
        while (true)
        {
            Vector3 currentScale = sceneRoot.transform.localScale;
            sceneRoot.transform.localScale = currentScale * 1.5f;

            yield return new WaitForSeconds(scaleWait);
        }
    }


    IEnumerator IEScaleWorldDown()
    {
        while (true)
        {
            Vector3 currentScale = sceneRoot.transform.localScale;
            sceneRoot.transform.localScale = currentScale * 2/3f;

            yield return new WaitForSeconds(scaleWait);
        }
    }
}
