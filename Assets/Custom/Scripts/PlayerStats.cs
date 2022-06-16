using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
	public ItemStatSO health;
	public Image healthImage;
	public ItemStatSO oxygen;
	public Image oxygenImage;
	public bool canBreath = true;



	private void Update()
	{
		OnOxygenBar();
		OnHealthBar();
	}


	public void OnHoldBreath()
	{
		canBreath = false;
	}
	public void OnBreath()
	{
		canBreath = true;
	}
	void OnOxygenBar()
	{
		oxygenImage.fillAmount = oxygen.fillAmount;
		if (canBreath) oxygen.UpdateStat(.03f);
		if (!canBreath) oxygen.UpdateStat(-.1f);
	}
	void OnHealthBar()
	{
		healthImage.fillAmount = health.fillAmount;
		if (Input.GetKeyDown(KeyCode.M) || oxygen.value <= 0)
		{
			health.UpdateStat(-.1f);


		}
		else
		{
			health.UpdateStat(.1f);
		}
	}
}
