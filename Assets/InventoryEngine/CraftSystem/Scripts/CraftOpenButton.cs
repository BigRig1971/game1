using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftOpenButton : MonoBehaviour
{
    CraftManager manager;

    void Awake()
    {
        manager = GameObject.Find("CraftSystem").GetComponent<CraftManager>(); 
        if(manager != null) {
            manager.CraftButton = gameObject;
            transform.GetComponent<Button>().onClick.AddListener(delegate { manager.OpenCloseCraft(); });
        }
        else Debug.LogError("[CraftSystem Error]: Can't find CraftSystem");
    }
}
