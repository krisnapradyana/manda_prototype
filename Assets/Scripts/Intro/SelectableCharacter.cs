using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class SelectableCharacter : MonoBehaviour
{
    [field : SerializeField] public int CharacterId { get; private set; }
    [field: SerializeField] public Animator IntroCharacterAnimator { get; private set; }
    public EventTrigger _eventTrigger;

    public event Action onSelectCharacter;

    private void OnDestroy()
    {
        onSelectCharacter = null;
    }

    private void Awake()
    {
        InitSelectableCharacter();
    }

    public void InitSelectableCharacter()
    {
        _eventTrigger.AddEvent(EventTriggerType.PointerClick, (data) =>
        {
            onSelectCharacter?.Invoke();
        });

        IntroCharacterAnimator.SetFloat("AnimId", CharacterId);
    }
}