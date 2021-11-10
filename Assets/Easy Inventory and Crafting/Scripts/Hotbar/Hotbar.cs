using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{
    public class Hotbar : MonoBehaviour
    {
        [SerializeField]
        private string equipParentName;

        Transform equipParent;
        InventorySlot[] slots;
        Transform currentEquip;
        ItemSO lastItem;

        int currentSlotIndex = 0;

        // Start is called before the first frame update
        void Start()
        {
            equipParent = GameObject.Find(equipParentName).transform;
            slots = GetComponentsInChildren<InventorySlot>();
        }

        // Update is called once per frame
        void Update()
        {
            //Scale up currently selected slot
            for (int i = 0; i < slots.Length; i++)
            {
                if (i == currentSlotIndex)
                    slots[i].transform.localScale = Vector3.one * 1.2f;
                else
                    slots[i].transform.localScale = Vector3.one;
            }

            //Scroll to swap slots
            if (Input.mouseScrollDelta.y < 0)
            {
                if (currentSlotIndex >= slots.Length - 1)
                    currentSlotIndex = 0;
                else
                    currentSlotIndex++;
            }
            else if (Input.mouseScrollDelta.y > 0)
            {
                if (currentSlotIndex <= 0)
                    currentSlotIndex = slots.Length - 1;
                else
                    currentSlotIndex--;

            }

            //On item change (not called when swapping between slots containing same item)
            if (lastItem != slots[currentSlotIndex].currentItem)
            {
                lastItem = slots[currentSlotIndex].currentItem;

                //Destroy previously equipped item
                if (currentEquip)
                {
                    Destroy(currentEquip.gameObject);
                }
                //Instantiate equip item if currentItem type = Hand
                if (slots[currentSlotIndex].currentItem && slots[currentSlotIndex].currentItem.type == ItemSO.Type.Hand)
                {
                    currentEquip = Instantiate(slots[currentSlotIndex].currentItem.equipPrefab, equipParent).transform;
                    currentEquip.localPosition = Vector3.zero;
                }
            }
        }
    }
}