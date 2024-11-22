using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    private int bgmVolume = 5, sfxVolume = 5;

    [SerializeField] AudioClip bgmClip;
    [SerializeField] AudioClip[] sfxClips;
    void Awake()
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
    void Start()
    {
        if (bgmClip != null)
        { PlayBGM(bgmClip); }
    }
    public void PlayBGM(AudioClip clip) { bgmSource.clip = clip; bgmSource.loop = true; bgmSource.Play(); }
    public void PlaySFX(int index)
    {
        if (index >= 0 && index < sfxClips.Length)
        {
            if (sfxSource.isPlaying && sfxSource.clip == sfxClips[index])
            {
                sfxSource.Stop();
            }
            sfxSource.clip = sfxClips[index];
            sfxSource.Play();
        }
    }
    public void StopBGM() { bgmSource.Stop(); }
    public void StopSFX() { sfxSource.Stop(); }
    public void PLayerAttack()
    {
        PlaySFX(1);
    }
    public void UpdateVolumes() { bgmSource.volume = bgmVolume / 5f; sfxSource.volume = sfxVolume / 5f; }
    public void SetBGMVolume(int volume) { bgmVolume = volume; UpdateVolumes(); }
    public void SetSFXVolume(int volume) { sfxVolume = volume; UpdateVolumes(); }
    public int GetBGMVolume() => bgmVolume;
    public int GetSFXVolume() => sfxVolume;
}
