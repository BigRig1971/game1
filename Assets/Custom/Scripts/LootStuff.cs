using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
public class LootStuff : MonoBehaviour
{
	Animator anim;
	LootableItem itemPickup;
	ItemPickupable droppedItemPickup;

	public string scriptToPause;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Lootable"))
		{
			Debug.Log("lootstuff");
			itemPickup = other.gameObject.GetComponent<LootableItem>();
			droppedItemPickup = other.gameObject.GetComponent<ItemPickupable>();
			if(itemPickup.readyToLoot) StartCoroutine(LootItem());
		}
		
	}
	private IEnumerator LootItem()
	{
		(gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = false;
		if (itemPickup != null) itemPickup.LootableItems();
		if (droppedItemPickup != null) droppedItemPickup.LootableItems();
		anim.SetBool("Pickup", true);

		yield return new WaitForSeconds(.3f);

		(gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = true;

		anim.SetBool("Pickup", false);
		
	}
	
}
