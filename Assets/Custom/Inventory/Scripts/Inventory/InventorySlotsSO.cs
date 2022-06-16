using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StupidHumanGames
{


	[CreateAssetMenu(fileName = "New List", menuName = "Inventory/ListOfSlots")]
	public class InventorySlotsSO : ScriptableObject
	{
		public List<InventorySlot> slots = new List<InventorySlot>();
	}
}

