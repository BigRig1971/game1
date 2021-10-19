using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CraftManager))]
public class CraftManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CraftManager craftManager = (CraftManager)target;

        EditorGUILayout.HelpBox("CraftSystem lets player craft items", MessageType.Info);
    }
}

