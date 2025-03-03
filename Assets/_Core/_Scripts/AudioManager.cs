using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] private AudioClip scorePointAudio, losePointAudio, matchPointAudio;
    
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