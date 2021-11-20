using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableManager : MonoBehaviour
{
	
	[SerializeField]
	private GameObject equipableItemHolder;
	public List<GameObject> EquipableItems = new List<GameObject>();
	private Transform[] equipableItems;
	private ScriptableObject[] itemSoList;
	private void Awake()
	{
		itemSoList = Resources.LoadAll<ScriptableObject>("EquipableItems");
		equipableItems = equipableItemHolder.GetComponentsInChildren<Transform>();
		foreach (Transform child in equipableItems)
		{
			foreach (ScriptableObject iso in itemSoList)
			{
				if (child.name == iso.name)
				{
					if (!EquipableItems.Contains(child.gameObject))
						EquipableItems.Add(child.gameObject);
				}
			}
		}
	}
	private void Start()
	{
		DisableEquipped();
	}
	void DisableEquipped()
	{
		foreach (GameObject go in EquipableItems)
		{
			go.SetActive(false);
		
		}
	}
}
