using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(StoreData)), CanEditMultipleObjects]
public class StoreDataEditor : Editor
{
    SerializedProperty id;
    SerializedProperty title;
    SerializedProperty description;
    SerializedProperty slotsAmount;
    SerializedProperty items;

    // int itemID = 0;
    // int amount = 1;

    void OnEnable() {
        id = serializedObject.FindProperty("id");
        title = serializedObject.FindProperty("title");
        description = serializedObject.FindProperty("description");
        slotsAmount = serializedObject.FindProperty("slotsAmount");
     }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        StoreData storeData = (StoreData)target;
        InventoryManager manager = GameObject.Find("InventorySystem").GetComponent<InventoryManager>(); 

         GUILayout.Label("Initial Properties", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(id);
        EditorGUILayout.PropertyField(title);
        EditorGUILayout.PropertyField(description);
        EditorGUILayout.PropertyField(slotsAmount);

        EditorGUILayout.HelpBox("Storage slots must have SlotID = -1 to be added to the first empty slot", MessageType.Info);
        EditorGUILayout.HelpBox("If you try to add more than " + storeData.slotsAmount + " not stackable items to store, they will be thrown away", MessageType.Info);

        serializedObject.ApplyModifiedProperties();
    
    }
}
