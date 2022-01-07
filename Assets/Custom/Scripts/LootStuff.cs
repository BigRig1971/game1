using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
public class LootStuff : MonoBehaviour
{
	Animator anim;
	LootableItem itemPickup;
	ItemPickupable droppedItemPickup;
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
			itemPickup = other.gameObject.GetComponent<LootableItem>();
			droppedItemPickup = other.gameObject.GetComponent<ItemPickupable>();
			StartCoroutine(LootItem());
		}
	}
	private IEnumerator LootItem()
	{
		(gameObject.GetComponent(element) as MonoBehaviour).enabled = false;
		if(itemPickup!= null) itemPickup.LootableItems();
		if (droppedItemPickup != null) droppedItemPickup.LootableItems();
		anim.SetBool("Pickup", true);
		
		yield return new WaitForSeconds(.3f);

		(gameObject.GetComponent(element) as MonoBehaviour).enabled = true;
		
		anim.SetBool("Pickup", false);
		Destroy(otherGO);
	}
}
