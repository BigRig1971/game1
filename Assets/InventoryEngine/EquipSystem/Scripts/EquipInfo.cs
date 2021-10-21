using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInfo : MonoBehaviour
{
    EquipManager manager;

    void Awake()
    {
        manager = GameObject.Find("EquipSystem").GetComponent<EquipManager>(); 
        if(manager != null) manager.EquipInfo = gameObject;
        else Debug.LogError("[EquipSystem Error]: Can't find EquipSystem");
    }
}
