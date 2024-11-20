using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource; // Attach an AudioSource component
    [SerializeField] private AudioClip[] soundEffects; // Array of sound effects to randomize from
    [SerializeField] private string targetTag;
    private bool isPlaying;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == targetTag)
        {
            PlayRandomSoundEffect();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == targetTag)
        {
            PlayRandomSoundEffect();
        }
    }

    // Function to play a random sound effect
    public void PlayRandomSoundEffect()
    {
        Debug.LogWarning($"is played");

        if (soundEffects.Length == 0)
        {
            Debug.LogWarning("No sound effects assigned!");
            return;
        }
        if (!isPlaying)
        {
            isPlaying = true;

            // Choose a random clip from the array
            int randomIndex = Random.Range(0, soundEffects.Length);
            AudioClip randomClip = soundEffects[randomIndex];

            // Play the chosen clip
            audioSource.clip = randomClip;
            StartCoroutine(ResetAudioFlag(audioSource.clip.length));
        }
    }

    private IEnumerator ResetAudioFlag(float delay)
    {
        audioSource.Play();
        Debug.Log("walk");
        yield return new WaitForSeconds(delay);
        Debug.Log("walk end");
        isPlaying = false;
    }
}
