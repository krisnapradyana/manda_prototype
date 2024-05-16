using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadUI : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    [SerializeField] private float distance = 3.0f;
    [SerializeField] private bool isCentered = false;

    public void OnBecameInvisible()
    {
        isCentered = false;
        StartCoroutine(CenterObject());
    }

    private IEnumerator CenterObject()
    {
        yield return null;

        while (!isCentered)
        {
            Vector3 targetPosition = FindTargetPosition();
            MoveTowards(targetPosition);

            if (ReachedPosition(targetPosition))
            {
                isCentered = true;
            }
        }
    }

    private Vector3 FindTargetPosition()
    {
        return generalAttributes.centerEyeObject.transform.position + (generalAttributes.centerEyeObject.transform.forward * distance);
    }

    private void MoveTowards(Vector3 targetPosition)
    {
        // Instead of a tween, that would need to be constantly restarted
        transform.position += (targetPosition - transform.position) * 0.025f;
    }

    private bool ReachedPosition(Vector3 targetPosition)
    {
        return Vector3.Distance(targetPosition, transform.position) < 0.1f;
    }
}
