using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider allVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private GameObject pauseWindow;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        StartVolume();
        StartResolution();
    }

    private void Update()
    {
        CloseWindowByKey();
    }

    #region Public Methods
    public void SetAllVolume()
    {
        float allVolume = allVolumeSlider.value;
        audioMixer.SetFloat("music", allVolume);
        audioMixer.SetFloat("sfx", allVolume);
        musicVolumeSlider.value = allVolume;
        sfxVolumeSlider.value = allVolume;
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicVolumeSlider.value;
        audioMixer.SetFloat("music", musicVolume);
        PlayerPrefs.SetFloat("musicVolumeValue", musicVolume);
    }

    public void SetSFXVolume()
    {
        float sfxVolume = sfxVolumeSlider.value;
        audioMixer.SetFloat("sfx", sfxVolume);
        PlayerPrefs.SetFloat("sfxVolumeValue", sfxVolume);
    }

    public void SetGraphics(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void CloseWindow(GameObject window)
    {
        window.SetActive(false);
    }

    public void OpenWindow(GameObject window)
    {
        window.SetActive(true);
    }

    #endregion

    #region Private Methods
    private void LoadVolume()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicVolumeValue");
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("sfxVolumeValue");
    }

    private void StartVolume()
    {
        if (PlayerPrefs.HasKey("musicVolumeValue") || PlayerPrefs.HasKey("sfxVolumeValue"))
        {
            LoadVolume();
        }
        else
        {
            SetAllVolume();
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    private void StartResolution()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    private void CloseWindowByKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameObject.activeSelf == true)
        {
            gameObject.SetActive(false);
            OpenWindow(pauseWindow);
        }
    }

    #endregion
}