using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
    public class RoomPreferences : MonoBehaviour
    {
        [field : SerializeField] public int roomId { get; private set; }
    }
}
