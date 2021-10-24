// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEditor;
using UnityEngine;

namespace Cleverous.VaultDashboard
{
    internal class VaultSettings : ScriptableObject
    {
        public const string SettingsPath = "Assets/Cleverous/Vault/Editor/VaultSettings.asset";
        
        public int CurrentAssetIndex;
        public string CurrentTypeName;
        public string BreadcrumbBarGuids;
        public string SelectedAssetGuids;
        public string SearchType;
        public string SearchAssets;

        internal static VaultSettings GetOrCreateSettings()
        {
            VaultSettings settings = AssetDatabase.LoadAssetAtPath<VaultSettings>(SettingsPath);
            if (settings != null) return settings;

            settings = CreateInstance<VaultSettings>();
            AssetDatabase.CreateAsset(settings, SettingsPath);
            AssetDatabase.SaveAssets();

            return settings;
        }
        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}