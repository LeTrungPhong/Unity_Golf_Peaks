using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    private Color colorGeneral = new Color(113f / 255f, 113f / 255f, 113f / 255f, 1);
    [SerializeField] private TMP_Text textMusic;
    [SerializeField] private TMP_Text textSFX;
    [SerializeField] private TMP_Text textMute;

    private string playerPrefsMusic = "VolumeMusic";
    private string playerPrefsSFX = "VolumeSFX";
    private string playerPrefsMute = "MuteVolume";

    private GameObject setting;
    private GameObject RawImageMute;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        setting = GameObject.Find("Setting");
        RawImageMute = GameObject.Find("RawImageMute");
        setting.GetComponent<Image>().color = colorGeneral;
        textMusic.color = colorGeneral;
        textSFX.color = colorGeneral;
        setting.SetActive(false);
    }

    public void SetUpVolume()
    {
        float notFound = -100.0f;

        string muteVolume = PlayerPrefs.GetString(playerPrefsMute, "false");

        float volumeMusic = PlayerPrefs.GetFloat(playerPrefsMusic, notFound);
        if (volumeMusic == notFound)
        {
            volumeMusic = 50.0f;
            PlayerPrefs.SetFloat(playerPrefsMusic, volumeMusic);
        }
        textMusic.SetText(volumeMusic.ToString());

        float volumeSFX = PlayerPrefs.GetFloat(playerPrefsSFX, notFound);
        if (volumeSFX == notFound)
        {
            volumeSFX = 50.0f;
            PlayerPrefs.SetFloat(playerPrefsSFX, volumeSFX);
        }
        textSFX.SetText(volumeSFX.ToString());

        if (muteVolume == "true")
        {
            textMute.SetText("Enabled");
            SoundManager.Instance.SetMusicVolume(0);
            SoundManager.Instance.SetSFXVolume(0);
            RawImageMute.SetActive(true);
        } else
        {
            textMute.SetText("Disabled");      
            SoundManager.Instance.SetMusicVolume(volumeMusic);
            SoundManager.Instance.SetSFXVolume(volumeSFX);
            RawImageMute.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSettingClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (setting.activeSelf == true)
        {
            setting.SetActive(false);
            return;
        }
        setting.SetActive(true);
        setting.transform.SetAsLastSibling();
        SetUpVolume();
    }

    public void HighMusic()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        float notFound = -100.0f;
        float volume = PlayerPrefs.GetFloat(playerPrefsMusic, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(playerPrefsMusic, volume);
        }
        else
        {
            volume = PlayerPrefs.GetFloat(playerPrefsMusic) + 10;
            if (volume > 100) volume = 100;
            PlayerPrefs.SetFloat(playerPrefsMusic, volume);
        }
        SetUpVolume();
    }

    public void LowMusic()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        float notFound = -100.0f;
        float volume = PlayerPrefs.GetFloat(playerPrefsMusic, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(playerPrefsMusic, volume);
        } else
        {
            volume = PlayerPrefs.GetFloat(playerPrefsMusic) - 10;
            if (volume < 0) volume = 0;
            PlayerPrefs.SetFloat(playerPrefsMusic, volume);
        }
        SetUpVolume();
    }

    public void HighSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        float notFound = -100.0f;
        float volume = PlayerPrefs.GetFloat(playerPrefsSFX, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(playerPrefsSFX, volume);
        }
        else
        {
            volume = PlayerPrefs.GetFloat(playerPrefsSFX) + 10;
            if (volume > 100) volume = 100;
            PlayerPrefs.SetFloat(playerPrefsSFX, volume);
        }
        SetUpVolume();
    }

    public void LowSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        float notFound = -100.0f;
        float volume = PlayerPrefs.GetFloat(playerPrefsSFX, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(playerPrefsSFX, volume);
        }
        else
        {
            volume = PlayerPrefs.GetFloat(playerPrefsSFX) - 10;
            if (volume < 0) volume = 0;
            PlayerPrefs.SetFloat(playerPrefsSFX, volume);
        }
        SetUpVolume();
    }

    public void Disabled()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (PlayerPrefs.GetString(playerPrefsMute, "false") == "false")
        {
            PlayerPrefs.SetString(playerPrefsMute, "true");
        } else
        {
            PlayerPrefs.SetString(playerPrefsMute, "false");
        }
        SetUpVolume();
    }

    public void Control()
    {

    }

    public void Unlock()
    {

    }

    public void Clear()
    {

    }
}
