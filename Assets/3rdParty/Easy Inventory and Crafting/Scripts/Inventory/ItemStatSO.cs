using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;








[CreateAssetMenu(fileName = "New Stat", menuName = "EZ Inventory/Stat")]
public class ItemStatSO : ScriptableObject
{
	//public Image image;
	public float value;
	public float maxValue;
	public float fillAmount;
	public float overTime = .1f;
	


	public void UpdateStat(float amount)
	{
		value += amount;
		value = (Mathf.Clamp(value, 0f, maxValue));
		fillAmount = value / maxValue;
	}
}
