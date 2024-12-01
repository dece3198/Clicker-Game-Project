using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    FullScreenMode screenMode;
    public Toggle fullscreenBtn;
    public GameObject settingUi;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private List<Resolution> resolutions = new List<Resolution>();
    public int resolutionNum;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitUi();
    }

    public void BGMSoundVolume(float val)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(val) * 20);
    }

    public void SFXoundVolume(float val)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(val) * 20);
    }

    private void InitUi()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value >= 70 && Screen.resolutions[i].refreshRateRatio.value <= 80)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        dropdown.options.Clear();

        int optionNum = 0;
        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + " X " + item.height + " " + item.refreshRateRatio.value.ToString("N0") + "hz";
            dropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
            {
                dropdown.value = optionNum;
                optionNum++;
            }
        }
        dropdown.RefreshShownValue();
        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropBoxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void OkButton()
    {
        Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, screenMode);
        settingUi.SetActive(false);
    }

    public void MenuButton()
    {
        settingUi.SetActive(true);
    }
}
    