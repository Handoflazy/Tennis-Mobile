using System;
using UnityEngine;
using UnityServiceLocator;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] private AudioClip scorePointAudio, losePointAudio, matchPointAudio;

    private void Awake() {
        ServiceLocator.ForSceneOf(this).Register(this);
    }

    public void PlayScorePoint()
    {
        audioSource.PlayOneShot(scorePointAudio);
    }
    public void PlayLosePoint()
    {
        audioSource.PlayOneShot(losePointAudio);
    }
    public void PlayMatchPoint()
    {
        audioSource.PlayOneShot(matchPointAudio);
    }
    
}