using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;






[CreateAssetMenu(fileName = "New Stat", menuName = "EZ Inventory/Stat")]
public class ItemStatSO : ScriptableObject
{
	public Image image;
	public float value;
	public float maxValue;
	
	public void UpdateStat(int amount)
	{
		value += amount;
		image.fillAmount = value / maxValue;
	}
}
