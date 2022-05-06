using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{
    /// <summary>
    /// All this does is open the crafting menu if the inventory is open and the player is nearby
    /// </summary>
    public class CraftingTable : MonoBehaviour
    {
        public GameObject craftingMenuCanvas;
        float radius = 5;
        Transform player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {

            craftingMenuCanvas.SetActive(InventoryManager.IsOpen() && Vector3.Distance(player.position, transform.position) < radius);
            
        }
    }
}