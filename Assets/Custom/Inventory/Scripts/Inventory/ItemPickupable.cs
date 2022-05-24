using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StupidHumanGames
{
	/// <summary>
	/// Adds items to inventory on collision with Player
	/// </summary>
	public class ItemPickupable : MonoBehaviour
	{
		ItemSO itemSO;
		int itemAmount;

		static Transform camTransform;

		private void Start()
		{
			if (!camTransform)
				camTransform = Camera.main.transform;
		}

		private void Update()
		{
			//  transform.forward = -camTransform.forward;
		}

		public void SetUpPickupable(ItemSO item, int amount)
		{
			itemSO = item;
			itemAmount = amount;

			// GetComponent<SpriteRenderer>().sprite = item.itemSprite;

		}
		public void LootableItems()
		{
			int remaining = InventoryManager.AddItemToInventory(itemSO, itemAmount);

			if (remaining > 0)
			{
				itemAmount = remaining;
			}
            else
            {
				StartCoroutine(DestroyItem());
            }
			
		}
		IEnumerator DestroyItem()
        {
			yield return new WaitForSeconds(.3f);
			Destroy(gameObject);
		}
	}
}
