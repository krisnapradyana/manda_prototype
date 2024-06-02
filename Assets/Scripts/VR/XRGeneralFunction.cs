using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRGeneralFunction : MonoBehaviour
{
    //ResizeRelated Parameter
    Coroutine resizeCoroutine;
    Vector3 initialSize;

    private void Start()
    {
        Vector3 initialSize = transform.localScale;
    }

    public void StartResize(float sizeTarget)
    {
        StopCoroutine(resizeCoroutine);
        resizeCoroutine = StartCoroutine(ChangeSizeToTarget(sizeTarget));
    }
    public void StopReSize()
    {
        StopCoroutine(resizeCoroutine);
    }

    private IEnumerator ChangeSizeToTarget(float sizeMultiplier)
    {
        Vector3 currentSize = transform.localScale;
        Vector3 targetSize;
        float transitionDuration = (currentSize.x / initialSize.x) / 10;
        float elapsedTime = 0f;

        if (currentSize != initialSize)
        {
            targetSize = new Vector3((initialSize.x * sizeMultiplier), (initialSize.y * sizeMultiplier), (initialSize.z * sizeMultiplier));
        }
        else
        {
            targetSize = initialSize;
        }

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;

            transform.localScale = Vector3.Lerp(currentSize, targetSize, t);
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.localScale = targetSize;
    }
}
