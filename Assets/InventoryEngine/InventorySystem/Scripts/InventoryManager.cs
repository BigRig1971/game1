using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;

public class InventoryManager : MonoBehaviour
{
    public List<GameObject> SlotList;//all player slots from all systems are stored in this variable. So you can easily manipulate them

    public int SlotsAmount;//only inventory slots amount

    //GameObject initiated by scripts
    public GameObject InventorySlots;
    public GameObject InventoryButton;
    public GameObject Store;
    public GameObject Equip;
    public GameObject Craft;
    public GameObject ItemThrow;
    public GameObject TipPanel;
    public GameObject SplitPanel;
    public GameObject Character;

    //Private variables
    public InventoryDatabase database;
    public GameObject slot;
    public GameObject item;

    void Awake () {
        database = transform.GetComponent<InventoryDatabase>();
        slot = Resources.Load<GameObject>("Prefabs/Slot");
        item = Resources.Load<GameObject>("Prefabs/Item");
    }

    void Start () {
        CreateSystemSlots(InventorySlots, "Inventory", SlotsAmount);
        CreateSystemSlots(Equip.GetComponent<EquipManager>().EquipSlots, "Equip", Equip.GetComponent<EquipManager>().SlotsAmount);

        LoadInventoryFromPrefs();
        OpenCloseInventory();
    }

    //search and return slot by ID
    public GameObject GetSlotByID (int id) {
        int amount = GetSlotsAmount();

        foreach(var slot in SlotList){
            SlotData slotData = slot.GetComponent<SlotData>();
            if(slotData.ID == id) return slot;
        }

        Debug.LogWarning("[InventorySystem Warning]: Can't find slot with ID = " + id.ToString());
        return null;
    }

    //Return inventory slots amount
    public int GetSlotsAmount () {
        return SlotList.Count;
    }

    //check if inventory has one item with specified id
    public bool HasItem (int id){
        return HasAmountItem(id) == 0;
    }

    //check if inventory has items with specified id, amount and slot types
    public int HasAmountItem (int id, int amount = 1, List<string> types = null){
        int result = amount;

        if(types == null) types = new List<string>(new string[]{"Inventory"});//which slots can be used

        //searching slot
        foreach (var slot in SlotList){
            SlotData slotData = slot.GetComponent<SlotData>();

            //Can remove item only from inventory slots
            ItemData itemData = slotData.GetItemData();
            if(types.Contains(slotData.Type) && slotData.HasItem() && itemData.item.ID == id){
                result -= itemData.amount;

                if(result <= 0){
                    result = 0;
                    break;
                }
            }
        }

        return result;
    }

    //remove item with specified id from slots with specified types
    public void RemoveItem(int id, List<string> types = null){
        if(types == null) types = new List<string>(new string[]{"Inventory"});//which slots can be used

        //searching slot
        foreach (var slot in SlotList){
            SlotData slotData = slot.GetComponent<SlotData>();

            //Can remove item only from inventory slots
            ItemData itemData = slotData.GetItemData();
            if(types.Contains(slotData.Type) && slotData.HasItem() && itemData.item.ID == id){
                if (itemData.amount > 1) itemData.amount -= 1;
                else slotData.Clear();
                break;
            }
        }
    }

    //searching empty slot in inventory and fill it with a new item
    public void AddNewItem(int id, bool stackable = true, int amount = 1, List<string> types = null)
    {
        Item itemToAdd = database.FindItemByID(id);
        SplitPanel.GetComponent<InventoryItemSplit>().Deactivate();

        if(types == null) types = new List<string>(new string[]{"Inventory","Equip"});//which slots can be used

        if(itemToAdd != null){
            bool foundEmpty = false;

            //searching slot
            foreach (var slot in SlotList)
            {
                SlotData slotData = slot.GetComponent<SlotData>();
                //Can add item to inventory or to Store or to both
                if(types.Contains(slotData.Type)){
                    if (!slotData.HasItem())
                    {
                        //if empty slot is found
                        GameObject itemObj = Instantiate(item, slot.transform);

                        itemObj.GetComponent<ItemData>().item = itemToAdd;
                        itemObj.GetComponent<ItemData>().slotID = slotData.ID;
                        itemObj.GetComponent<ItemData>().amount = itemToAdd.Stackable?amount:1;
                        itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                        foundEmpty = true;
                        if(!itemToAdd.Stackable && amount > 1) AddNewItem(id, stackable, amount - 1, types); //add more items to another slots if they aren't stackable
                        break;
                    }else if (stackable && slotData.GetItemData().item.ID == id && slotData.GetItemData().item.Stackable){
                        //if can stack items and they are stackable and their IDs are equal
                        slotData.GetItemData().amount += amount;
                        foundEmpty = true;
                        break;
                    }
                }
            }
            
            //if empty slot isn't found, item will be thrown away
            if(!foundEmpty) ItemThrow.GetComponent<InventoryItemThrow>().ThrowItemAway(itemToAdd, Character.transform, amount);
        }
    }

    //add item to the chosen slot
    public void AddNewItemToSlot(int itemID, int slotID, int amount = 1)
    {
        Item itemToAdd = database.FindItemByID(itemID);
        SplitPanel.GetComponent<InventoryItemSplit>().Deactivate();

        if(itemToAdd != null){
            // int amount = GetSlotsAmount();

            GameObject slot = GetSlotByID(slotID);
            if(slot != null){
                SlotData slotData = slot.GetComponent<SlotData>();

                if(!slotData.HasItem() || slotData.GetItemData().item.ID != itemID){
                    slotData.Clear();

                    GameObject itemObj = Instantiate(item, slot.transform);

                    itemObj.GetComponent<ItemData>().item = itemToAdd;
                    itemObj.GetComponent<ItemData>().slotID = slotData.ID;
                    itemObj.GetComponent<ItemData>().amount = amount;
                    itemObj.GetComponent<Image>().sprite = itemToAdd.Sprite;
                }else if(slotData.GetItemData().item.Stackable) {
                    slotData.GetItemData().amount += 1;
                }
            }
        }
    }

    //change slot of chosen item
    public void ChangeItemSlot(int fromSlotID, int toSlotID) {
        Transform fromSlot = GetSlotByID(fromSlotID).transform;
        Transform toSlot = GetSlotByID(toSlotID).transform;

        if(fromSlot != null && toSlot != null) {
            Transform slotItem = fromSlot.GetChild(0);
            
            if(slotItem != null){
                slotItem.SetParent(toSlot);
                slotItem.position = toSlot.position;
                slotItem.GetComponent<ItemData>().slotID = toSlotID;
            }
        }
    }

    //open or close inventory
    public void OpenCloseInventory()
    {
        if(InventorySlots.activeSelf) CloseInventory();
        else OpenInventory();
    }

    //open inventory
    public void OpenInventory() {
        InventorySlots.SetActive(true);
        SplitPanel.GetComponent<InventoryItemSplit>().Deactivate();
        SplitPanel.SetActive(true);
    }

    //close inventory
    public void CloseInventory() {
        SaveInventoryInPrefs();

        InventorySlots.SetActive(false);
        SplitPanel.GetComponent<InventoryItemSplit>().Deactivate();
        SplitPanel.SetActive(false);
    }

    //Add one slot to inventory with certain type
    public void AddNewSlot (GameObject systemSlots, string type) {
        GameObject slot = this.slot?this.slot:Resources.Load<GameObject>("Prefabs/Slot");
        
        GameObject newSlot = Instantiate(slot, systemSlots.transform);
        SlotList.Add(newSlot);
        int slotID = SlotList.IndexOf(newSlot);
        newSlot.GetComponent<SlotData>().ID = slotID;
        newSlot.GetComponent<SlotData>().Type = type;
    }

    //Add slots of certain type to Inventory
    public void AddSystemSlots (GameObject systemSlots, string type, int amount) {
        for(int i = 0; i < amount; i++){
            AddNewSlot(systemSlots, type);
        }
    }

    //Remove all childs from Inventory
    public void ClearSystemSlots (GameObject systemSlots, string type) {
        int amount = systemSlots.transform.childCount;

        for (int i = amount - 1; i >= 0; i--){
            Destroy(systemSlots.transform.GetChild(i).gameObject);
        }

        for (int i = SlotList.Count - 1; i >= 0; i--){
            if(SlotList[i].GetComponent<SlotData>().Type == type){
                SlotList.RemoveAt(i);
            }
        }
    }

    //Fill inventory with the new slots
    public void CreateSystemSlots (GameObject systemSlots, string type, int amount) {
        if(SlotList == null) SlotList = new List<GameObject>();
        ClearSystemSlots(systemSlots, type);
        AddSystemSlots(systemSlots, type, amount);
    }

    //Save inventory data in player prefs
    void SaveInventoryInPrefs(){
        if(SlotList != null){
            List<StoreItem> items = new List<StoreItem>();

            foreach(var slot in SlotList){
                ItemData data = slot.GetComponent<SlotData>().GetItemData();
                if(data != null) items.Add(new StoreItem(data.slotID, data.amount, data.item.ID));
            }

            string name = "Inventory";
            string json = JsonMapper.ToJson(items);
            PlayerPrefs.SetString(name, json);
        }
    }

    //Load inventory data in player prefs
    void LoadInventoryFromPrefs() {
        string name = "Inventory";
        string json = PlayerPrefs.GetString(name);

        if(json != ""){
            JsonData data = JsonMapper.ToObject(json);

            if(data != null){
                for (int i = 0; i < data.Count; i++){
                    AddNewItemToSlot((int)data[i]["ItemID"], (int)data[i]["SlotID"], (int)data[i]["Amount"]);
                }
            }
        }
    }
}

