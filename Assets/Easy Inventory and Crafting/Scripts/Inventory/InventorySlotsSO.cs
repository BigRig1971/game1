using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{


	[CreateAssetMenu(fileName = "New List", menuName = "EZ Inventory/ListOfSlots")]
	public class InventorySlotsSO : ScriptableObject
	{
		public List<InventorySlot> slots = new List<InventorySlot>();
	}
}

