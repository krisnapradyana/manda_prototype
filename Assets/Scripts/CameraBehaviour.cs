using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraBehaviour : MonoBehaviour
{   
    private CinemachineVirtualCamera virtualCamera;
    [field : SerializeField]
    public int CameraId { get; private set; }
    [field : SerializeField]
    public int Priority { get => virtualCamera.Priority; private set => virtualCamera.Priority = value; }

    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void SetCameraPriority(int targetPriority)
    {
        Priority = targetPriority;
    }
}
