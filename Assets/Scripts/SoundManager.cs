using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    BUTTON_CLICK
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource MusicSource;
    [SerializeField] private AudioSource SFXSource;
    [SerializeField] private AudioMixer audioMixer;

    public AudioClip MusicBackground;
    public AudioClip[] SoundList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayBackgroundMusic(MusicBackground);
    }   

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    public void PlayBackgroundMusic(AudioClip clip)
    {
        if (MusicSource.clip != clip)
        {
            MusicSource.clip = clip;
            MusicSource.Play();
        }
    }

    public void StopBackgroundMusic()
    {
        MusicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        volume = volume / 100.0f;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        volume = volume / 100.0f;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
    }

    public void SoundButtonClick()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.SoundList[(int)SoundType.BUTTON_CLICK]);
    }
}
