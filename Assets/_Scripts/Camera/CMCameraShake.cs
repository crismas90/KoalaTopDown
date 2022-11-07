using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMCameraShake : MonoBehaviour
{
    public static CMCameraShake Instance { get; private set; }

    private CinemachineVirtualCamera cmVirtCam;
    private float shakeTimer;

    private void Awake()
    {
        Instance = this;
        cmVirtCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cmBasic = cmVirtCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cmBasic.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                CinemachineBasicMultiChannelPerlin cmBasic = cmVirtCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cmBasic.m_AmplitudeGain = 0f;
            }
        }
    }
}
