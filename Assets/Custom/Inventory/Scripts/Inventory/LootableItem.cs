using UnityEngine;
using UnityEngine.Events;


namespace StupidHumanGames
{
	public class LootableItem : MonoBehaviour
	{
		[SerializeField] Rigidbody rb;
		public bool lootable = true;
		[SerializeField] AudioSource impactSound;
		[SerializeField] AudioSource _treeFall;
		[SerializeField] bool isDamagable = false;
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
		    if(impactSound != null) impactSound?.Play();
			
			health -= amount;
			if(health <= 0)
			{
				death.Invoke();
				isDamagable = false;	
				if(_treeFall != null) _treeFall?.Play();
				if(rb!= null)
                {
					rb.useGravity = true;
					rb.isKinematic = false;
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
