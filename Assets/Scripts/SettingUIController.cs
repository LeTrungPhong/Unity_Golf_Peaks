using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class SettingUIController : MonoBehaviour
{
    // root
    //private VisualElement root;

    // container
    private VisualElement _containerSettingVisualElement;

    // setting
    private Button _settingButton;
    private bool displaySetting = false;

    // audio

    // music
    private Button _lowMusicButton;
    private Button _highMusicButton;
    private Label _valueMusicLabel;

    // SFX
    private Button _lowSFXButton;
    private Button _highSFXButton;
    private Label _valueSFXLabel;

    // mute
    private Button _muteButton;

    // accessibility

    private Button _controlDirectionButton;
    private Button _unlockLevelButton;
    private Button _clearLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // container
        _containerSettingVisualElement = root.Q<VisualElement>("container_setting_visualElement");

        // button setting
        _settingButton = root.Q<Button>("button_setting");

        // biding
        // audio
        // music
        _lowMusicButton = root.Q<Button>("button_low_music");
        _highMusicButton = root.Q<Button>("button_high_music");
        _valueMusicLabel = root.Q<Label>("label_val_music");
        // SFX
        _lowSFXButton = root.Q<Button>("button_low_SFX");
        _highSFXButton = root.Q<Button>("button_high_SFX");
        _valueSFXLabel = root.Q<Label>("label_val_SFX");
        // mute
        _muteButton = root.Q<Button>("button_mute");
        // accessibility
        _controlDirectionButton = root.Q<Button>("button_control_direction");
        _unlockLevelButton = root.Q<Button>("button_unlock_level");
        _clearLevelButton = root.Q<Button>("button_clear_level");

        // register
        _settingButton.RegisterCallback<ClickEvent>(ButtonSettingClick);
        _lowMusicButton.RegisterCallback<ClickEvent>(LowMusic);
        _highMusicButton.RegisterCallback<ClickEvent>(HighMusic);
        _lowSFXButton.RegisterCallback<ClickEvent>(LowSFX);
        _highSFXButton.RegisterCallback<ClickEvent>(HighSFX);
        _muteButton.RegisterCallback<ClickEvent>(Disabled);
        _controlDirectionButton.RegisterCallback<ClickEvent>(Control);
        _unlockLevelButton.RegisterCallback<ClickEvent>(Unlock);
        _clearLevelButton.RegisterCallback<ClickEvent>(Clear);

        SetUpSetting();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonSettingClick(ClickEvent evt)
    {
        if (displaySetting == false)
        {
            _containerSettingVisualElement.style.translate = new StyleTranslate(new Translate(Length.Percent(10), Length.Percent(-50)));
            _settingButton.style.scale = new StyleScale(new Scale(new Vector2(0.8f, 0.8f)));
            _settingButton.style.rotate = new StyleRotate(new Rotate(180));
            displaySetting = true;
        } else
        {
            _containerSettingVisualElement.style.translate = new StyleTranslate(new Translate(Length.Percent(-100), Length.Percent(-50)));
            _settingButton.style.scale = new StyleScale(new Scale(new Vector2(0.6f, 0.6f)));
            _settingButton.style.rotate = new StyleRotate(new Rotate(0));
            displaySetting = false;
        }
        SoundManager.Instance.SoundButtonClick();
    }

    public void SetUpSetting()
    {
        float notFound = -100.0f;

        float volumeMusic = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsMusic, notFound);
        if (volumeMusic == notFound)
        {
            volumeMusic = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsMusic, volumeMusic);
        }
        _valueMusicLabel.text = volumeMusic.ToString();
        float volumeSFX = PlayerPrefs.GetFloat(PlayerPrefsName.playerPrefsSFX, notFound);
        if (volumeSFX == notFound)
        {
            volumeSFX = 50.0f;
            PlayerPrefs.SetFloat(PlayerPrefsName.playerPrefsSFX, volumeSFX);
        }
        _valueSFXLabel.text = volumeSFX.ToString();

        string muteVolume = PlayerPrefs.GetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.falsePrefs);
        if (muteVolume == PlayerPrefsName.truePrefs)
        {
            _muteButton.text = "ENABLED";
            SoundManager.Instance.SetMusicVolume(0);
            SoundManager.Instance.SetSFXVolume(0);
            //RawImageMute.SetActive(true);
        } else
        {
            _muteButton.text = "DISABLED";      
            SoundManager.Instance.SetMusicVolume(volumeMusic);
            SoundManager.Instance.SetSFXVolume(volumeSFX);
            //RawImageMute.SetActive(false);
        }

        string unlock = PlayerPrefs.GetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.falsePrefs);
        if (unlock == PlayerPrefsName.truePrefs)
        {
            _unlockLevelButton.text = "DISABLED";
        } else
        {
            _unlockLevelButton.text = "ENABLED";
        }

        string control = PlayerPrefs.GetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.falsePrefs);
        if (control == PlayerPrefsName.truePrefs)
        {
            _controlDirectionButton.text = "INVERTED";
        } else
        {
            _controlDirectionButton.text = "REGULAR";
        }
    }

    public void HighMusic(ClickEvent evt)
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
        SetUpSetting();
    }

    public void LowMusic(ClickEvent evt)
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
        SetUpSetting();
    }

    public void HighSFX(ClickEvent evt)
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
        SetUpSetting();
    }

    public void LowSFX(ClickEvent evt)
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
        SetUpSetting();
    }

    public void Disabled(ClickEvent evt)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (PlayerPrefs.GetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.falsePrefs) == PlayerPrefsName.falsePrefs)
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.truePrefs);
        } else
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsMute, PlayerPrefsName.falsePrefs);
        }
        SetUpSetting();
    }

    public void Control(ClickEvent evt)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (PlayerPrefs.GetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.falsePrefs) == PlayerPrefsName.falsePrefs)
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.truePrefs);
            _controlDirectionButton.text = "INVERTED";
        } else
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsControl, PlayerPrefsName.falsePrefs);
            _controlDirectionButton.text = "REGULAR";
        }
    }

    public void Unlock(ClickEvent evt)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        if (PlayerPrefs.GetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.falsePrefs) == PlayerPrefsName.falsePrefs)
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.truePrefs);
            _unlockLevelButton.text = "DISABLED";
        } else
        {
            PlayerPrefs.SetString(PlayerPrefsName.playerPrefsUnlock, PlayerPrefsName.falsePrefs);
            _unlockLevelButton.text = "ENABLED";
        }
        SceneManager.LoadScene("Level");
    }

    public void Clear(ClickEvent evt)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
        foreach(string key in LevelManager.Instance.listPathLevelData)
        {
            PlayerPrefs.DeleteKey(key);
        }
        SceneManager.LoadScene("Level");
    }
}
