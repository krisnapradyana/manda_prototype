using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectCharacter : MonoBehaviour
{
    //[SerializeField] private UnityEvent onSelectEvent;
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private GameObject[] selectableCharacters;
    public int selectedChar = -1;

    private void Start()
    {
        Debug.LogWarning($"sc: {selectedChar}, opt: {selectableCharacters.Length}");
    }

    public void OnSelectCharacter(int targetID)
    {
        for (int i = 0; i < selectableCharacters.Length; i++)
        {
            Debug.Log(i);
            selectableCharacters[i].SetActive(false);
        }

        selectedChar = targetID;
        confirmationPanel.SetActive(true);
    }

    public void OnConfirmSelection()
    {
        if (selectedChar != null && selectedChar >= 0 && selectedChar <= selectableCharacters.Length)
        {
            Debug.Log($"selected = {selectedChar}");
        }
    }

    public void OnBackSelection()
    {
        confirmationPanel.SetActive(false);

        for (int i = 0; i < selectableCharacters.Length; i++)
        {
            selectableCharacters[i].SetActive(true);
        }
    }
}
