using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotPlace : MonoBehaviour
{
    InventoryManager manager;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) manager.InventorySlots = gameObject;
        else Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");
    }
}
