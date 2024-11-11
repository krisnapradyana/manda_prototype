using Singletons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{ 
    public class RootHandler : MonoBehaviour
    {
        [Header("Root Handler Parameters")]
        //Publics
        [HideInInspector] public GameCentralSystem centralSystem;
        [HideInInspector] public InputListener inputListener;
        [HideInInspector] public bool IsInspecting;
        [property: SerializeField] public CharacterBehaviour ControlledPlayer
        {
            get
            {
                CharacterBehaviour result = null;
                foreach (var item in _players)
                {
                    if (item.IsSelected)
                    {
                        result = item;
                    }
                }
                return result;
            }
        }

        //Privates
        [field: SerializeField] public CharacterBehaviour[] _npcs;
        [field: SerializeField] public CharacterBehaviour[] _players;
        [field: SerializeField] public CameraCore[] _worldCameras;
        [field: SerializeField] public ObjectBehaviour[] _objects;

        //Events
        public Action<CameraCore[]> E_ResetAllVirtualCamera;
        public Action<CameraCore[]> E_AssignCameraPriority;

        protected void Awake()
        {
            Debug.Log("Starting handler");
            centralSystem = FindObjectOfType<GameCentralSystem>();
            inputListener = FindObjectOfType<InputListener>();
        }


        public void AssignCameraPriority(int comparedId, CameraCore[] collectionList)//, bool saveLastId = true)
        {
            CameraCore selectedCam = null;
            foreach (var item in collectionList)
            {
                if (item.CameraId == comparedId)
                {
                    item.SetCameraPriority(1);
                    selectedCam = item;
                    //Think about this later
                }
            }
        }

        public void ResetAllVirtualCameraPriority(CameraCore[] collectionList)
        {
            foreach (var item in collectionList)
            {
                item.SetCameraPriority(0);
            }
        }
    }
}
