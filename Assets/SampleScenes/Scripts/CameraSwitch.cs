using System;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class CameraSwitch : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCameraBase freelookCam;
    [SerializeField] CinemachineVirtualCameraBase aimCam;
    [SerializeField] CinemachineVirtualCameraBase bulletCam;
    [SerializeField] CinemachineVirtualCameraBase nozzleCam;
    private int m_CurrentActiveObject;
    [SerializeField] Image crossHair;
    private void OnEnable()
    {
        EventManager.StartListening("Aiming State Changed", (UnityEngine.Events.UnityAction<bool>)NextCamera);
        EventManager.StartListening("Penetrating Shot Triggered", MoveToNozzleCam);
        EventManager.StartListening("Penetrating Shot Fired", MoveToBulletCam);
        EventManager.StartListening("Penetrating Shot Hit", MoveToFreeLookCam);
    }


    private void OnDisable()
    {
        EventManager.StopListening("Aiming State Changed", (UnityEngine.Events.UnityAction<bool>)NextCamera);
        EventManager.StopListening("Penetrating Shot Triggered", MoveToNozzleCam);
        EventManager.StopListening("Penetrating Shot Fired", MoveToBulletCam);
        EventManager.StopListening("Penetrating Shot Hit", MoveToFreeLookCam);

    }
    private void MoveToFreeLookCam()
    {
        bulletCam.Priority = 0;
        aimCam.Priority = 0;
        freelookCam.Priority = 1;
    }
    private void MoveToBulletCam(Transform bullet)
    {
        bulletCam.transform.position = bullet.position ;
        bulletCam.transform.rotation = bullet.rotation;
        bulletCam.Follow = bullet;
        bulletCam.LookAt = bullet;
        bulletCam.Priority = 1;
        aimCam.Priority = 0;
        nozzleCam.Priority = 0;
    }
    private void MoveToNozzleCam()
    {

        nozzleCam.Priority = 1;
        aimCam.Priority = 0;
        crossHair.enabled = false;
    }
    void NextCamera(bool aiming)
    {
        if (aiming)
        {
            aimCam.Priority = 1;
            freelookCam.Priority = 0;
            crossHair.enabled = true;
        }
        else
        {
            aimCam.Priority = 0;
            freelookCam.Priority = 1;
            crossHair.enabled = false;
        }
    }

}
