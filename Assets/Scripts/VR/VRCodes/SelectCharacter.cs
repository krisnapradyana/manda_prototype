using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectCharacter : MonoBehaviour
{
    //[SerializeField] private UnityEvent onSelectEvent;
    [SerializeField] private GameObject[] selectableCharacter;
    [SerializeField] private GameObject[] pokeablePanel;
    [SerializeField] private GameObject[] confirmationPanel;

    //public void InvokeEvent()
    //{
    //    onSelectEvent.Invoke();
    //}

    public void OnSelectCharacter(int targetID)
    {
        for (int i = 0; i < selectableCharacter.Length; i++)
        {
            if (i == targetID)
            {
                selectableCharacter[i].SetActive(true);
                pokeablePanel[i].SetActive(false);
                confirmationPanel[i].SetActive(true);
            }

            else
            {
                selectableCharacter[i].SetActive(false);
                pokeablePanel[i].SetActive(true);
                confirmationPanel[i].SetActive(false);
            }
        }
    }
}
