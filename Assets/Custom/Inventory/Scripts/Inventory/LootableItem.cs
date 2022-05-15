using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{

	public class LootableItem : MonoBehaviour
	{
		public Vector3 colliderSize = Vector3.one;
		public Vector3 colliderCenter = Vector3.zero;
		public bool isTree = false;	
		
		public bool lootable = true;
		public AudioSource impactSound;
		public AudioSource _treeFall;
		public bool isDamagable = false;
		public int health = 30;
	
		Rigidbody rb;
		[System.Serializable]
		public class Item
		{
			public ItemSO _item;
			public int _itemAmount = 1;
		}
		public Item[] _listOfItems;

        private void Start()
		{
			if (TryGetComponent<Rigidbody>(out rb))
			{
				rb = GetComponent<Rigidbody>();
			}
			else
			{
				rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
				rb.isKinematic = true;
			}			
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>() as BoxCollider;
			boxCollider.size = colliderSize;
			boxCollider.center = (Vector3.zero + colliderCenter);
			boxCollider.isTrigger = true;
		}
		public void LootableItems()
		{
			if (!lootable) return;
            
			foreach (Item loi in _listOfItems) 
			{
				int remaining = InventoryManager.AddItemToInventory(loi._item, loi._itemAmount);

				if (remaining > 0)
				{
					loi._itemAmount = remaining;
				}
				else
				{
					if (!isDamagable)
					{						
						Destroy(transform.gameObject, .3f);
					}
                    
				}
			}
		}
		public void TakeDamage(int amount)
		{
			if (!isDamagable) return;
			lootable = false;
			impactSound?.Play();
			health -= amount;
			if(health <= 0)
			{
				
				isDamagable = false;	
				_treeFall?.Play();
				rb.useGravity = true;
				rb.isKinematic = false;
				Invoke(nameof(DestroyItem), 3f);
			}
		}
		
		void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
		}
		void DestroyItem()
        {
			lootable = true;
			LootableItems();
			Destroy(transform.gameObject);
		}
	}

}
