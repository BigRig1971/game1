using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableItem : MonoBehaviour
{
	[SerializeField]
	private InventoryChannel m_InventoryChannel;
	[SerializeField]
	private InventorySystem.InventoryItem[] m_LootableItems;

	private void OnTriggerEnter(Collider other)
	{

		if (other == null) return;
		foreach (InventorySystem.InventoryItem i in m_LootableItems)
		{			
			m_InventoryChannel?.RaiseLootItem(i);
		}
		

		Destroy(gameObject);

	}
}
