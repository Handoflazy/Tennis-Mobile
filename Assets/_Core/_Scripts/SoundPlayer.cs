using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer: MonoBehaviour {
    RandomPitch randomPitch;
    AudioSource source;

    [SerializeField] private AudioClip fireSound, hitBallSound;
    
    private void Awake() {
        randomPitch = GetComponent<RandomPitch>();
        source = GetComponent<AudioSource>();
    }

    public void SetRandomPitchAndPlay() {
        randomPitch.Set();
        source.Play();
    }
    public void PlayFireSound() {
        source.clip = fireSound;
        SetRandomPitchAndPlay();
    }

    public void PlayHitBallSound() {
        source.clip = fireSound;
        SetRandomPitchAndPlay();
    }
}