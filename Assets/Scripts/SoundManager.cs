using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
