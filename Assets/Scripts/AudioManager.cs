using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void PlayAudio()
    {
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    public void PauseAudio()
    {
        audioSource.Pause();
    }
}
