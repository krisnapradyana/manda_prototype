using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Gameplay
{
    public class FreeLookCameraBehaviour : CameraCore
    {
        public CinemachineFreeLook freeLookCamera { get; set; }
        [field: SerializeField]
        public override int CameraId { get; set; }
        [field: SerializeField]
        public override int Priority { get; set; }

        // Start is called before the first frame update
        void Awake()
        {
            freeLookCamera = GetComponent<CinemachineFreeLook>();
        }

        public override void SetCameraPriority(int targetPriority)
        {
            base.SetCameraPriority(targetPriority);
            freeLookCamera.Priority = Priority;
        }

    }
}
