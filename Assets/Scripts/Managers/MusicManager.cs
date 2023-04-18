using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static float SOUND_EFFECT_VOLUME = 1f;
    public static float MUSIC_VOLUME = 1f;
    public static MusicManager instance;

    private float timer = 1f;

    [SerializeField] private GameObject[] sound_effects;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (timer >= 0f)
        { 
            timer -= Time.deltaTime;
        }

    }

    public void PlayLevelUp()
    {
        if (timer <= 0)
        {
            Instantiate(sound_effects[0], Vector3.one, Quaternion.identity);
            timer = 1f;
        }
    }

    public void ChangeAudioVolume()
    {
        SOUND_EFFECT_VOLUME = soundEffectSlider.value;
        MUSIC_VOLUME = musicSlider.value;
        audioSource.volume = MUSIC_VOLUME;
    }
}
