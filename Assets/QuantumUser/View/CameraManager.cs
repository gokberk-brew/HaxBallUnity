using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Client.StructWrapping;
using Quantum;
using UnityEngine;
using Input = UnityEngine.Input;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] private CinemachineVirtualCamera _vcam;
    public static CinemachineVirtualCamera VCam => Instance._vcam;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
