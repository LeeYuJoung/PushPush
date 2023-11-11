using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance()
    {
        return instance;
    }

    public AudioSource audioSource;
    public AudioClip moveSound;
    public AudioClip correctSound;
    public AudioClip hpLessSound;
    public AudioClip stageClearSound;
    public AudioClip gameOverSound;
    public AudioClip dieSound;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void OnAudioPlay(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
