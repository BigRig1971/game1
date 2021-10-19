using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSplit : MonoBehaviour
{
    InventoryManager manager;

    Button splitButton;
    Slider splitSlider;
    Text slpitText;

    ItemData splitItemData;
    int splitAmount = 1;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) manager.SplitPanel = gameObject;
        else Debug.LogError("[InventorySystem Error]: Can't find InventorySystem");

        splitButton = transform.Find("SplitButton").GetComponent<Button>();
        splitSlider = transform.Find("SplitSlider").GetComponent<Slider>();
        slpitText = transform.Find("Text").GetComponent<Text>();

        splitButton.onClick.AddListener(delegate { SplitItem(); });
        splitSlider.onValueChanged.AddListener (delegate {SplitAmountChange ();});
    }

    void Start()
    {
        Deactivate();
    }

    void Update(){
        ChangeSplitText(splitItemData, splitAmount);
    }

    //set text in split info panel
    public void ChangeSplitText(ItemData itemData, int amount){
        if(itemData != null){
            string data = "<color=#00008B><b>" + itemData.item.Name + ": " + amount.ToString() + "</b></color>" ;
            slpitText.text = data;
        }
    }
    
    //activate split panel
    public void Activate(ItemData itemData){
        ChangeSplitText(itemData, splitAmount);

        splitItemData = itemData;

        if(itemData.amount > 1) {
            splitSlider.interactable = true;
            splitSlider.minValue = 1;
            splitSlider.maxValue = itemData.amount-1;
            splitButton.interactable = true;
        }
    }

    //deactivate split panel
    public void Deactivate(){
        slpitText.text = "Press Item";
        splitItemData = null;
        splitAmount = 1;
        splitButton.interactable = false;
        splitSlider.interactable = false;
        splitSlider.minValue = 1;
        splitSlider.maxValue = 1;
    }

    //Split item
    public void SplitItem () {
        splitItemData.amount -= splitAmount;
        List<string> types = new List<string>(new string[]{"Inventory", "Equip"});//which slots can be used

        manager.AddNewItem(splitItemData.item.ID, false, splitAmount, types: types);
        Deactivate();
    }

    //change amount of items to be splited
    public void SplitAmountChange(){
        splitAmount = (int)splitSlider.value;
	}
}
