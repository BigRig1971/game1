using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterInventory : MonoBehaviour
{
    [SerializeField] EZInventory.ItemSO starterItem;
    [SerializeField] int amount;

    void Awake()
    {
        EZInventory.InventoryManager.AddItemToInventory(starterItem, amount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
