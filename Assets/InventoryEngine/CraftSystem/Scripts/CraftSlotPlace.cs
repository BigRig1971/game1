using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSlotPlace : MonoBehaviour
{
    CraftManager manager;

    void Awake()
    {
        manager = GameObject.Find("CraftSystem").GetComponent<CraftManager>(); 
        if(manager != null) manager.CraftSlots = gameObject;
        else Debug.LogError("[CraftSystem Error]: Can't find CraftSystem");
    }
}
