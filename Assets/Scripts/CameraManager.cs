using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField]
    private List<CinemachineVirtualCamera> _virtualCameras;

    public void ChooseCamera(CinemachineVirtualCamera virtualCamera)
    {
        if (virtualCamera == null || _virtualCameras == null) return;

        foreach (CinemachineVirtualCamera vcam in _virtualCameras)
        {
            vcam.Priority = 0;
        }

        virtualCamera.Priority = 1;
    }
}
