using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class Interactables : MonoBehaviour, IInteractableObject, ILevel
    {
        [Header("Parent Referece")]
        [HideInInspector] public GameHandler _gameHandler;
        [field: SerializeField] public bool IsInspectable { get; set; }
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public ObjectType Type { get; set; }
        public Action<GameObject> onHoverObject { get; set; }
        public Action<GameObject> onExitHoverObject { get; set; }
        public Action<GameObject> onInteractObject { get; set; }

        public event Action onlevelUp;

        public virtual void OnDestroy()
        {
            onExitHoverObject = null;
            onHoverObject = null;
            onInteractObject = null;

            Debug.Log("Destroyed : " + gameObject.name);
        }

        public void IncreaseLevel(int levelIncrement)
        {
            Level = Level + levelIncrement;
            onlevelUp?.Invoke();
        }

        public void OnObjectInspected (out Interactables inspectedObject, GameObject inspectParent, GameObject playgroundParent, GameplayUIControl uiControl, Action additionalEvent = null)
        {
            inspectedObject = this;
            inspectParent.transform.position = new Vector3(inspectedObject.transform.position.x, 0, inspectedObject.transform.position.z);
            inspectedObject.transform.parent = inspectParent.transform;

            playgroundParent.SetActive(false);
            inspectParent.SetActive(true);
            uiControl.ToggleInspectVisibility(true);
            additionalEvent?.Invoke();
            Debug.Log("Inspecting Object : " + gameObject.name);
        }

        public void OnCharacterSelected(out Interactables inspectedCharacter, Action additionalEvent = null)
        {
            inspectedCharacter = this;
            additionalEvent?.Invoke();
            Debug.Log("Inspecting Object : " + gameObject.name);
        }

        public void ExitInspectObject
        (GameObject inspectParent, GameObject playgroundParent, GameplayUIControl uiControl, Action additionalEvent = null)
        {
            transform.parent = playgroundParent.transform;

            playgroundParent.SetActive(true);
            inspectParent.SetActive(false);
            uiControl.ToggleInspectVisibility(false);
            additionalEvent?.Invoke();
        }
    }
}