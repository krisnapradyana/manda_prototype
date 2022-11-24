using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CameraBehaviour : MonoBehaviour
{   
    private CinemachineFreeLook _freelook;
    [field : SerializeField]
    public int CameraId { get; private set; }
    [field : SerializeField]
    public int Priority { get => _freelook.Priority; private set => _freelook.Priority = value; }

    private void Start()
    {
        _freelook = GetComponent<CinemachineFreeLook>();
    }

    public void SetCameraPriority(int targetPriority)
    {
        Priority = targetPriority;
    }
}
