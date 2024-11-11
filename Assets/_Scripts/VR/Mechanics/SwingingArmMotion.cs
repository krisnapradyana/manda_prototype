using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingArmMotion : MonoBehaviour
{
    private GeneralAttributes generalAttributes;

    // Game Objects
    private GameObject leftHand, rightHand;
    private GameObject mainCamera;

    // Axis Tracking Booleans
    //[SerializeField] private bool trackXAxis = true, trackYAxis = true, trackZAxis = true;

    // Vector3 Positions
    private Vector3 positionPreviousFrameLeftHand;
    private Vector3 positionPreviousFrameRightHand;
    private Vector3 playerPositionPreviousFrame;
    private Vector3 playerPositionCurrentFrame;
    private Vector3 positionCurrentFrameRightHand;
    private Vector3 positionCurrentFrameLeftHand;

    // Speed
    private float handSpeed;
    [SerializeField] private float staticSpeed = 70;
    private float initialSpeed = 0f;   // Starting speed of movement
    private float maximumSpeed = 100f;  // Max possible speed
    private float acceleration = 5f;    // Rate of speed increase per second
    private float currentSpeed;

    void Start()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();

        leftHand = generalAttributes.leftHandAnchor;
        rightHand = generalAttributes.rightHandAnchor;
        mainCamera = generalAttributes.centerEyeAnchor;

        playerPositionPreviousFrame = transform.position; // set current positions
        positionPreviousFrameLeftHand = leftHand.transform.position; // set previous positions
        positionPreviousFrameRightHand = rightHand.transform.position;
        currentSpeed = initialSpeed; // set initial speed
    }

    void Update()
    {
        // get forward direction from the main camera and set it to the forward direction object

        if (generalAttributes.canMove)
        {
            MovingForwardMechanism();
        }
        //else if (!leftRunPose && !rightRunPose && playerPositionPreviousFrame == playerPositionCurrentFrame)
        //{
        //    currentSpeed = initialSpeed;
        //}
    }

    void MovingForwardMechanism()
    {
        // get positons of hands
        positionCurrentFrameLeftHand = leftHand.transform.position;
        positionCurrentFrameRightHand = rightHand.transform.position;

        // position of player
        playerPositionCurrentFrame = transform.position;

        // get distance the hands and player has moved from last frame
        var playerDistanceMoved = Vector3.Distance(playerPositionCurrentFrame, playerPositionPreviousFrame);
        var leftHandDistanceMoved = Vector3.Distance(positionPreviousFrameLeftHand, positionCurrentFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(positionPreviousFrameRightHand, positionCurrentFrameRightHand);

        if (leftHandDistanceMoved > 0 && rightHandDistanceMoved > 0)
        {
            // aggregate to get hand speed
            handSpeed = ((leftHandDistanceMoved - playerDistanceMoved) + (rightHandDistanceMoved - playerDistanceMoved));

            if (Time.timeSinceLevelLoad > 1f)
            {
                Vector3 targetDirection = mainCamera.transform.forward;
                targetDirection.y = 0f;

                transform.position += targetDirection * handSpeed * staticSpeed * Time.deltaTime;
            }

            // set previous position of hands for next frame
            positionPreviousFrameLeftHand = positionCurrentFrameLeftHand;
            positionPreviousFrameRightHand = positionCurrentFrameRightHand;
            // set player position previous frame
            playerPositionPreviousFrame = playerPositionCurrentFrame;

            // set player position previous frame
            if (playerPositionPreviousFrame == playerPositionCurrentFrame)
            {
                currentSpeed = initialSpeed;
            }

        }
    }
}
