using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EZInventory;


namespace EZInventory
{
    [CustomEditor(typeof(RecipeGrid))]
    public class RecipeGridEditor : Editor
    {
        RecipeGrid recipeComplex;

        public void OnEnable()
        {
            recipeComplex = (RecipeGrid)target;
        }

        public override void OnInspectorGUI()
        {
            int size = 90;

            recipeComplex.output = (ItemSO)EditorGUILayout.ObjectField("Result: ", recipeComplex.output, typeof(ItemSO), true);
            recipeComplex.outputAmount = EditorGUILayout.IntField("Result Amount: ", recipeComplex.outputAmount);

            EditorGUILayout.BeginHorizontal();
            recipeComplex.topLeft = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.topLeft, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            recipeComplex.topMiddle = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.topMiddle, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            recipeComplex.topRight = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.topRight, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            recipeComplex.middleLeft = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.middleLeft, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            recipeComplex.middle = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.middle, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            recipeComplex.middleRight = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.middleRight, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            recipeComplex.bottomLeft = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.bottomLeft, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            recipeComplex.bottomMiddle = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.bottomMiddle, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            recipeComplex.bottomRight = (ItemSO)EditorGUILayout.ObjectField(recipeComplex.bottomRight, typeof(ItemSO), false, GUILayout.Width(size), GUILayout.Height(size));
            EditorGUILayout.EndHorizontal();

            EditorUtility.SetDirty(recipeComplex);
        }
    }
}
