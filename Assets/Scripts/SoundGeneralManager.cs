using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundGeneralType
{
    BUTTON_CLICK,
}

[RequireComponent(typeof(AudioSource))]
public class SoundGeneralManager : MonoBehaviour
{
    public static SoundGeneralManager Instance;

    [SerializeField] private AudioClip[] soundList;
    [SerializeField] private AudioSource musicBackground;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
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

    public static void PlaySound(SoundGeneralType sound, float volume = 1)
    {
        Instance.audioSource.PlayOneShot(Instance.soundList[(int)sound], volume);
    }
}
