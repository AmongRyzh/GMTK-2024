using UnityEngine;
using UnityEngine.UI;

public class CustomCanvasSettings : MonoBehaviour
{
    PlayerMovement player;
    [SerializeField] Image playPauseButtonImage;
    [SerializeField] Sprite playButton, pauseButton;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    public void SwitchPlayPause()
    {
        player.SwitchPlayPause();
    }

    public void UpdatePlayPauseButtonImage()
    {
        playPauseButtonImage.sprite = player.speedState == PlayerMovement.SpeedState.Paused ? playButton : pauseButton;
    }

    public void RestartLevel()
    {
        player.RestartLevel();
    }
}
