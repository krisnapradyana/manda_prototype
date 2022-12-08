using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Gameplay;

[RequireComponent(typeof(EventTrigger))]
public class SelectableCharacter : Interactables
{
    [field : SerializeField] public int CharacterId { get; private set; }
    [field: SerializeField] public Animator IntroCharacterAnimator { get; private set; }
    public EventTrigger Trigger { get; private set; }                  

    private void Awake()
    {
        InitSelectableCharacter();
    }

    public void InitSelectableCharacter()
    {
        Trigger = GetComponent<EventTrigger>();
        Trigger.AddEvent(EventTriggerType.PointerClick, (data) =>
        {
            onInteractObject?.Invoke(this);
        });

        Trigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
        {
            onHoverObject?.Invoke(this);
        });

        Trigger.AddEvent(EventTriggerType.PointerExit, (data) =>
        {
            onExitHoverObject?.Invoke(this);
        });

        IntroCharacterAnimator.SetFloat("AnimId", CharacterId);
    }
}