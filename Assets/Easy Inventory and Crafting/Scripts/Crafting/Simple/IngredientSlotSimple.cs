using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{
    public class IngredientSlotSimple : InventorySlot
    {
        public bool ingredientsAvailable;

        // Start is called before the first frame update
        void Awake()
        {
            interactable = false;
            includeInInventory = false;
        }

        protected override void SetUI()
        {
            base.SetUI();
            if (ingredientsAvailable)
            {
                borderImage.color = currentItem.itemBorderColor / 2f;
                stackImage.color = currentItem.itemBorderColor / 2f;
                itemImage.color = Color.grey;
            }
            else
            {
                borderImage.color = Color.black;
                stackImage.color = Color.black;
                itemImage.color = Color.black;
            }
        }
    }
}
