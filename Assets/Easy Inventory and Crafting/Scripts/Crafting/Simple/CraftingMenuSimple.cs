using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZInventory;

namespace EZInventory
{
    public class CraftingMenuSimple : MonoBehaviour
    {
        public RecipeSimple[] recipes;
        
        public Transform recipesParent;
        public Transform ingredientsParent;
        public PickUpOnlySlot resultSlot;
        public InventorySlot inventorySlotPrefab;
        public IngredientSlotSimple IngredientSlotPrefab;
        
        RecipeSimple currentRecipe;
        List<IngredientSlotSimple> ingredientSlots;
        Transform currentSlotTransform;

        const int C_INGREDIENT_LIMIT = 4;

        // Start is called before the first frame update
        void Awake()
        {
            foreach(RecipeSimple recipe in recipes)
            {
                InventorySlot slot = Instantiate(inventorySlotPrefab, recipesParent);
                slot.includeInInventory = false;
                slot.interactable = false;
                slot.SetItemInSlot(recipe.result, recipe.resultAmount);
                slot.GetComponent<RectTransform>().sizeDelta = Vector2.one * 45;
                slot.gameObject.AddComponent<Button>().onClick.AddListener(delegate { ShowRecipe(slot.transform, recipe); });
            }

            ingredientSlots = new List<IngredientSlotSimple>();
            for (int i = 0; i < C_INGREDIENT_LIMIT; i++)
            {
                IngredientSlotSimple ingredientSlot = Instantiate(IngredientSlotPrefab, ingredientsParent);
                ingredientSlots.Add(ingredientSlot);
                ingredientSlot.gameObject.SetActive(false);
            }

            
        }

        // Update is called once per frame
        void Update()
        {
            resultSlot.gameObject.SetActive(currentRecipe != null);
            CheckIngredients();
        }

        public void ShowRecipe(Transform slotTransform, RecipeSimple recipe)
        {
            currentRecipe = recipe;

            if (currentSlotTransform) currentSlotTransform.localScale = Vector3.one;
            currentSlotTransform = slotTransform;
            currentSlotTransform.localScale = Vector3.one * 1.2f;

            for (int i = 0; i < ingredientSlots.Count; i++)
            {
                if (i < recipe.ingredients.Length)
                {
                    ingredientSlots[i].gameObject.SetActive(true);
                    ingredientSlots[i].SetItemInSlot(recipe.ingredients[i].item, recipe.ingredients[i].amount);
                }
                else
                {
                    ingredientSlots[i].gameObject.SetActive(false);
                }
            }


        }

        void CheckIngredients()
        {
            if (!currentRecipe)
                return;

            bool craftable = true;
            for (int i = 0; i < currentRecipe.ingredients.Length; i++)
            {
                if (!InventoryManager.CheckItem(currentRecipe.ingredients[i].item, currentRecipe.ingredients[i].amount))
                {
                    ingredientSlots[i].ingredientsAvailable = false;
                    craftable = false;
                }
                else
                {
                    ingredientSlots[i].ingredientsAvailable = true;
                }
            }

            if (craftable)
            {
                resultSlot.SetItemInSlot(currentRecipe.result, currentRecipe.resultAmount);
            }
            else
            {
                resultSlot.SetItemInSlot(null, 0);
            }

        }

        public void RemoveIngredients()
        {
            foreach (RecipeSimple.Ingredient ingredient in currentRecipe.ingredients)
            {
                InventoryManager.RemoveItemFromInventory(ingredient.item, ingredient.amount);
            }
        }
    }
}
