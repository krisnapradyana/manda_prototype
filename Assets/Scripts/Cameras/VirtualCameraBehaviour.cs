using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Gameplay
{
    public class VirtualCameraBehaviour : CameraCore
    {
        public CinemachineVirtualCamera virtualCamera { get; private set; }
        [field: SerializeField]
        public override int CameraId { get; set; }
        [field: SerializeField]
        public override int Priority { get; set; }

        private void Awake()
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public override void SetCameraPriority(int targetPriority)
        {
            base.SetCameraPriority(targetPriority);
            virtualCamera.Priority = Priority;
        }
    }
}
