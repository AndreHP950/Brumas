using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] AudioClip[] UI, Walk;
    private AudioSource audioSource;
    public static AudioManager Instance { get; private set; }

    public void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySound(int i)
    {
        Instance.audioSource.PlayOneShot(Instance.UI[i]);
    }
    public void PlaySoundButton(int i, float volume)
    {
            audioSource.PlayOneShot(Instance.UI[i], volume);
    }
    public static void WalkSound(int i)
    {
        Instance.audioSource.PlayOneShot(Instance.Walk[i]);
    }

}
