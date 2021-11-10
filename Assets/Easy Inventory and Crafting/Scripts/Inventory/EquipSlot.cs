using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{
    public class EquipSlot : InventorySlot
    {
        public string equipParentName;
        public Sprite defaultSprite;

        Transform equipParent;
        GameObject equipInstance;

        // Start is called before the first frame update
        void Start()
        {
            equipParent = GameObject.Find(equipParentName).transform;
        }

        private void Update()
        {
            if (currentItemAmount <= 0)
            {
                currentItem = null;
            }

            SetUI();

            if (mouseOver)
            {
                MouseOverChecks();
            }

            Equip();
        }

        void Equip()
        {
            if (currentItem)
            {
                if (!equipParent)
                {
                    Debug.LogError("Was unable to find equipParent, check equipParentName is correct");
                }
                else if (!equipInstance)
                {
                    equipInstance = Instantiate(currentItem.equipPrefab, equipParent) as GameObject;
                }
            }
            else if (equipInstance)
            {
                Destroy(equipInstance);
            }
        }

        protected override void SetUI()
        {
            base.SetUI();
            if (!currentItem)
            {
                itemImage.sprite = defaultSprite;
                itemImage.color = Color.white;
            }
        }
    }
}