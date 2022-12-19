using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public interface ILevel
    {
        public int Level { get; }
        public int MaxLevel { get; }
        public int LevelBaseCost { get; }
        public event Action onlevelUp;
        public void IncreaseLevel(int level, Action maxLevelCallback = null);
    }
}
