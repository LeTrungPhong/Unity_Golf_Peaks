using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    ROLL
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private static SoundManager Instance;
    [SerializeField] private AudioClip[] soundList;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }

    public static void PlayLoopSound(SoundType sound, float volume = 1)
    {
        Instance.audioSource.loop = true;
        Instance.audioSource.clip = Instance.soundList[(int)sound];
        Instance.audioSource.volume = 1;
        Instance.audioSource.Play();
    }

    public static void StopLoopSound()
    {
        Instance.audioSource.Stop();
    }
}
