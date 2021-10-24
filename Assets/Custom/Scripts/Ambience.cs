using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ambience : MonoBehaviour
{
	public static event System.Action<bool> isUnderWater;
	public AudioSource oceanSound;
	public AudioSource underWater;
	private void OnEnable()
	{
		
	}
	private OceanSampleHeightEvents waterHeight;
	
	private void Start()
	{
		waterHeight = GetComponent<OceanSampleHeightEvents>();
		if (!oceanSound.isPlaying)
		{
			oceanSound?.Play();
		}
		
	}
	public void OnBelowWater()
	{
		isUnderWater?.Invoke(true);
		if (oceanSound.isPlaying)
		{
			oceanSound?.Pause();
		}
		if (!underWater.isPlaying)
		{
			underWater?.Play();
		}
	}
	public void OnAboveWater()
	{
		isUnderWater?.Invoke(false);
		if (!oceanSound.isPlaying)
		{
			oceanSound?.Play();
		}
		if (underWater.isPlaying)
		{
			underWater?.Pause();
		}
	}
	public void Attenuate()
	{

	}

}
