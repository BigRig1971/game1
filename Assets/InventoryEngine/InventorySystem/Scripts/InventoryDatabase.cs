using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEditor;

public class InventoryDatabase : MonoBehaviour
{
    public List<Item> items;

    void Awake()
    {
        LoadItemsFromFile();
    }
    
    //Load and parse items from JSON file
    private void LoadItemsFromFile () {
        try {
            items = new List<Item>();

            TextAsset txtAsset = Resources.Load("Items/Items") as TextAsset;
            JsonData itemsData = JsonMapper.ToObject(txtAsset.text);

            if(itemsData != null){
                for (int i = 0; i < itemsData.Count; i++)
                {
                    items.Add(new Item((int)itemsData[i]["id"], itemsData[i]["name"].ToString(), (int)itemsData[i]["price"], (int)itemsData[i]["power"], (bool)itemsData[i]["stackable"]));
                }
            }else Debug.LogError("[InventorySystem Error]: Can't find items.json");
        }
        catch(DirectoryNotFoundException e) {
            Debug.LogError("[InventorySystem Error]: Please, set the correct path to JSON file with items data, using \"Path To Data File\" variable");
        }
        catch(FileNotFoundException e) {
            Debug.LogError("[InventorySystem Error]: Please, set the correct path to JSON file with items data, using \"Path To Data File\" variable");
        }
        catch(KeyNotFoundException e) {
            Debug.LogError("[InventorySystem Error]: Can't find the key in your JSON database");
        }
    }

    //Get Item by id from JSON database
    public Item FindItemByID (int id) {
        foreach (var item in items) {
            if(item.ID == id) {
                return item;
            }
        }

        Debug.LogWarning("[InventorySystem Warning]: Item with ID = " + id.ToString() + " wasn't found in your JSON database");
        return null;
    }
}

[System.Serializable] 
public class Item
{
    public int ID;
    public string Name;
    public int Price;
    public int Power;
    public bool Stackable;
    public Sprite Sprite;
    public GameObject Prefab;

    public Item(int id, string name, int price, int power, bool stackable)
    {
        this.ID = id;
        this.Name = name;
        this.Price = price;
        this.Power = power;
        this.Stackable = stackable;
        this.Sprite = Resources.Load<Sprite>("Items/Sprites/" + id.ToString());
        this.Prefab = Resources.Load<GameObject>("Items/Prefabs/" + id.ToString());
    }

    public Item(int id)
    {
        this.ID = id;
        this.Name = "";
        this.Price = 0;
        this.Power = 0;
        this.Stackable = false;
    }

    public Item()
    {
        this.ID = -1;
        this.Name = "";
        this.Price = 0;
        this.Power = 0;
        this.Stackable = false;
    }
}
