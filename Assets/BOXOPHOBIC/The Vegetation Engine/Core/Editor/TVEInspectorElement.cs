//Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace TheVegetationEngine
{
    [DisallowMultipleComponent]
    [CustomEditor(typeof(TVEElement))]
    public class TVEInspectorElement : Editor
    {
        private static readonly string excludeProps = "m_Script";
        private TVEElement targetScript;

        void OnEnable()
        {
            targetScript = (TVEElement)target;
        }

        public override void OnInspectorGUI()
        {
            DrawInspector();

            if (targetScript.inVolume)
            {
                var sharedMaterial = targetScript.gameObject.GetComponent<Renderer>().sharedMaterial;

                if (sharedMaterial != null)
                {
                    if (!EditorUtility.IsPersistent(sharedMaterial))
                    {
                        EditorGUILayout.HelpBox("Save the material to be able to use it in prefabs or to enable GPU Instancing support!", MessageType.Info);
                        GUILayout.Space(10);

                        if (GUILayout.Button("Save Material to Disk"))
                        {
                            var savePath = EditorUtility.SaveFilePanelInProject("Save Material", "My Element", "mat", "Save Element material to disk!");

                            if (savePath != "")
                            {
                                // Add TVE Element to be able to find the material on upgrade
                                var fullPath = savePath.Replace(".mat", " (TVE Element).mat");

                                AssetDatabase.CreateAsset(sharedMaterial, fullPath);
                                targetScript.gameObject.GetComponent<Renderer>().sharedMaterial = AssetDatabase.LoadAssetAtPath<Material>(fullPath);

                                if (Application.isPlaying == false)
                                {
                                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                                }

                                AssetDatabase.Refresh();
                            }
                        }
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("The current element is outside the Global Volume / Follow Main Camera Volume and it is not rendered! Activate the scene Gizmos to see the volume bounds!", MessageType.Error);
            }

            GUILayout.Space(5);
        }

        void DrawInspector()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, excludeProps);

            serializedObject.ApplyModifiedProperties();
        }
    }
}


