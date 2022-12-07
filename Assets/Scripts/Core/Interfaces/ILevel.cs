using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gameplay
{
    public interface ILevel
    {
        public int Level { get; }
        public event Action onlevelUp;
        public void IncreaseLevel(int level);
    }
}
