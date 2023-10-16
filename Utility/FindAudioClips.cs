using System.Collections.Generic;
using UnityEngine;

public class FindAudioClips : MonoBehaviour
{
    void Start()
    {
        // Find all GameObjects with AudioSource components
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        // Create a HashSet to store unique AudioClips
        HashSet<AudioClip> uniqueAudioClips = new HashSet<AudioClip>();

        // Add AudioClips to the HashSet
        foreach (var audioSource in audioSources)
        {
            if (audioSource.clip != null)
            {
                uniqueAudioClips.Add(audioSource.clip);
            }
        }

        // Log the unique AudioClips
        Debug.Log("Unique AudioClips being used in the scene:");
        foreach (var audioClip in uniqueAudioClips)
        {
            Debug.Log(audioClip.name);
        }
    }
}
