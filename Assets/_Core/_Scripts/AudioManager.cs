using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip gameOver;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip Notification;
    
    public void PlayGameOver()
    {
        audioSource.PlayOneShot(gameOver);
    }
    public void PlaySuccess()
    {
        audioSource.PlayOneShot(success);
    }
    public void PlayNotification()
    {
        audioSource.PlayOneShot(Notification);
    }
    
}