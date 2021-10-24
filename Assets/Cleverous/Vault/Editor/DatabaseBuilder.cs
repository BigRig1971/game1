// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cleverous.VaultSystem;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cleverous.VaultDashboard
{
    public class DatabaseBuilder : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            BuildDatabase();
        }

        /// <summary>
        /// Runs every time a Build is started or Scripts are recompiled. Must be done to sync editor work into the runtime.
        /// </summary>
        [DidReloadScripts]
        [MenuItem("Tools/Cleverous/Vault/Rebuild Vault Database File", priority = 10)]
        public static void BuildDatabase()
        {
            List<DataEntity> list = GetAllAssetsInProject(typeof(DataEntity));
            Vault.InitData();
            if (Vault.Data != null) Vault.Data.Items = list;
        }

        /// <summary>
        /// Forces a refresh of assets serialization.
        /// </summary>
        [MenuItem("Tools/Cleverous/Vault/Reimport Assets - By Type", priority = 100)]
        public static void ReimportAllByType()
        {
            bool confirm = EditorUtility.DisplayDialog("Reimport Vault Asset Files", $"Reimport all of the Vault Data Assets?\n\n" +
                                                                                     $"This reimports all DataEntity type Assets. Won't fix issues related to mismatching class/file names.\n\n This is generally a safe operation.", "Proceed", "Abort!");
            if (!confirm) return;

            int count = 0;
            AssetDatabase.StartAssetEditing();
            try
            {
                string storage = Vault.VaultItemPath;
                if (storage[storage.Length - 1] == '/') storage = storage.Remove(storage.Length - 1);
                string[] files = AssetDatabase.FindAssets("t:DataEntity", new[] { storage });
                for (int i = 0; i < files.Length; i++)
                {
                    EditorUtility.DisplayProgressBar("Importing...", AssetDatabase.GUIDToAssetPath(files[i]), (float)i / files.Length);
                    AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(files[i]), ImportAssetOptions.ForceUpdate);
                    Debug.Log($"{AssetDatabase.GUIDToAssetPath(files[i])}");
                    count++;
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog(
                    "Done reimporting",
                    $"{count} assets were reimported and logged to the console.",
                    "Great");
            }
        }
        
        /// <summary>
        /// Forces a refresh of assets serialization.
        /// </summary>
        [MenuItem("Tools/Cleverous/Vault/Reimport Assets - By Name", priority = 100)]
        public static void ReimportAllByName()
        {
            bool confirm = EditorUtility.DisplayDialog(
                "Reimport Vault Asset Files", 
                "Reimport all of the Vault Data Assets?\n\n" +
                "This reimports all files with names including 'Data-' which is the built-in prefix for saved Vault Files.\n\n This is generally a safe operation.", 
                "Proceed", 
                "Abort");

            if (!confirm) return;

            int count = 0;
            AssetDatabase.StartAssetEditing();
            try
            {
                string storage = Vault.VaultItemPath;
                if (storage[storage.Length - 1] == '/') storage = storage.Remove(storage.Length - 1);
                string[] files = AssetDatabase.FindAssets("Data-", new[] { storage });
                for (int i = 0; i < files.Length; i++)
                {
                    EditorUtility.DisplayProgressBar("Importing...", AssetDatabase.GUIDToAssetPath(files[i]), (float)i / files.Length);
                    AssetDatabase.ImportAsset(AssetDatabase.GUIDToAssetPath(files[i]), ImportAssetOptions.ForceUpdate);
                    UnityEngine.Debug.Log($"{AssetDatabase.GUIDToAssetPath(files[i])}");
                    count++;
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog(
                    "Done reimporting",
                    $"{count} assets were reimported and logged to the console.",
                    "Great");
            }
        }
        
        [MenuItem("Tools/Cleverous/Vault/Cleanup Vault", priority = 100)]
        public static void CleanupStorageFolder()
        {
            bool confirm = EditorUtility.DisplayDialog(
                "Cleanup Vault", 
                "This will check all asset files with the 'Data-' prefix and ensure validity.\n\n" +
                "This is primarily for identifying or removing assets which have broken script connections due to naming mismatches or class deletions. You will be able to confirm deletion/bypass for each file individually.\n", 
                "Proceed", 
                "Abort");

            if (!confirm) return;

            List<string> failedGuids = new List<string>();
            int found = 0;
            int deleted = 0;
            int failed = 0;
            int ignored = 0;
            AssetDatabase.StartAssetEditing();
            try
            {
                string storage = Vault.VaultItemPath;
                if (storage[storage.Length - 1] == '/') storage = storage.Remove(storage.Length - 1);
                string[] files = AssetDatabase.FindAssets("Data-", new[] { storage });
                for (int i = 0; i < files.Length; i++)
                {
                    EditorUtility.DisplayProgressBar("Scanning...", AssetDatabase.GUIDToAssetPath(files[i]), (float)i / files.Length);

                    string path = AssetDatabase.GUIDToAssetPath(files[i]);
                    DataEntity dataFile = AssetDatabase.LoadAssetAtPath<DataEntity>(AssetDatabase.GUIDToAssetPath(files[i]));
                    if (dataFile == null)
                    {
                        found++;

                        // how the heck do i get the object if we're literally dealing with objects that don't cast.
                        //EditorGUIUtility.PingObject();

                        bool deleteFaulty = EditorUtility.DisplayDialog(
                            "Faulty file found",
                            $"{path}\n\n" +
                            "This file seems to be broken. Please check:\n\n" +
                            "* File is actually a Vault Data file.\n" +
                            "* Class file still exists.\n" +
                            "* Class filename matches class name.\n" +
                            "* Assemblies are not black-listed.\n\n" +
                            "What do you want to do?", "Delete file", "Ignore file");

                        if (deleteFaulty)
                        {
                            bool success = AssetDatabase.DeleteAsset(path);
                            if (success) deleted++;
                            else failed++;
                        }
                        else ignored++;
                    }
                }
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtility.ClearProgressBar();
                EditorUtility.DisplayDialog(
                    "Done cleaning.",
                    $"{found} assets were faulty.\n" +
                    $"{deleted} assets were deleted.\n" +
                    $"{failed} assets failed to delete.\n" +
                    $"{ignored} assets were ignored.\n",
                    "Excellent");
            }

        }
        public static List<DataEntity> GetAllAssetsInProject(Type filterType)
        {
            List<DataEntity> list = new List<DataEntity>();

            string[] guids = AssetDatabase.FindAssets($"t:{filterType}");
            list.AddRange(guids.Select(guid => AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(DataEntity)) as DataEntity));

            return list;
        }
    }
}