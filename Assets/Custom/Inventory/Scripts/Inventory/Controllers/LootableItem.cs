using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableItem : MonoBehaviour
{
	[SerializeField]
	private InventoryChannel m_InventoryChannel;
	[SerializeField]
	private InventorySystem.InventoryItem m_LootableItem;

	private void OnTriggerEnter(Collider other)
	{
		if (other == null) return;

		m_InventoryChannel?.RaiseLootItem(m_LootableItem);

		Destroy(gameObject);

	}
}
