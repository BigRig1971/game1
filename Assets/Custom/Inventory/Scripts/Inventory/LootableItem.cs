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
		
		
		public bool readyToLoot = false;
		public AudioSource impactSound;
		public AudioSource _treeFall;
		public bool isDamagable = false;
		public int health = 30;
	
		Rigidbody rb;
		[System.Serializable]
		public class Item
		{
			public ItemSO _item;
			public int _itemAmount;
		}
		public Item[] _listOfItems;

        private void Awake()
        {
			
		}
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
						//readyToLoot = true;	
						Destroy(transform.gameObject, .3f);
					}
                    
				}
			}
		}
		public void TakeDamage(int amount)
		{
			//readyToLoot = false;	
			impactSound?.Play();
			health -= amount;
			if(health <= 0 && !readyToLoot)
			{
				StartCoroutine(TreeFall());
			}
		}
		private IEnumerator TreeFall()
        {
			readyToLoot = true;
			_treeFall?.Play();
			rb.useGravity = true;
			rb.isKinematic = false;
			yield return new WaitForSeconds(5);
			Destroy(transform.gameObject, .1f);
		}
		void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
		}
	}
}
