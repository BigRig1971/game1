using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StupidHumanGames
{
	public class RandomSounds : MonoBehaviour
	{
		[System.Serializable]
		public class RndSound
        {
			public AudioSource _rndSound;
			public float _volume;
		}
		
		public RndSound[] _rndSounds;


		public void StartRandomSounds()
		{
			foreach (var sound in _rndSounds)
			{
				sound._rndSound?.Play();
				sound._rndSound.volume = sound._volume;
			}
		}
		public void StopRandomSounds()
		{
			foreach (var sound in _rndSounds)
			{
				sound._rndSound?.Pause();
			}
		}
	}
}
