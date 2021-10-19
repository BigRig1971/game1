using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSlotPlace : MonoBehaviour
{
    StoreManager manager;

    void Awake()
    {
        manager = GameObject.Find("StoreSystem").GetComponent<StoreManager>(); 
        if(manager != null) manager.StoreSlots = gameObject;
        else Debug.LogError("[StoreSystem Error]: Can't find StoreSystem");
    }
}
