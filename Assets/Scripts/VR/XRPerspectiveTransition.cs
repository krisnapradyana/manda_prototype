using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRPerspectiveTransition : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    public XRRespawnOnFall xrRespawnOnFall;

    [SerializeField] private float floatingWaitTime = 1.2f;

    private void Awake()
    {
        generalAttributes.xrOrigin.transform.localScale = new Vector3(10, 10, 10);
    }

    public void ChangePerspective(float transitionDuration)
    {
        StartCoroutine(CheckCondition(transitionDuration));
    }

    public void NowCanTransition()
    {
        generalAttributes.canTransitionView = true;
    }
    public void NowCannotTransition()
    {
        generalAttributes.canTransitionView = false;
    }

    //use to check condition, when object released from hand (on playerchar/playerpivot
    IEnumerator CheckCondition(float duration)
    {
        yield return null;

        if (generalAttributes.canTransitionView)
        {
            // Wait for the first ToggleDarkOpacity to complete
            yield return StartCoroutine(ToggleDarkOpacity(duration));

            generalAttributes.xrOrigin.transform.localScale = new Vector3(1, 1, 1);
            generalAttributes.xrOrigin.transform.position = generalAttributes.startingPosition.position;

            // This can be used in the interface so the player position becomes 0,0,0 (but later)
            generalAttributes.xrOrigin.transform.rotation = Quaternion.Euler(0, 0, 0);

            generalAttributes.playerCharPlatform.SetActive(false);
            generalAttributes.inThirdPersonView = false;

            // Wait for the second ToggleDarkOpacity to complete
            yield return StartCoroutine(ToggleDarkOpacity(duration));
        }
        else
        {
            yield return new WaitForSeconds(floatingWaitTime);

            xrRespawnOnFall.SnapToSocket();
        }
    }

    // to toggle opacity for transition purpose
    private IEnumerator ToggleDarkOpacity(float duration)
    {
        float startOpacity, targetOpacity;
        // dark to light
        if (generalAttributes.darkOverlay.color.a == 0)
        {
            startOpacity = 0;
            targetOpacity = 1;
        }
        // light to dark
        else
        {
            startOpacity = 1;
            targetOpacity = 0;
        }

        Color newColor = generalAttributes.darkOverlay.color;
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            float newOpacity = Mathf.Lerp(startOpacity, targetOpacity, elapsedTime / duration);
            newColor.a = newOpacity;
            generalAttributes.darkOverlay.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final opacity is set correctly
        newColor.a = targetOpacity;
        generalAttributes.darkOverlay.color = newColor;
    }

    /*
     * function for Platform of char selection following hand
     */
}
