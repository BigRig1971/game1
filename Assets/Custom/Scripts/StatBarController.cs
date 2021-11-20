using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatBarController : MonoBehaviour
{
	[System.Serializable]
	public class Stat
	{
		public ItemStatSO statBarSO;
		public Image image;
	}

	public Stat[] _stats;
	private void Awake()
	{
		foreach(Stat stat in _stats)
		{
			stat.statBarSO.image = stat.image;
		}	
	}
}
