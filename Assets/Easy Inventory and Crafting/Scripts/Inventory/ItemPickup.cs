using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{

    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] ItemSO[] _items;
        ItemSO itemSO;
        int itemAmount;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                foreach(ItemSO _item in _items)
				{
                    int remaining = InventoryManager.AddItemToInventory(_item, 1);

                    if (remaining > 0)
                    {
                        itemAmount = remaining;
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
               
            }
        }
    }
}
