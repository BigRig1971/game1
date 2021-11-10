using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EZInventory;

namespace EZInventory
{
    public class GridSlot : InventorySlot
    {
        public CraftingMenuGrid craftingMenu;

        bool onApplicationQuit;

        // Start is called before the first frame update
        void Awake()
        {
            includeInInventory = false;
        }

        protected override void MouseOverChecks()
        {
            //quick grab
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                InventoryManager.AddItemToInventory(currentItem, currentItemAmount);
            }
            else base.MouseOverChecks();

            //check recipes
            if (mouseOver && (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)))
            {
                craftingMenu.CheckRecipes();
            }
        }

        private void OnApplicationQuit()
        {
            onApplicationQuit = true;
        }

        private void OnDisable()
        {
            if (tooltipInstance)
                Destroy(tooltipInstance);

            //Drop item when craft menu is closed
            if (currentItem && !onApplicationQuit)
            {
                InventoryManager.DropItem(currentItem, currentItemAmount);
                currentItem = null;
                currentItemAmount = 0;
            }
        }
    }
}
