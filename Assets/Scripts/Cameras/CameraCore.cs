using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CameraCore : MonoBehaviour
{
    public virtual int CameraId { get; set; }
    public virtual int Priority { get; set; }
    public virtual float CameraRotationValue { get; set; }
    public virtual void SetCameraPriority(int targetPriority)
    {
        Priority = targetPriority;
    }

    public virtual void OnCameraRotation(float value)
    {
        CameraRotationValue = value;
    }
}
