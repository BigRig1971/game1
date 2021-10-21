using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSounds : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource source;
    public float minWaitBetweenPlays = 1f;
    public float maxWaitBetweenPlays = 5f;
    public float waitTimeCountdown = -1f;
    [SerializeField]
    private float maxDistance = 50;
    [SerializeField]
    private float volume = .1f;
    private float pitch = 1f;
    private bool attenuateSound = false;

	private void OnEnable()
	{
        Ambience.isUnderWater += Attenuate;
	}
	void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
		if (!attenuateSound)
		{
            if (!source.isPlaying)
            {
                if (waitTimeCountdown < 0f)
                {
                    currentClip = audioClips[Random.Range(0, audioClips.Count)];
                    source.clip = currentClip;
                    source.volume = volume * Random.Range(.8f, 1f);
                    source.minDistance = 1f;
                    source.maxDistance = maxDistance;
                    source.spread = 0.3f;
                    source.spatialize = true;
                    source.spatialBlend = 1f;
                    source.dopplerLevel = 2f;
                    source.pitch = pitch * Random.Range(.9f, 1.1f);
                    source.rolloffMode = AudioRolloffMode.Custom;
                    source.Play();
                    waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
                }
                else
                {
                    waitTimeCountdown -= Time.deltaTime;
                }
            }
        }
       
    }
    private void OnDisable()
    {
        Ambience.isUnderWater -= Attenuate;
    }
    void Attenuate(bool a)
	{
        attenuateSound = a;
	}
}
