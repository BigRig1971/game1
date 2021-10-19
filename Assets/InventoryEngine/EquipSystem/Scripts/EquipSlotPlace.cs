using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipSlotPlace : MonoBehaviour
{
    EquipManager manager;

    void Awake()
    {
        manager = GameObject.Find("EquipSystem").GetComponent<EquipManager>(); 
        if(manager != null) manager.EquipSlots = gameObject;
        else Debug.LogError("[EquipSystem Error]: Can't find EquipSystem");
    }
}
