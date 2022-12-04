using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
	Slider slider;
	private void Start()
	{
		slider = GetComponent<Slider>();

	}
	public void SetValue(int value)
	{
		slider.value = value;
	}
	public void SetMaxValue(int maxValue)
	{
		slider.maxValue = maxValue;
	}
}