using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class XRPerspectiveTransition : MonoBehaviour
{
    public GeneralAttributes generalAttributes;
    public XRRespawnOnFall xrRespawnOnFall;

    [SerializeField] private float floatingWaitTime = 1.2f;
    //public UnityEvent onTransitionEvent;
    Coroutine perspectiveCoroutine;

    private void Awake()
    {
        generalAttributes.xrOrigin.transform.localScale = new Vector3(10, 10, 10);
    }

    public void ChangePerspective(float transitionDuration)
    {
        perspectiveCoroutine = StartCoroutine(CheckCondition(transitionDuration));
    }

    public void ChangeTransitionBool(bool newValue)
    {
        generalAttributes.ToggleTransitionPermit(newValue);
    }

    //use to check condition, when object released from hand (on playerchar/playerpivot
    IEnumerator CheckCondition(float duration)
    {
        yield return new WaitForSeconds(floatingWaitTime);

        if (generalAttributes.canTransitionView)
        {
            //yield return new WaitUntil (()=> bool trasitionNow = )

            generalAttributes.inThirdPersonView = false;

            // Wait for the first ToggleDarkOpacity to complete
            yield return StartCoroutine(ToggleDarkOpacity(duration));

            generalAttributes.xrOrigin.transform.localScale = new Vector3(1, 1, 1);

            Transform playerTrans = generalAttributes.playerChar[xrRespawnOnFall.objectIndex].transform;
            generalAttributes.startingPosition.position = new Vector3(playerTrans.position.x, generalAttributes.startingPosition.position.y, playerTrans.position.z);
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
