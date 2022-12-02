using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    [RequireComponent(typeof(EventTrigger))]
    public class ObjectBehaviour : MonoBehaviour, InteractableObject
    {
        [Header("Properties")]
        [SerializeField] GameHandler _gameHandler;

        [field: SerializeField] public EventTrigger Trigger { get ; set; }
        public bool IsInspectable { get; set; }

        public event Action<GameObject> onHoverObject;
        public event Action<GameObject> onExitHoverObject;
        public event Action<GameObject> onInteractObject;


        private void OnDestroy()
        {
            onHoverObject = null;
            onExitHoverObject = null;
        }

        // Start is called before the first frame update
        void Start()
        {
            _gameHandler = FindObjectOfType<GameHandler>();
            Trigger = gameObject.Acquire<EventTrigger>();

            Trigger.AddEvent(EventTriggerType.PointerEnter, (data) =>
            {
                Debug.Log("Hovered on building : " + gameObject.name);
                onHoverObject?.Invoke(this.gameObject);
            });

            Trigger.AddEvent(EventTriggerType.PointerExit, (data) =>
            {
                Debug.Log("Exited on building : " + gameObject.name);
                onExitHoverObject?.Invoke(this.gameObject);
            });

            Trigger.AddEvent(EventTriggerType.PointerClick, (data) =>
            {
                if (_gameHandler.IsInspecting)
                {
                    return;
                }
                onInteractObject?.Invoke(this.gameObject);
            });
        }
    }
}
