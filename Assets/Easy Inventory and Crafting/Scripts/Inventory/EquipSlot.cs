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
        public GameObject[] _equipableGo;
        
		private void Awake()
		{
            foreach (GameObject go in _equipableGo)
            {
                go.SetActive(false);
            }
        }
		void Start()
        {
            
          //  equipParent = GameObject.Find(equipParentName).transform;
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
                foreach (GameObject go in _equipableGo)
                {
                    if (go.name == currentItem.name)
                    {
                        go.SetActive(true);
                    }
                }
            }
			else
			{
                foreach(GameObject go in _equipableGo)
				{
                    go.SetActive(false);
				}
			}
           /* if (currentItem)
            {
                //Debug.Log(currentItem);
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
            }*/
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