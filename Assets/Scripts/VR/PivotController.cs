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
    [SerializeField, Range(0, 100)] int controllerOffset;
    [SerializeField, Range(0, 100)] int handOffset;

    [SerializeField] float topScaleTreshold, lowerScaleTreshold;
    [SerializeField] float scaleChange;
    [SerializeField] float scaleDelay;

    [SerializeField] float yRot;
    [SerializeField] float xPos, zPos;
    
    [SerializeField] Vector3 currentHand;
    #endregion

    Coroutine thumbUpRoutine;
    Coroutine thumbDownRoutine;

    GameObject sceneRoot;
    GameObject leftControllerPivotChecker;

    private void Start()
    {
        sceneRoot = gameObject.transform.GetChild(0).gameObject;
        leftControllerPivotChecker = leftControllerPivot.transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        if (leftControllerPivotChecker.activeSelf)
        {
            Debug.Log("currently using controller");
            this.transform.position = new Vector3(leftControllerPivot.transform.position.x, leftControllerPivot.transform.position.y + ((float)controllerOffset / 100), leftControllerPivot.transform.position.z);
        }
        else
        {
            Debug.Log("currently using hand");
            this.transform.position = new Vector3(leftHandPivot.transform.position.x, leftHandPivot.transform.position.y + ((float)handOffset / 100), leftHandPivot.transform.position.z);
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
        //check first if its already reaching limit or not
        while (sceneRoot.transform.localScale.x < topScaleTreshold)
        {
            Vector3 currentScale = sceneRoot.transform.localScale;
            float sampleValue = currentScale.x;

            // if decreement higher than TopScaleTreshold
            if (sampleValue + scaleChange > topScaleTreshold)
            {
                currentScale = new Vector3(topScaleTreshold, topScaleTreshold, topScaleTreshold);
                sceneRoot.transform.localScale = currentScale;

                Debug.Log($"scene up: {currentScale}");
            }
            else
            {
                sampleValue += scaleChange;
                currentScale = new Vector3(sampleValue, sampleValue, sampleValue);
                sceneRoot.transform.localScale = currentScale;

                Debug.Log($"scene up: {currentScale}");
            }

            yield return new WaitForSeconds(scaleDelay);
        }
    }

    IEnumerator IEScaleWorldDown()
    {
        while (sceneRoot.transform.localScale.x > lowerScaleTreshold)
        {
            Vector3 currentScale = sceneRoot.transform.localScale;
            float sampleValue = currentScale.x;

            // if decreement lower than lowerScaleTreshold
            if (sampleValue - scaleChange < lowerScaleTreshold)
            {
                currentScale = new Vector3(lowerScaleTreshold, lowerScaleTreshold, lowerScaleTreshold);
                sceneRoot.transform.localScale = currentScale;

                Debug.Log($"scene down: {currentScale}");
            }
            else
            {
                sampleValue -= scaleChange;
                currentScale = new Vector3(sampleValue, sampleValue, sampleValue);
                sceneRoot.transform.localScale = currentScale;

                Debug.Log($"scene down: {currentScale}");
            }

            yield return new WaitForSeconds(scaleDelay);
        }
    }
}
