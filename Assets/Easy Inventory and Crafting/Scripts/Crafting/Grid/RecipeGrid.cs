using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;

namespace EZInventory
{
    [CreateAssetMenu(fileName = "GR_New Recipe", menuName = "EZ Inventory/Recipe Grid")]
    public class RecipeGrid : ScriptableObject
    {
        public ItemSO topLeft, topMiddle, topRight,
            middleLeft, middle, middleRight,
            bottomLeft, bottomMiddle, bottomRight,
            output;

        public int outputAmount;
    }
}
