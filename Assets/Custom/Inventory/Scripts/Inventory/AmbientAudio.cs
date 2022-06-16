using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StupidHumanGames
{
	public class AmbientAudio : MonoBehaviour
	{
		[System.Serializable]
		public class AmbientSounds
        {

			public AudioSource _ambientAudio;
			[Range(0f,1f)] public float _volume;
		}
		
		public AmbientSounds[] _ambientSounds;


		public void StartRandomSounds()
		{
			foreach (var sound in _ambientSounds)
			{
				sound._ambientAudio?.Play();
				sound._ambientAudio.volume = sound._volume;
			}
		}
		public void StopRandomSounds()
		{
			foreach (var sound in _ambientSounds)
			{
				sound._ambientAudio?.Pause();
			}
		}
	}
}
