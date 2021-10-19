using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(InventoryManager)), CanEditMultipleObjects]
public class InventoryManagerEditor : Editor
{
    SerializedProperty SlotsAmount;

    int itemID = 0;
    int slotID = 0;

    void OnEnable() {
        SlotsAmount = serializedObject.FindProperty("SlotsAmount");
     }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        InventoryManager manager = (InventoryManager)target;

        EditorGUILayout.HelpBox("InventorySystem lets player drag and drop items, throw them away, stack, swap, collect, split and etc", MessageType.Info);

        if(EditorApplication.isPlaying){
            if(manager.InventorySlots == null) EditorGUILayout.HelpBox("Please, set the Inventory Slot Place", MessageType.Error);
            if(manager.InventoryButton == null) EditorGUILayout.HelpBox("Please, set the Inventory Open Button", MessageType.Error);
            if(manager.ItemThrow == null) EditorGUILayout.HelpBox("Please, set the Inventory Item Trow Away Panel", MessageType.Error);
            if(manager.TipPanel == null) EditorGUILayout.HelpBox("Please, set the Inventory Tip Panel", MessageType.Error);
            if(manager.Character == null) EditorGUILayout.HelpBox("Please, set the Item Collector", MessageType.Error);
            if(manager.SplitPanel == null) EditorGUILayout.HelpBox("Please, set the Inventory Split Panel", MessageType.Error);
        }

        GUILayout.Label("Inventory Properties", EditorStyles.boldLabel);

        EditorGUILayout.IntSlider(SlotsAmount, 1, 21);

        EditorGUILayout.Space();

        if(!EditorApplication.isPlaying) EditorGUILayout.HelpBox("You can add new item only in Play Mode", MessageType.Info);
        else {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Item ID");
            itemID = EditorGUILayout.IntField(itemID);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Slot ID");
            slotID = EditorGUILayout.IntField(slotID);
            GUILayout.EndHorizontal();
            if(GUILayout.Button("Add new item to empty slot") && manager.InventorySlots != null) manager.AddNewItem(itemID, false);
            if(GUILayout.Button("Add new item to slot") && manager.InventorySlots != null) manager.AddNewItemToSlot(itemID, slotID);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
