using Cinemachine;
using UnityEngine;

public class CameraSwapTrigger : MonoBehaviour
{
    [SerializeField] private Dir direction;

    [DrawIf("direction", Dir.Vertical, ComparisonType.Equals)]
    [SerializeField] private CinemachineVirtualCamera cameraUp;

    [DrawIf("direction", Dir.Vertical, ComparisonType.Equals)]
    [SerializeField] private CinemachineVirtualCamera cameraDown;

    [DrawIf("direction", Dir.Horizontal, ComparisonType.Equals)]
    [SerializeField] private CinemachineVirtualCamera cameraLeft;

    [DrawIf("direction", Dir.Horizontal, ComparisonType.Equals)]
    [SerializeField] private CinemachineVirtualCamera cameraRight;

    enum Dir
    {
        Horizontal,
        Vertical
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            Vector2 exitDir = (other.transform.position - GetComponent<Collider2D>().bounds.center).normalized;
            print(exitDir);

            if (direction == Dir.Horizontal)
            {
                CameraManager.Instance.SwapCameraHor(cameraLeft, cameraRight, exitDir);
            }
            else
            {
                CameraManager.Instance.SwapCameraVer(cameraDown, cameraUp, exitDir);
            }
        }
    }
}