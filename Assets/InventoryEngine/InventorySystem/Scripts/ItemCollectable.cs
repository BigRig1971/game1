using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectable : MonoBehaviour
{
    public int ItemID;

    private InventoryManager manager;

    void Awake() {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
    }

    //add item to inventory or Equip panel when we collect the prefab with this script
    void OnTriggerEnter(Collider other)
    {
        ItemCollector collector = other.transform.GetComponent<ItemCollector>();
        if (collector != null)
        {
            List<string> types = new List<string>(new string[]{"Inventory", "Equip"});

            manager.AddNewItem(ItemID, types: types);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
