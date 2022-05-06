using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

public class MobileCrafting : MonoBehaviour
{
    public GameObject craftingMenuCanvas;
    void Start()
    {
        craftingMenuCanvas.SetActive(InventoryManager.IsOpen());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
