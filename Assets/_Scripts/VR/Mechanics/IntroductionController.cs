using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionController : MonoBehaviour
{
    private GeneralAttributes generalAttributes;

    private void Awake()
    {
        GameObject targetObject = GameObject.Find("GameManager");
        generalAttributes = targetObject.GetComponent<GeneralAttributes>();
    }

    private void FixedUpdate()
    {
        if (generalAttributes.isIntroduction)
        {
            generalAttributes.canMove = false;
        }
    }

    public void ShutIntro()
    {
        generalAttributes.isIntroduction = false;
    }
}
