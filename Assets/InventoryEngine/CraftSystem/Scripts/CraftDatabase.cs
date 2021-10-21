using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEditor;

public class CraftDatabase : MonoBehaviour
{
    public List<Recipe> recipes;

    void Awake()
    {
        LoadRecipeFromFile();
    }
    
    //Load and parse recipes from JSON file
    private void LoadRecipeFromFile () {
        try {
            recipes = new List<Recipe>();

            TextAsset txtAsset = Resources.Load("Recipes/Recipes") as TextAsset;
            JsonData recipesData = JsonMapper.ToObject(txtAsset.text);

            if(recipesData != null){
                for (int i = 0; i < recipesData.Count; i++){

                    List<RecipeItem> recipeItems = new List<RecipeItem>();

                    for(int j = 0; j < recipesData[i]["items"].Count; j++){
                        var item = recipesData[i]["items"][j];
                        recipeItems.Add(new RecipeItem((int)item["id"], (int)item["amount"]));
                    }

                    recipes.Add(new Recipe((int)recipesData[i]["id"], (int)recipesData[i]["itemID"], recipeItems));
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
    public Recipe FindRecipeByID (int id) {
        foreach (var recipe in recipes) {
            if(recipe.ID == id) {
                return recipe;
            }
        }

        Debug.LogWarning("[InventorySystem Warning]: Recipe with ID = " + id.ToString() + " wasn't found in your JSON database");
        return null;
    }
}

[System.Serializable] 
public class Recipe
{
    public int ID;
    public int ItemID;
    public List<RecipeItem> Items;

    public Recipe(int id, int itemID, List<RecipeItem> items){
        ID = id;
        ItemID = itemID;
        Items = items;
    }

    public Recipe(int id){
        ID = id;
        ItemID = -1;
        Items = new List<RecipeItem>();
    }
}

[System.Serializable] 
public class RecipeItem {
    public int ID;
    public int Amount; 

    public RecipeItem(int id, int amount){
        ID = id;
        Amount = amount;
    }
}