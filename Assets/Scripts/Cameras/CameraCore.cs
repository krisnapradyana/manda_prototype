using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCore : MonoBehaviour
{
    public virtual int CameraId { get; set; }
    public virtual int Priority { get; set; }
    public virtual void SetCameraPriority(int targetPriority)
    {
        Priority = targetPriority;
    }
}
