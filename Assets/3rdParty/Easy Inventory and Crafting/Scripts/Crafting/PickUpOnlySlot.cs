using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using EZInventory;

namespace EZInventory
{
    public class PickUpOnlySlot : InventorySlot
    {
        public UnityEvent onGrab;
        // Start is called before the first frame update
        void Awake()
        {
            includeInInventory = false;
        }

        protected override void MouseOverChecks()
        {
            if (currentItem == null)
                return;
            if (!interactable) return;

            if (Input.GetMouseButtonDown(0))
            {
                //Quick grab
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    InventoryManager.AddItemToInventory(currentItem, currentItemAmount);
                }
                else
                {
                    InventoryManager.GrabItemFromSlot(this, currentItemAmount);
                }
                SetItemInSlot(currentItem, currentItemAmount);
                onGrab.Invoke();
            }
        }
    }
}