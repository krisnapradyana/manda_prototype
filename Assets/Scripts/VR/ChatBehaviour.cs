using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject centerEyeObject;
    [SerializeField] private Collider triggerableObject;

    private bool showCanvas;
    [SerializeField] private GameObject chatCanvas;
    [SerializeField] private TextMeshProUGUI tmpContent;
    [SerializeField] private float normalTypingSpeed = 0.025f;
    [SerializeField] private float quickTypingSpeed = 0.625f;
    [SerializeField] private bool quickTyping;
    public string[] textForNPC;

    private void Start()
    {
        chatCanvas.SetActive(false);
    }

    private void Update()
    {
        //this.transform.LookAt(centerEyeObject.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == triggerableObject)
        {
            Debug.Log("Detected");
            ToggleObjectActivation();
        }
    }

    private void ToggleObjectActivation()
    {
        if (showCanvas)
        {
            chatCanvas.SetActive(false);
            showCanvas = false;
        }
        else
        {
            chatCanvas.SetActive(true);
            showCanvas = true;
        }
    }

    public void ChatWithNPC()
    {
        if(showCanvas && chatCanvas != null)
        {
            for (int i = 0; i < textForNPC.Length; i++)
            {
                Debug.Log($"Chat should Display ID: {i}");
                //if (quickTyping)
                //{
                //    StartCoroutine(TypeByWord(textForNPC[i]));
                //}
                //else
                //{
                //    StartCoroutine(TypeByChar(textForNPC[i]));
                //}
            }
        }
    }

    IEnumerator TypeByChar(string message)
    {
        yield return null;

        tmpContent.text = "";
        foreach (char letter in message.ToCharArray())
        {
            tmpContent.text += letter;
            yield return new WaitForSeconds(normalTypingSpeed);
        }
    }
    IEnumerator TypeByWord(string message)
    {
        yield return null;

        tmpContent.text = "";
        string[] words = message.Split(' ');

        foreach (string word in words)
        {
            if (tmpContent.text.Length > 0)
            {
                tmpContent.text += " "; // Add a space between words
            }
            tmpContent.text += word;
            yield return new WaitForSeconds(quickTypingSpeed);
        }
    }
}
