using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StupidHumanGames;

public class StarterInventory : MonoBehaviour
{
    [SerializeField] ItemSO starterItem;
    [SerializeField] int amount;

    void Awake()
    {
        StupidHumanGames.InventoryManager.AddItemToInventory(starterItem, amount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
