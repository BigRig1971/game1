using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableManager : MonoBehaviour
{
	public List<GameObject> listOfGO = new List<GameObject>();
	public GameObject equipableItemHolder;
	public Transform[] equipableItems;
	[SerializeField] ScriptableObject[] itemSoList;
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
					if (!listOfGO.Contains(child.gameObject))
						listOfGO.Add(child.gameObject);
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
		foreach (GameObject go in listOfGO)
		{
			go.SetActive(false);
		}
	}
}
