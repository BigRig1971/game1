using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SlotData : MonoBehaviour, IDropHandler
{
    public int ID;//slot id
    public string Type;

    InventoryManager manager;

    void Awake () {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>();
        if(manager == null) Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");
    }

    //Check if slot has any item
    public bool HasItem () {
        return transform.childCount > 0;
    }

    //Get data of slot item
    public ItemData GetItemData() {
        return transform.childCount > 0?transform.GetChild(0).GetComponent<ItemData>():null;
    }

    //Remove all items from slot
    public void Clear(){
        for (int i = transform.childCount - 1; i >= 0; i--){
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    //if item droped on this slot
    public void OnDrop(PointerEventData eventData)
    {
        if(ID != -1){
            ItemData droppedItemData = eventData.pointerDrag.GetComponent<ItemData>();

            //check if slot already has any item
            if(HasItem()) {
                ItemData currentItemData = GetItemData();

                //if dropped id and this slot item id are equal and this slot item is stackable
                if(currentItemData != null && droppedItemData.item.ID == currentItemData.item.ID && currentItemData.item.Stackable){
                    Clear();//clear slot
                    droppedItemData.amount += currentItemData.amount;//increase amount of this item
                }else{
                    if(manager == null) Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");
                    else manager.ChangeItemSlot(ID, droppedItemData.slotID);//swap items
                }
            }

            droppedItemData.slotID = ID;
        }
    }
}
