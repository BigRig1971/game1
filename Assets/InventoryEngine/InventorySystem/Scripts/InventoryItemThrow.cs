using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemThrow : MonoBehaviour, IDropHandler
{
    InventoryManager manager;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) manager.ItemThrow = gameObject;
        else Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");
    }

    public void OnDrop(PointerEventData eventData)
    {
        ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
        ThrowItemAway(droppedItem.item, manager.Character.transform, droppedItem.amount);
        eventData.pointerDrag.GetComponent<ItemData>().slotID = -1;
    }

    //throw item  away to the world
    public void ThrowItemAway(Item throwItem, Transform origin, int amount = 1) {
        for(int k = 0; k < amount; k++){
            GameObject away = Instantiate(throwItem.Prefab);
            
            away.transform.position = origin.position;
            away.transform.rotation = origin.rotation;

            for (float i = 0.0f; i < 2f; i += Time.deltaTime)
            {
                away.transform.Translate(-1 * Time.deltaTime, 0, 0, origin);
            }
        }
    }
}
