using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{

	public class LootableItem : MonoBehaviour
	{
		
		public bool isTree = false;
		public bool addRigidBody = false;
		public bool lootable = true;
		public AudioSource impactSound;
		public AudioSource _treeFall;
		public bool isDamagable = false;
		public int health = 30;
		CapsuleCollider collider;
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
			else if(addRigidBody)
			{
				rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
				rb.isKinematic = false;
				rb.useGravity = false;
				rb.freezeRotation = true;	
			}	
			if(TryGetComponent<CapsuleCollider>(out collider))
            {
				collider = GetComponent<CapsuleCollider>();
            }
            else
            {
				collider = gameObject.AddComponent<CapsuleCollider>() as CapsuleCollider;
				
			}					
		}
#if UNITY_EDITOR
		private void OnValidate()
    {
			if (TryGetComponent<CapsuleCollider>(out collider))
			{
				collider = GetComponent<CapsuleCollider>();
			}
			else
			{
				collider = gameObject.AddComponent<CapsuleCollider>() as CapsuleCollider;
				
			}
		}
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
				Invoke(nameof(DestroyItem), 3f);
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
