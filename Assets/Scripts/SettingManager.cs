using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    private Color colorGeneral = new Color(113f / 255f, 113f / 255f, 113f / 255f, 1);
    [SerializeField] private TMP_Text textMusic;
    [SerializeField] private TMP_Text textSFX;
    [SerializeField] private TMP_Text textMute;
    [SerializeField] private TMP_Text textUnlock;
    [SerializeField] private TMP_Text textClear;
    [SerializeField] private TMP_Text textControl;

    

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

        float volumeMusic = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsMusic, notFound);
        if (volumeMusic == notFound)
        {
            volumeMusic = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsMusic, volumeMusic);
        }
        textMusic.SetText(volumeMusic.ToString());

        float volumeSFX = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsSFX, notFound);
        if (volumeSFX == notFound)
        {
            volumeSFX = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsSFX, volumeSFX);
        }
        textSFX.SetText(volumeSFX.ToString());

        string muteVolume = PlayerPrefs.GetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.falsePrefs);
        if (muteVolume == PlayerPrefsName.truePrefs)
        {
            textMute.SetText("ENABLED");
            SoundManager.Instance.SetMusicVolume(0);
            SoundManager.Instance.SetSFXVolume(0);
            RawImageMute.SetActive(true);
        } else
        {
            textMute.SetText("DISABLED");      
            SoundManager.Instance.SetMusicVolume(volumeMusic);
            SoundManager.Instance.SetSFXVolume(volumeSFX);
            RawImageMute.SetActive(false);
        }

        string unlock = PlayerPrefs.GetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.falsePrefs);
        if (unlock == PlayerPrefsName.truePrefs)
        {
            textUnlock.SetText("DISABLED");
        } else
        {
            textUnlock.SetText("ENABLED");
        }

        string control = PlayerPrefs.GetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.falsePrefs);
        if (control == PlayerPrefsName.truePrefs)
        {
            textControl.SetText("INVERTED");
        } else
        {
            textControl.SetText("REGULAR");
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
        float volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsMusic, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsMusic, volume);
        }
        else
        {
            volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsMusic) + 10;
            if (volume > 100) volume = 100;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsMusic, volume);
        }
        SetUpVolume();
    }

    public void LowMusic()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        float notFound = -100.0f;
        float volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsMusic, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsMusic, volume);
        } else
        {
            volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsMusic) - 10;
            if (volume < 0) volume = 0;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsMusic, volume);
        }
        SetUpVolume();
    }

    public void HighSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        float notFound = -100.0f;
        float volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsSFX, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsSFX, volume);
        }
        else
        {
            volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsSFX) + 10;
            if (volume > 100) volume = 100;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsSFX, volume);
        }
        SetUpVolume();
    }

    public void LowSFX()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        float notFound = -100.0f;
        float volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsSFX, notFound);
        if (volume == notFound)
        {
            volume = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsSFX, volume);
        }
        else
        {
            volume = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsSFX) - 10;
            if (volume < 0) volume = 0;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsSFX, volume);
        }
        SetUpVolume();
    }

    public void Disabled()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (PlayerPrefs.GetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.falsePrefs) == PlayerPrefsName.falsePrefs)
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.truePrefs);
        } else
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.falsePrefs);
        }
        SetUpVolume();
    }

    public void Control()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (PlayerPrefs.GetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.falsePrefs) == PlayerPrefsName.falsePrefs)
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.truePrefs);
            textControl.SetText("INVERTED");
        } else
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.falsePrefs);
            textControl.SetText("REGULAR");
        }
    }

    public void Unlock()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (PlayerPrefs.GetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.falsePrefs) == PlayerPrefsName.falsePrefs)
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.truePrefs);
            textUnlock.SetText("DISABLED");
        } else
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.falsePrefs);
            textUnlock.SetText("ENABLED");
        }
        SceneManager.LoadScene("Level");
    }

    public void Clear()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        foreach(string key in LevelManager.Instance.listPathLevelData)
        {
            PlayerPrefs.DeleteKey(key);
        }
        SceneManager.LoadScene("Level");
    }
}
