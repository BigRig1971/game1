using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(EquipManager)), CanEditMultipleObjects]
public class EquipManagerEditor : Editor
{
    SerializedProperty SlotsAmount;
    
    void OnEnable() {
         SlotsAmount = serializedObject.FindProperty("SlotsAmount");
     }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EquipManager manager = (EquipManager)target;

        EditorGUILayout.HelpBox("EquipSystem lets player read items' parameters and set player stats", MessageType.Info);

        if(EditorApplication.isPlaying){
            if(manager.EquipSlots == null) EditorGUILayout.HelpBox("Please, set the Equip Slot Place", MessageType.Error);
            if(manager.EquipInfo == null) EditorGUILayout.HelpBox("Please, set the Equip Info", MessageType.Error);
        }

        EditorGUILayout.IntSlider(SlotsAmount, 1, 21);
        serializedObject.ApplyModifiedProperties();
    }
}
