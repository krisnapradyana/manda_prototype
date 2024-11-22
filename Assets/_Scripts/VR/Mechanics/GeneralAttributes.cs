using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] // This attribute makes ToolOption visible in the Unity Inspector
public class ToolOption
{
    public GameObject toolToSpawn;
    public string targetTag;
    public int toolStrength;
    public float waitTime;
}

public class GeneralAttributes : MonoBehaviour
{
    public static GeneralAttributes Instance;

    [Header("Meta Objects")]
    public GameObject playerRoot;
    public GameObject xrOrigin;
    public GameObject centerEyeAnchor, leftEyeAnchor, rightEyeAnchor;
    public GameObject leftHandAnchor, rightHandAnchor;

    [Header("Additional GameObjects")]
    //player related additional object
    public GameObject forwardDir;
    public GameObject[] playerChar;
    public GameObject fallThreshold;
    public GameObject leftWatch, rightWatch;

    [Header("Parameters")]
    public bool isRightHanded = true;
    public bool disableHandCanvas = true;
    public bool canMove = false;
    public bool leftHoldingTools, rightHoldingTools;
    [HideInInspector] public static float sceneStartTime;
    public bool isIntroduction;

    [Header("Canvas")]
    //Save Canvas/Interactable Menu Here
    public GameObject HandPsuedoCanvas;
    public GameObject BasicMenu;

    [Header("Tools n Drop")]
    public ToolOption[] toolsOption;
    //public GameObject[] toolsToSpawn;

    [Header("Pose")]
    //Save Pose Here
    public GameObject openMenu_Right;
    public GameObject openMenu_Left;
    public GameObject closeMenu_Right, closeMenu_Left;
    public GameObject runGesture_Right, runGesture_Left;
    private bool leftRunPose, rightRunPose;

    [Header("Prefabs Related")]
    //Add Prefabs Here
    public GameObject LogPrefab_Parent;
    public GameObject RockPrefab_Parent;
    public Transform minThreshold, maxThreshold;

    void Awake()
    {
        Debug.Log("Initializing general attribute");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
            return;
        }

        sceneStartTime = Time.time;
        Debug.Log($"started at: {sceneStartTime}");
    }

    public static float CurrentTime
    {
        get { return Time.time - sceneStartTime; }
    }

    private void MoveConditionChecker()
    {
        if (!leftHoldingTools && !rightHoldingTools && (leftRunPose || rightRunPose))
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
    }

    public void R_GrabChecker(bool value)
    {
        rightHoldingTools = value;
        MoveConditionChecker();
    }

    public void L_GrabChecker(bool value)
    {
        leftHoldingTools = value;
        MoveConditionChecker();
    }

    public void R_RunPoseChecker(bool value)
    {
        rightRunPose = value;
        MoveConditionChecker();
    }

    public void L_RunPoseChecker(bool value)
    {
        leftRunPose = value;
        MoveConditionChecker();
    }
}
