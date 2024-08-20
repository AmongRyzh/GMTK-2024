using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineVirtualCamera[] allCamerasInScene;
    [HideInInspector] public CinemachineVirtualCamera currentCamera;
    [HideInInspector] public CinemachineVirtualCamera firstActiveCamera;
    [HideInInspector] public CinemachineVirtualCamera prevActiveCamera;
    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < allCamerasInScene.Length; i++)
        {
            if (allCamerasInScene[i].enabled)
            {
                currentCamera = allCamerasInScene[i];
                firstActiveCamera = allCamerasInScene[i];
            }
        }
        
        /*GlobalEventManager.onPlayerDied.AddListener(() => {
            foreach (var camera in allCamerasInScene)
            {
                camera.Follow = FindObjectOfType<PlayerController>().transform;
            }
        });*/
    }

    /*private void OnDestroy()
    {
        GlobalEventManager.onPlayerDied.RemoveListener(() => {
            foreach (var camera in allCamerasInScene)
            {
                camera.Follow = FindObjectOfType<PlayerController>().transform;
            }
        });
    }*/

    public void SwapCameraHor(CinemachineVirtualCamera cameraFromLeft,
        CinemachineVirtualCamera cameraFromRight,
        Vector2 triggerExitDir)
    {
        if (currentCamera == cameraFromLeft && triggerExitDir.x > 0f)
        {
            cameraFromRight.enabled = true;

            cameraFromLeft.enabled = false;

            currentCamera = cameraFromRight;
        }
        if (currentCamera == cameraFromRight && triggerExitDir.x < 0f)
        {
            cameraFromLeft.enabled = true;

            cameraFromRight.enabled = false;

            currentCamera = cameraFromLeft;
        }
    }

    public void SwapCameraVer(CinemachineVirtualCamera cameraFromDown,
        CinemachineVirtualCamera cameraFromUp,
        Vector2 triggerExitDir)
    {
        if (currentCamera == cameraFromDown && triggerExitDir.y > 0f)
        {
            cameraFromUp.enabled = true;

            cameraFromDown.enabled = false;

            currentCamera = cameraFromUp;
        }
        if (currentCamera == cameraFromUp && triggerExitDir.y < 0f)
        {
            cameraFromDown.enabled = true;

            cameraFromUp.enabled = false;

            currentCamera = cameraFromDown;
        }
    }
}