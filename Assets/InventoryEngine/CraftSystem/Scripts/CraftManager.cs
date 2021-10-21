using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftManager : MonoBehaviour
{
    InventoryManager manager;

    //GameObject initiated by scripts
    public GameObject CraftSlots;
    public GameObject CraftInfo;
    public GameObject CraftButton;

    //only CraftManager veriables
    public List<GameObject> SlotList;
    public Recipe ChosenRecipe;

    //Private variables
    public CraftDatabase database;
    public GameObject slot;
    public GameObject recipe;

    void Awake()
    {
        manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 
        if(manager != null) manager.Craft = gameObject;
        else Debug.LogError("[CraftSystem Error]: Can't find InventorySystem");

        database = transform.GetComponent<CraftDatabase>();
        slot = Resources.Load<GameObject>("Prefabs/Slot");
        recipe = Resources.Load<GameObject>("Prefabs/Recipe");
        SlotList = new List<GameObject>();
    }

    void Start () {
        CreateCraftSlots();
        OpenCloseCraft();
    }

    //open or close craft panel
    public void OpenCloseCraft() {
        CraftSlots.SetActive(!CraftSlots.activeSelf);
        CraftInfo.SetActive(!CraftInfo.activeSelf);

        CraftInfo.GetComponent<CraftInfo>().Deactivate();
        ChosenRecipe = null;
    }

    //Return CraftSystem slots amount
    public int GetSlotsAmount () {
        return SlotList.Count;
    }

    //check inventory if it has all necessary items and craft chosen item
    public void CraftItem() {
        if(ChosenRecipe != null){
            Debug.Log("");
            Debug.Log("ChosenRecipe.ID: " + ChosenRecipe.ID);
            List<string> types = new List<string>(new string[]{"Inventory", "Equip"});//which slots can be used

            if(CanCraft()){
                Debug.Log("ChosenRecipe.Items.Count: " + ChosenRecipe.Items.Count);
                for(int i =0; i < ChosenRecipe.Items.Count; i++)
                {
                    Debug.Log("i: " + i);
                    Debug.Log("ChosenRecipe.Items[i].ID: " + ChosenRecipe.Items[i].ID);
                    Debug.Log("ChosenRecipe.Items[i].Amount: " + ChosenRecipe.Items[i].Amount);
                    for(int j =0; j < ChosenRecipe.Items[i].Amount; j ++) {
                        Debug.Log("j: " + j);
                        manager.RemoveItem(ChosenRecipe.Items[i].ID, types);//remove all necessary items to craft
                    }
                }
                manager.AddNewItem(ChosenRecipe.ItemID, types: types);//add crafted item
            }else {
                Debug.Log("[CraftSystem Info]: Not enough items in slots with Type == \"Inventory\"!");
            }
        }
    }

    //check inventory if it has all necessary
    public bool CanCraft () {
        bool canCraft = true;
        List<string> types = new List<string>(new string[]{"Inventory", "Equip"});//which slots can be used

        if(ChosenRecipe != null){
            for(int i =0; i < ChosenRecipe.Items.Count; i++){
                if(manager.HasAmountItem(ChosenRecipe.Items[i].ID, ChosenRecipe.Items[i].Amount, types) != 0){
                    canCraft = false;
                }
            }
        }

        return canCraft;
    }

    //Add one slot to Craft panel
    public void AddNewSlot (Recipe recipeToAdd) {
        GameObject slot = this.slot?this.slot:Resources.Load<GameObject>("Prefabs/Slot");
        
        GameObject newSlot = Instantiate(slot, CraftSlots.transform);
        SlotList.Add(newSlot);
        int slotID = SlotList.IndexOf(newSlot);

        newSlot.GetComponent<SlotData>().ID = -1;
        newSlot.GetComponent<SlotData>().Type = "Сraft";

        Item recipeItem = manager.database.FindItemByID(recipeToAdd.ItemID);

        GameObject newRecipe = Instantiate(recipe, newSlot.transform);
        newRecipe.GetComponent<RecipeData>().recipe = recipeToAdd;
        newRecipe.GetComponent<Image>().sprite = recipeItem.Sprite;
    }

    //Set text of craft info panel
    public void SetCraftInfo(Recipe data) {
        Item itemToCraft = manager.database.FindItemByID(data.ItemID);
        string text = "<color=#00008B><b>" + itemToCraft.Name + "</b></color>" ;
        for(int i=0; i< data.Items.Count; i++){
            Item recipeItem = manager.database.FindItemByID(data.Items[i].ID);
            text += "\n<color=#FFFFFF>" + data.Items[i].Amount + "x" + recipeItem.Name + "</color>";
        }

        CraftInfo.GetComponent<CraftInfo>().Activate(text);
        ChosenRecipe = data;
    }

    //Add slots to Craft panel
    public void AddCraftSlots () {
        for(int i = 0; i < database.recipes.Count; i++){
            AddNewSlot(database.recipes[i]);
        }
    }

    //Remove all children from Craft
    public void ClearCraftSlots () {
        int amount = CraftSlots.transform.childCount;
        SlotList = new List<GameObject>();

        for (int i = amount - 1; i >= 0; i--){
            Destroy(CraftSlots.transform.GetChild(i).gameObject);
        }
    }

    //Fill CraftSystem panel with the new slots
    public void CreateCraftSlots () {
        ClearCraftSlots();
        AddCraftSlots();
    }
}
