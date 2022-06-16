using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
	float Armor { get; set; }
	float Health { get; set; }
	void Damage(float amount);
}
