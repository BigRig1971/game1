using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{

	public class LootableItem : MonoBehaviour
	{
		[System.Serializable]
		public class Item
		{
			public ItemSO _item;
			public int _itemAmount;
		}
		public Item[] _listOfItems;
	
		public void LootableItems()
		{
			foreach (Item loi in _listOfItems)
			{
				int remaining = InventoryManager.AddItemToInventory(loi._item, loi._itemAmount);

				if (remaining > 0)
				{
					loi._itemAmount = remaining;
				}
			}
		}
	}
}
