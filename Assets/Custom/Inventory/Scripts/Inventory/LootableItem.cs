using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{

	public class LootableItem : MonoBehaviour
	{
		public Rigidbody rb;
		public bool isTree = false;
		public bool addRigidBody = false;
		public bool lootable = true;
		public AudioSource impactSound;
		public AudioSource _treeFall;
		public bool isDamagable = false;
		public int health = 30;
		
	
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
		}
#if UNITY_EDITOR
		
#endif
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
				Invoke(nameof(DestroyItem), 5f);
			}
		}
		
		void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix;
			//Gizmos.(Vector3.zero + colliderCenter, colliderSize);
		}
		void DestroyItem()
        {
			lootable = true;
			LootableItems();
			Destroy(transform.gameObject);
		}
	}

}
