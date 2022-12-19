using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace Gameplay
{
    [RequireComponent(typeof(EventTrigger))]
    public class ObjectBehaviour : Interactables
    {
        [field : Header("Properties")]
        [SerializeField] GameObject[] platformObjects;
        [field: SerializeField] public PlatformDataScriptables PlatformData { get; private set; }
        [field: SerializeField] public EventTrigger Trigger { get ; set; }

        // Start is called before the first frame update
        void Start()
        {
            _gameHandler = FindObjectOfType<GameHandler>();
            Trigger = gameObject.GetComponent<EventTrigger>();

            //StartCoroutine(IncreaseGold());

            SetMaxLevel(platformObjects.Length - 1);

            Trigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
            {
                //Debug.Log("Hovered on building : " + gameObject.name);
                onHoverObject?.Invoke(this);
            });

            Trigger.AddEvent(EventTriggerType.PointerExit, (data) =>
            {
                //Debug.Log("Exited on building : " + gameObject.name);
                onExitHoverObject?.Invoke(this);
            });

            Trigger.AddEvent(EventTriggerType.PointerClick, (data) =>
            {
                if (_gameHandler.IsInspecting)
                {
                    return;
                }

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    onInteractObject?.Invoke(this);
                }
            });
        }

        public void ShowObjectProperties()
        {
            for (int i = 0; i < Level; i++)
            {
                platformObjects[Level - 1].SetActive(true);
                platformObjects[Level - 1].GetComponent<Animator>().SetTrigger("popItem");
            }
        }

        IEnumerator IncreaseGold()
        {
            while (true)
            {
                var tempGold = _gameHandler.PlayerGold + Level * CurrentCost/20;
                Debug.Log(tempGold);
                _gameHandler.PlayerGold += tempGold;
                Debug.Log("Increasing gold");
                yield return new WaitForSeconds(3f);
            }
        }
    }
}
