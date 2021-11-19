using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;


namespace EZInventory
{
	public class EquipSlot : InventorySlot
	{
		public EquipableManager equipManager;
		public string equipParentName;
		public Sprite defaultSprite;
		Transform equipParent;
		GameObject equipInstance;
		public GameObject previousGO = null;
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
				//if (previousGO != null) previousGO.SetActive(false);
				foreach (GameObject go in equipManager.listOfGO)
				{

					if (go.name == currentItem.name)
					{
						go.SetActive(true);
						previousGO = go;
						
					}
					else
					{
						if(currentItem.type == this.type)
						{
							go.SetActive(false);
						}
					}
					
				}	
			}
			else
			{
				
				
				
				
				if (previousGO != null) previousGO.SetActive(false);
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