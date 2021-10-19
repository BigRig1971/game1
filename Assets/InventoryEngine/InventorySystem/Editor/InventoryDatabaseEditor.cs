using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InventoryDatabase))]
public class InventoryDatabaseEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        InventoryDatabase database = (InventoryDatabase)target;

        EditorGUILayout.HelpBox("Database is automatically created from the JSON file in Resources/Items  folder", MessageType.Info);
        EditorGUILayout.HelpBox("NOTE: fields of your items' data in JSON file must correspond to the fields in Item class in InventoryDatabase.cs script!", MessageType.Info);
        EditorGUILayout.HelpBox("How to add new Item to database:"
        + "\n\n1) Add new data to Items.json in Resources/Items folder"
        + "\n\n2) Add new sprite (TextureType = \"Sprite (2D and UI)\") to Resources/Items/Sprites folder with the name of your new item ID."
        + "\n\n3) Create new 3D object with the name of your new item ID. Add ItemCollectable script and set ItemID. In Collider component set IsTrigger to True. Add your new 3D object to .../Items/Prefabs folder."
        , MessageType.Info);

        DrawDefaultInspector();
    }
}