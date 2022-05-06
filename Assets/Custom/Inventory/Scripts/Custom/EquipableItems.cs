using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItems : MonoBehaviour
{
    public GameObject[] _equipableItems;
    public GameObject _equipItemGo;
   
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject g in _equipableItems)
		{
            g.SetActive(false);
            
		}

    }
    public void UpdateEquipableItems(GameObject Go)
	{
        foreach(GameObject g in _equipableItems)
		{
            if(Go.name == g.name)
			{
                Debug.Log("success");
            }
            
		}
	}
    
}
