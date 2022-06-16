using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace StupidHumanGames
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "Inventory/Recipe Simple")]
    public class RecipeSimple : ScriptableObject
    {
        public Ingredient[] ingredients;
        public ItemSO result;
        public int resultAmount = 1;

        [System.Serializable]
        public class Ingredient
        {
            public ItemSO item;
            public int amount;
        }
    }
}
