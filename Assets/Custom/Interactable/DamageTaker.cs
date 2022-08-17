using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTaker : MonoBehaviour, IGenericInteractable, IDamage
{
	[SerializeField] private float health = 100;
	[SerializeField] private float armor = 5f;
	public float Health { get { return health; } set { health = value; } }
	public float Armor { get { return armor; } set { armor = value; } }

	public void Damage(float amount)
	{

		health -= (amount * armor);
		Debug.Log(health);
		if (health <= 0)
		{
			Debug.Log("dead");
			Destroy(gameObject);
		}
	}

	public void Interact()
	{
		//gameObject.GetComponent<Renderer>().material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

	}
}
