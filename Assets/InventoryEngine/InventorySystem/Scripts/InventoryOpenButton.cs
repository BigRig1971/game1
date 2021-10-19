using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOpenButton : MonoBehaviour
{
    InventoryManager manager;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) {
            manager.InventoryButton = gameObject;
            transform.GetComponent<Button>().onClick.AddListener(delegate { manager.OpenCloseInventory(); });
        }
        else Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");
    }
}
