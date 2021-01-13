using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlayAudio()
    {
        audioSource.Play();
    }

    public void StopAudio()
    {
        audioSource.Stop();
    }
}
