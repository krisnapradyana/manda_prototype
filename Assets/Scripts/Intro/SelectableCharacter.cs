using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class SelectableCharacter : MonoBehaviour, InteractableObject
{
    [field : SerializeField] public int CharacterId { get; private set; }
    [field: SerializeField] public Animator IntroCharacterAnimator { get; private set; }
    public EventTrigger Trigger { get; private set; }
    public bool IsInspectable { get; set; }

    public event Action onSelectCharacter;
    public event Action<GameObject> onHoverObject;
    public event Action<GameObject> onExitHoverObject;
    public event Action<GameObject> onInteractObject;

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
        Trigger = GetComponent<EventTrigger>();
        Trigger.AddEvent(EventTriggerType.PointerClick, (data) =>
        {
            onSelectCharacter?.Invoke();
        });

        Trigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
        {
            onHoverObject?.Invoke(this.gameObject);
        });

        Trigger.AddEvent(EventTriggerType.PointerExit, (data) =>
        {
            onExitHoverObject?.Invoke(this.gameObject);
        });

        IntroCharacterAnimator.SetFloat("AnimId", CharacterId);
    }
}