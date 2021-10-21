using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    InventoryManager manager;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        manager.Character = gameObject;
    }
}
