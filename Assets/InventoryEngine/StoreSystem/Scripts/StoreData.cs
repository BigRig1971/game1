using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class StoreData : MonoBehaviour
{
    public string id;
    public string title;
    public string description;
    public int slotsAmount;
    public List<StoreItem> items;

    InventoryManager manager;
    StoreManager storeManager;

    void Start () {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
        if(manager == null) Debug.LogError("[StoreSystem Error]: Can't find InventorySystem");
        else{
            if(manager.Store == null) Debug.LogError("[StoreSystem Error]: Can't find StoreManager");
            else{
                storeManager = manager.Store.GetComponent<StoreManager>();
                if(storeManager == null) Debug.LogError("[StoreSystem Error]: Can't find StoreManager");
            }
        }
    }

    //Open storage with items when player enetred storage
    void OnTriggerEnter(Collider other)
    {
        ItemCollector collector = other.transform.GetComponent<ItemCollector>();
        //check if player (gameObject with script ItemCollector.cs) enter the storage
        if (collector != null)
        {
            if(manager != null){
                storeManager.OpenStore(this);//open storage panel with items (if this storage has them) or without (if this storage is empty)
                manager.OpenInventory();//also open player inventory
            } else Debug.LogError("[StoreSystem Error]: Can't find InventorySystem");
        }
    }

    //Close storage with items when player exited storage
    void OnTriggerExit(Collider other)
    {
        ItemCollector collector = other.transform.GetComponent<ItemCollector>();
        //check if player (gameObject with script ItemCollector.cs) enter the storage
        if (collector != null)
        {
            if(manager != null){
                storeManager.CloseStore();//close storage panel and save items in it
                manager.CloseInventory();//also close player inventory
            }else Debug.LogError("[StoreSystem Error]: Can't find InventorySystem");
        }
    }


    //save items in storage
    public void SaveStore (GameObject StoreSlots) {
        if(StoreSlots != null){
            int amount = StoreSlots.transform.childCount;
            if(amount > 0){
                List<StoreItem> newItems = new List<StoreItem>();
                //checking all storage slots
                for (int i = 0; i < StoreSlots.transform.childCount; i++){
                    ItemData data = StoreSlots.transform.GetChild(i).GetComponent<SlotData>().GetItemData();
                    //if slot has item
                    if(data != null) {
                        StoreItem storeItem = new StoreItem(data.slotID, data.amount, data.item.ID);
                        newItems.Add(storeItem);//we get info about it and save in "items" variable
                    }
                }

                items = newItems;
                SaveStoreInPrefs(newItems);
            }
        }
    }

    //fill opened storage with saved items
    public void LoadStore () {
        List<StoreItem> newItems = LoadStoreFromPrefs();
        if(newItems != null){
            if(manager != null){
                List<string> types = new List<string>(new string[]{"Store"});//add items only to storage slots

                for (int i = 0; i < newItems.Count; i++){
                    var itemData = newItems[i];

                    if(itemData.SlotID == -1) manager.AddNewItem(itemData.ItemID, amount: itemData.Amount, types: types);
                    else if(itemData.SlotID >= manager.SlotsAmount) for(int j =0; j < itemData.Amount; j++) manager.AddNewItemToSlot(itemData.ItemID, itemData.SlotID);
                }
            }else Debug.LogError("[StoreSystem Error]: Can't find InventorySystem");
        }
    }

    //Save storage data in player prefs
    void SaveStoreInPrefs(List<StoreItem> items){
        string name = "Store_" + id;
        string json = JsonMapper.ToJson(items);
        PlayerPrefs.SetString(name, json);
    }

    //Load storage data in player prefs
    List<StoreItem> LoadStoreFromPrefs() {
        List<StoreItem> result = new List<StoreItem>();

        string name = "Store_" + id;
        JsonData data = null;
        if(PlayerPrefs.HasKey(name)) data = JsonMapper.ToObject(PlayerPrefs.GetString(name));

        if(data != null){
            for (int i = 0; i < data.Count; i++){
                result.Add(new StoreItem((int)data[i]["SlotID"], (int)data[i]["Amount"], (int)data[i]["ItemID"]));
            }
        }

        return  result;
    }
}

[System.Serializable] 
public class StoreItem {
    public int SlotID;
    public int ItemID;
    public int Amount;

    public StoreItem(int slotID, int amount, int itemID) {
        this.SlotID = slotID;
        this.Amount = amount;
        this.ItemID = itemID;
    }
}
