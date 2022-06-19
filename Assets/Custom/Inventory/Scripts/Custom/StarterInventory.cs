using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StupidHumanGames
{

    public class StarterInventory : MonoBehaviour
    {
        [System.Serializable]

        public class Item
        {
            public ItemSO _item;
            public int _itemAmount = 1;
        }
        public Item[] _listOfItems;
        void Start()
        {
            foreach(Item item in _listOfItems)
            {
                InventoryManager.AddItemToInventory(item._item, item._itemAmount);
            } 
        }
    }
}