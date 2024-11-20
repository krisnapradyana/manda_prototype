using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandCanvasManager : MonoBehaviour
{
    public Vector3 additionNum;

    private GeneralAttributes generalAttributes;
    [SerializeField] private float canvasHeight;
    private Transform handToFollow;

    [SerializeField] private GameObject interactablePanel;

    private void Start()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();

        interactablePanel.SetActive(false);
        ToggleHandCanvas(generalAttributes.isRightHanded);

        if (generalAttributes.disableHandCanvas)
        {
            DisableHandCanvas();
        }
    }

    private void Update()
    {
        FollowHandPosition();
        LookAtPsuedo();
    }

    public void LookAtPsuedo()
    {
        Vector3 target = new Vector3(generalAttributes.centerEyeAnchor.transform.position.x + additionNum.x, transform.position.y + additionNum.y, generalAttributes.centerEyeAnchor.transform.position.z + additionNum.z);
        transform.LookAt(target);
    }

    public void FollowHandPosition()
    { 
        transform.position = new Vector3(handToFollow.position.x, handToFollow.position.y + canvasHeight, handToFollow.position.z);
    }

    public void ToggleCanvas()
    {
        generalAttributes.BasicMenu.SetActive(!generalAttributes.BasicMenu.activeSelf);
    }

    public void DisableHandCanvas()
    {
        generalAttributes.openMenu_Right.SetActive(false);
        generalAttributes.closeMenu_Right.SetActive(false);
        generalAttributes.openMenu_Left.SetActive(false);
        generalAttributes.closeMenu_Left.SetActive(false);
    }

    public void ToggleHandCanvas(bool value)
    {
        generalAttributes.isRightHanded = value;

        if (generalAttributes.isRightHanded)
        {
            handToFollow = generalAttributes.leftHandAnchor.transform;


            if (!generalAttributes.disableHandCanvas)
            {
                generalAttributes.openMenu_Left.SetActive(true);
                generalAttributes.closeMenu_Left.SetActive(true);
            }

            generalAttributes.openMenu_Right.SetActive(false);
            generalAttributes.closeMenu_Right.SetActive(false);
            generalAttributes.rightWatch.SetActive(false);
            generalAttributes.leftWatch.SetActive(true);
        }
        else
        {
            handToFollow = generalAttributes.rightHandAnchor.transform;

            if (!generalAttributes.disableHandCanvas)
            {
                generalAttributes.openMenu_Right.SetActive(true);
                generalAttributes.closeMenu_Right.SetActive(true);
            }

            generalAttributes.rightWatch.SetActive(true);
            generalAttributes.openMenu_Left.SetActive(false);
            generalAttributes.closeMenu_Left.SetActive(false);
            generalAttributes.leftWatch.SetActive(false);
        }
    }
}
