using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
      if(audioSource!=null)  audioSource.Play();
    }
}
