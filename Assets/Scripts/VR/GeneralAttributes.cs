using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralAttributes : MonoBehaviour
{
    [Header("Meta Object")]
    public GameObject xrOrigin;
    public GameObject centerEyeObject;
    public GameObject pivotLeftController, pivotLeftHand, pivotRightController, pivotRightHand;

    [Header("Additional GameObject")]
    //player relate
    public GameObject playerCharPlatform;
    public GameObject[] playerChar;
    public Transform[] xrPrevPos;
    public Transform startingPosition;
    public GameObject fallThreshold;
    public Transform pivotPos;
    //UI
    public Image darkOverlay, lightOverlay;

    [Header("Parameter")]
    public bool isUsingController;
    public bool isRightHanded = false;
    public bool inThirdPersonView = true;
    public bool canTransitionView = false;

    private void Update()
    {
        checkControllerActivation();
        toggleDominantHand();
    }

    private void checkControllerActivation()
    {
        //is using controller
        if (pivotLeftController.activeSelf)
        {
            pivotPos.position = pivotLeftController.transform.position;
        }

        //is using hand
        else
        {
            pivotPos.position = pivotLeftHand.transform.position;
        }
    }

    private void toggleDominantHand()
    {
        isRightHanded = !isRightHanded;
    }
}
