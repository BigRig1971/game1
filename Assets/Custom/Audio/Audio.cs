using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        if (audioSource != null) Invoke(nameof(PlayAudio), Random.Range(0f, .5f));

	}
    void PlayAudio()
    {
		audioSource.Play();
	}
}
