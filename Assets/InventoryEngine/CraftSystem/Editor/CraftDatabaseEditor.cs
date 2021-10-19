using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CraftDatabase))]
public class CraftDatabaseEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        CraftDatabase database = (CraftDatabase)target;

        EditorGUILayout.HelpBox("Database is automatically created from the JSON file in Resources folder", MessageType.Info);
        EditorGUILayout.HelpBox("NOTE: fields of your recipes' data in JSON file must correspond to the fields in Recipe class in CraftDatabase.cs script!", MessageType.Info);
        EditorGUILayout.HelpBox("How to add new Recipe to database:"
        + "\n\n1) Add new data to Recipes.json in Resources/Recipes folder"
        + "\n\nNOTE: Note that \"itemID\" (and \"id\" in \"items\") in Recipes.json file must be got from items in Items.json file."
        , MessageType.Info);

        DrawDefaultInspector();
    }
}
