using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{

	public class LootableItem : MonoBehaviour
	{
		public AudioSource impactSound;
		public bool damagableItem = false;
		private bool damagable = false;
		public int health = 30;
		Rigidbody rb;
		[System.Serializable]
		public class Item
		{
			public ItemSO _item;
			public int _itemAmount;
		}
		public Item[] _listOfItems;

		private void Start()
		{
			rb = transform.parent.GetComponent<Rigidbody>();
		}
		public void LootableItems()
		{
			foreach (Item loi in _listOfItems)
			{
				int remaining = InventoryManager.AddItemToInventory(loi._item, loi._itemAmount);

				if (remaining > 0)
				{
					loi._itemAmount = remaining;
				}
				else
				{
					if (!damagableItem)
					{
						Destroy(transform.parent.gameObject, .3f);
					}
				}
			}
		}
		public void TakeDamage(int amount)
		{
			impactSound?.Play();
			//rb.isKinematic = false;
			health -= amount;
			if(health <= 0)
			{
				Destroy(transform.parent.gameObject, .1f);
			}
		}
				
	}
}
