using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipManager : MonoBehaviour
{
    public int SlotsAmount;

    //Player stats
    public int Power = 0;
    public int Price = 0;

    //GameObject initiated by scripts
    public GameObject EquipSlots;
    public GameObject EquipInfo;

    InventoryManager manager;

    void Awake () {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) {
            manager.Equip = gameObject;
        }
        else Debug.LogError("[EquipSystem Error]: Can't find InventorySystem");
    }

    void Update () {
        CheckStats();
        SetEquipInfo();
    }

    //Set text in EquipInfo panel
    void SetEquipInfo() {
        string text = "<color=#FF5C34><b>Power: " + "<color=#000000>" + Power + "</color>" + "</b></color>"
        + "\n<color=#FF5C34><b>Money: "+ "<color=#000000>" + Price + "</color>" + "</b></color>";

        EquipInfo.transform.Find("Text").GetComponent<Text>().text = text;
    }

    //Checking Equip slots and read info from items to update stats
    void CheckStats() {
        Power = 0;
        Price = 0;
        foreach (var slot in manager.SlotList)
        {
            SlotData slotData = slot.GetComponent<SlotData>();
            if(slotData.Type == "Equip" && slotData.HasItem()){
                for(int i=0; i < slotData.GetItemData().amount; i++) {
                    Power += slotData.GetItemData().item.Power;
                    Price += slotData.GetItemData().item.Price;
                }
            }
        }
    }
}
