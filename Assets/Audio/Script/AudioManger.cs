using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManger : MonoBehaviour
{
    public static AudioManger instance;

    [SerializeField] private AudioSource effet, music;

    public AudioClip[] audioClips;
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        musicPlaySound(audioClips[0]);
    }

    public void effetPlaySound(AudioClip audioClip)
    {
        effet.PlayOneShot(audioClip);
    }
    public void musicPlaySound(AudioClip audioClip)
    {
        music.clip = audioClip;
        music.loop = true;
        music.Play();
    }
}
