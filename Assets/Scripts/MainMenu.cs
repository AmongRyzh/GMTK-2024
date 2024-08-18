using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    Resolution[] resolutions;

    [Space(5)]
    [SerializeField] public AudioMixer audioMixer;

    [SerializeField] Slider musSlider;
    [SerializeField] Slider sfxSlider;

    private void Awake()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width}x{resolutions[i].height}@{resolutions[i].refreshRate}Hz";
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.value = currentResolutionIndex;
    }

    private void Start()
    {
        float musVolume = PlayerPrefs.GetFloat("musVolume", 0);
        float sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 0);

        audioMixer.SetFloat("musVolume", musVolume);
        audioMixer.SetFloat("sfxVolume", sfxVolume);

        musSlider.value = musVolume;
        sfxSlider.value = sfxVolume;
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", System.Convert.ToInt32(Screen.fullScreen));
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
    }

    public void ChangeMusVolume(float value)
    {
        audioMixer.SetFloat("musVolume", value);
        PlayerPrefs.SetFloat("musVolume", value);
    }

    public void ChangeSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
