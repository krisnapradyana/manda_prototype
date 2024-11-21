using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingArmMotion : MonoBehaviour
{
    private GeneralAttributes generalAttributes;
    private Rigidbody rb;

    // Game Objects
    private GameObject leftHand, rightHand;
    private GameObject mainCamera;

    // Vector3 Positions
    private Vector3 positionPreviousFrameLeftHand;
    private Vector3 positionPreviousFrameRightHand;
    private Vector3 playerPositionPreviousFrame;
    private Vector3 playerPositionCurrentFrame;
    private Vector3 positionCurrentFrameRightHand;
    private Vector3 positionCurrentFrameLeftHand;

    // Speed
    [SerializeField] private float lowHandSpeed;
    [SerializeField] private float maxHandSpeed;
    [SerializeField] private float staticSpeed = 70;
    private float handSpeed;

    // Movement Detection
    [SerializeField] private float movementThreshold = 0.01f; // Minimum distance to consider movement

    void Start()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();

        rb = GetComponent<Rigidbody>();

        leftHand = generalAttributes.leftHandAnchor;
        rightHand = generalAttributes.rightHandAnchor;
        mainCamera = generalAttributes.centerEyeAnchor;

        playerPositionPreviousFrame = transform.position; // set current positions
        positionPreviousFrameLeftHand = leftHand.transform.position; // set previous positions
        positionPreviousFrameRightHand = rightHand.transform.position;
    }

    void Update()
    {
        if (generalAttributes.canMove)
        {
            UpdateHandPositions();
            HandleMovement();
        }
        else
        {
            StopPlayerMotion();
        }
    }

    private void UpdateHandPositions()
    {
        // Update hand and player positions
        positionCurrentFrameLeftHand = leftHand.transform.position;
        positionCurrentFrameRightHand = rightHand.transform.position;
        playerPositionCurrentFrame = transform.position;
    }

    private void HandleMovement()
    {
        // Calculate distances moved
        var leftHandDistanceMoved = Vector3.Distance(positionPreviousFrameLeftHand, positionCurrentFrameLeftHand);
        var rightHandDistanceMoved = Vector3.Distance(positionPreviousFrameRightHand, positionCurrentFrameRightHand);

        // Check if hands are stationary
        if (leftHandDistanceMoved < movementThreshold && rightHandDistanceMoved < movementThreshold)
        {
            StopPlayerMotion();
            return;
        }

        // Calculate hand speed
        handSpeed = leftHandDistanceMoved + rightHandDistanceMoved;

        // Clamp hand speed
        handSpeed = Mathf.Clamp(handSpeed, lowHandSpeed, maxHandSpeed);

        ApplyMovementForce();

        // Update previous positions
        positionPreviousFrameLeftHand = positionCurrentFrameLeftHand;
        positionPreviousFrameRightHand = positionCurrentFrameRightHand;
        playerPositionPreviousFrame = playerPositionCurrentFrame;
    }

    private void ApplyMovementForce()
    {
        Vector3 targetDirection = mainCamera.transform.forward;
        targetDirection.y = 0f; // Ignore vertical movement
        targetDirection.Normalize();

        rb.AddForce(targetDirection * handSpeed * staticSpeed, ForceMode.Force);
    }

    private void StopPlayerMotion()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
