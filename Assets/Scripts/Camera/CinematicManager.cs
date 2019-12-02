using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinematicManager : MonoBehaviour
{
    public static CinematicManager instance;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera[] vcams;

    public CinemachineVirtualCamera currentCam;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void ReturnToMainCam()
    {
        if (instance.currentCam == null)
            return;

        instance.currentCam.Priority = 0;
        instance.currentCam = null;

    }

    public void GoToCamOfIndex(int index)
    {

        if(instance.currentCam != null)
            instance.currentCam.Priority = 0;


        instance.currentCam = vcams[index];
        instance.currentCam.Priority = 11;
    }
}
