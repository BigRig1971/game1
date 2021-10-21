using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    //GameObject initiated by scripts
    public GameObject StoreSlots;
    public GameObject StoreInfo;

    InventoryManager manager;

    StoreData openedStoreData;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) manager.Store = gameObject;
        else Debug.LogError("[StoreSystem Error]: Can't find InventorySystem");
    }

    void Start() {
        CloseStore();
    }

    //close storage panel
    public void CloseStore()
    {
        if(openedStoreData!= null) openedStoreData.SaveStore(StoreSlots);
        if(manager != null) manager.ClearSystemSlots(StoreSlots, "Store");
        else Debug.LogError("[StoreSystem Error]: Can't find InventorySystem");

        gameObject.SetActive(false);
    }

    //open storage panel
    public void OpenStore(StoreData data)
    {
        openedStoreData = data;
        if(data != null){
            if(manager != null) manager.CreateSystemSlots(StoreSlots, "Store", data.slotsAmount);//create slots for storage when it is opened
            else Debug.LogError("[StoreSystem Error]: Can't find InventorySystem");
            
            if(openedStoreData!= null) openedStoreData.LoadStore();
            else Debug.LogError("[StoreSystem Error]: openedStoreData == null");
            
            SetStoreInfo(data);
            gameObject.SetActive(true);
        }else Debug.LogError("[StoreSystem Error]: StoreData == null");
    }

    //set text in storage info panel 
    public void SetStoreInfo(StoreData data) {
        string text = "<color=#00008B><b>" + data.title + "</b></color>" + "\n<color=#FFFFFF>" + data.description + "</color>";
        StoreInfo.transform.Find("Text").GetComponent<Text>().text = text;
    }
}
