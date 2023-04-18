using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        audioSource.volume = MusicManager.SOUND_EFFECT_VOLUME;
        audioSource.Play();
        Destroy(this.gameObject, 1f);
    }
}
