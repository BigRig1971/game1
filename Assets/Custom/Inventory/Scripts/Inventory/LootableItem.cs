using UnityEngine;
using UnityEngine.Events;


namespace StupidHumanGames
{
	public class LootableItem : MonoBehaviour
	{
		public Rigidbody rb;
		public bool lootable = true;
		public AudioSource impactSound;
		public AudioSource _treeFall;
		public bool isDamagable = false;
		public int health = 30;
		public UnityEvent death;

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
			lootable = false;
			impactSound?.Play();
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
			isDamagable = false;
			LootableItems();
			
		}
		
	}
	
}
