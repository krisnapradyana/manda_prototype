using Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public class PlatformBehaviour : MonoBehaviour, ILevel
    {
        public int Level { get; private set; }
        public event Action onlevelUp;
        [Header("Platform Items")]
        [SerializeField] GameObject[] platformObjects;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void IncreaseLevel(int level)
        {
            Level = level;
            onlevelUp?.Invoke();
        }
    }
}
