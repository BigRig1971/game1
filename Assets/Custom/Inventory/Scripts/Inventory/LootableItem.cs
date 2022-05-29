using UnityEngine;
using UnityEngine.Events;
using System.Collections;


namespace StupidHumanGames
{
	public class LootableItem : MonoBehaviour
	{
		[SerializeField] bool canFall = false;
		private bool lootable;		
		[SerializeField] AudioClip _impactSound;
		[SerializeField, Range(0f, 1f)] float _impactVolume;
		[SerializeField] AudioClip _deathSound;
		[SerializeField, Range(0f, 1f)] float _deathVolume;
		
		public bool isDamagable = false;
		[SerializeField] int health = 30;
		[SerializeField] float deathDelay = 5f;
		public UnityEvent death;
		public UnityEvent takeHit;
		[System.Serializable]
		public class Item
		{
			public ItemSO _item;
			public int _itemAmount = 1;
		}
		public Item[] _listOfItems;
		private void Awake()
		{
			if (isDamagable) lootable = false; else lootable = true;
		}
		private void Start()
		{
			if (death == null)
				death = new UnityEvent();
			if(takeHit == null)
				takeHit = new UnityEvent();
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
					 if(transform.parent != null) Destroy(transform.parent.gameObject, .3f); 
						Destroy(transform.gameObject, .3f);
						
					}
                    
				}
			}
		}
      
        public void TakeDamage(int amount)
		{
			if (!isDamagable) return;
			takeHit.Invoke();
			lootable = false;
		    if(_impactSound != null) AudioSource.PlayClipAtPoint(_impactSound, transform.position, _impactVolume);

			health -= amount;
			if(health <= 0)
			{
				death.Invoke();
				isDamagable = false;	
				if(_deathSound != null) AudioSource.PlayClipAtPoint(_deathSound, transform.position, _deathVolume);
                if (canFall)
                {
					Rigidbody rBody = gameObject.AddComponent<Rigidbody>();
					rBody.useGravity = true;
					rBody.isKinematic = false;
				}		
				Invoke(nameof(LootItem), deathDelay);
			}
		}
		
		void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;
			Gizmos.matrix = transform.localToWorldMatrix;
			//Gizmos.(Vector3.zero + colliderCenter, colliderSize);
		}
		void LootItem()
        {
			
			lootable = true;
			isDamagable = false;
			LootableItems();
			
		}
		
	}
	
}
