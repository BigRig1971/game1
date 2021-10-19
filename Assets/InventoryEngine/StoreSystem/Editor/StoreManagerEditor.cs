using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(StoreManager))]
public class StoreManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        StoreManager storeManager = (StoreManager)target;

        EditorGUILayout.HelpBox("StoreSystem lets player store items in gameObjects (like chest) with StoreData.cs script!", MessageType.Info);

        EditorGUILayout.HelpBox("How to add new Storage:"
        + "\n\n1) Create gameObject and set IsTrigger to True in Collider component"
        + "\n\n2) Add StoreData.cs script to this gameObject"
        , MessageType.Info);
    }
}
