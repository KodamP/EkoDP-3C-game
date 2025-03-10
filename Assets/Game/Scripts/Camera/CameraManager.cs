using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] public CameraState CameraState;
    [SerializeField] private CinemachineVirtualCamera _fpsCamera;
    [SerializeField] private CinemachineFreeLook _tpsCamera;

    private void Start()
    {
        InputEventManager.OnChangePOV += SwitchCamera;
    }

    private void OnDestroy()
    {
        InputEventManager.OnChangePOV -= SwitchCamera;
    }

    public void SetFPSClampedCamera(bool isClamped, Vector3 playerRotation)
    {
        CinemachinePOV pov = _fpsCamera.GetCinemachineComponent<CinemachinePOV>();
        if (isClamped)
        {
            pov.m_HorizontalAxis.m_Wrap = false;
            pov.m_HorizontalAxis.m_MinValue = playerRotation.y - 75;
            pov.m_HorizontalAxis.m_MaxValue = playerRotation.y + 75;
        }
        else
        {
            pov.m_HorizontalAxis.m_MinValue = -180;
            pov.m_HorizontalAxis.m_MaxValue = 180;
            pov.m_HorizontalAxis.m_Wrap = true;
        }
    }

    public void SwitchCamera()
    {
        //Siapa tau bakal ada kamera jenis lain, biar ga if else beranak pinak.
        switch (CameraState)
        {
            case CameraState.ThirdPerson:
                CameraState = CameraState.FirstPerson;
                _tpsCamera.gameObject.SetActive(false);
                _fpsCamera.gameObject.SetActive(true);
                break;
            case CameraState.FirstPerson:
                CameraState = CameraState.ThirdPerson;
                _tpsCamera.gameObject.SetActive(true);
                _fpsCamera.gameObject.SetActive(false);
                break;
        }
        PlayerEventManager.FireOnChangePOV();
    }

    public void SetTPSFieldOfView(float fieldOfView)
    {
        _tpsCamera.m_Lens.FieldOfView = fieldOfView;
    }
}
