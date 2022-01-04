using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
public class LootStuff : MonoBehaviour
{
	Animator anim;
	ItemPickup itemPickup;
	public string element;
	GameObject otherGO;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Lootable")
		{
			otherGO = other.gameObject;
			itemPickup = other.gameObject.GetComponent<ItemPickup>();
			StartCoroutine(LootItem());
		}
	}
	private IEnumerator LootItem()
	{
		(gameObject.GetComponent(element) as MonoBehaviour).enabled = false;
		itemPickup.LootableItems();
		anim.SetBool("Pickup", true);
		
		yield return new WaitForSeconds(.3f);

		(gameObject.GetComponent(element) as MonoBehaviour).enabled = true;
		
		anim.SetBool("Pickup", false);
		Destroy(otherGO);
	}
}
