using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Gameplay
{
    [RequireComponent(typeof(EventTrigger))]
    public class ObjectBehaviour : Interactables
    {
        [field : Header("Properties")]
        [Header("Platform Items")]
        [SerializeField] GameObject[] platformObjects;
        [field: SerializeField] public EventTrigger Trigger { get ; set; }

        // Start is called before the first frame update
        void Start()
        {
            _gameHandler = FindObjectOfType<GameHandler>();
            Trigger = gameObject.Acquire<EventTrigger>();

            SetMaxLevel(platformObjects.Length - 1);

            Trigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
            {
                //Debug.Log("Hovered on building : " + gameObject.name);
                onHoverObject?.Invoke(this.gameObject);
            });

            Trigger.AddEvent(EventTriggerType.PointerExit, (data) =>
            {
                //Debug.Log("Exited on building : " + gameObject.name);
                onExitHoverObject?.Invoke(this.gameObject);
            });

            Trigger.AddEvent(EventTriggerType.PointerClick, (data) =>
            {
                if (_gameHandler.IsInspecting)
                {
                    return;
                }

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    onInteractObject?.Invoke(this.gameObject);
                }
            });
        }

        public void ShowObjectProperties()
        {
            for (int i = 0; i < Level; i++)
            {
                platformObjects[Level - 1].SetActive(true);
            }
        }
    }
}
