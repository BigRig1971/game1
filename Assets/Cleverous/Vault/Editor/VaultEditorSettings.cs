// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using UnityEditor;

namespace Cleverous.VaultDashboard
{
    public class VaultEditorSettings
    {
        private static VaultSettings m_settings;
        public enum VaultData
        {
            CurrentAssetIndex, 
            CurrentTypeName, 
            BreadcrumbBarGuids, 
            SelectedAssetGuids, 
            SearchType, 
            SearchAssets
        }

        private static void Load()
        {
            m_settings = VaultSettings.GetOrCreateSettings();
        }

        public static int GetInt(VaultData data)
        {
            if (m_settings == null) Load();
            if (m_settings == null) UnityEngine.Debug.LogError("Error loading editor persistence settings file.");

            switch (data)
            {
                case VaultData.CurrentAssetIndex: return m_settings.CurrentAssetIndex;
            }

            UnityEngine.Debug.LogError("Error Getting int data.");
            return -1;
        }
        public static string GetString(VaultData data)
        {
            if (m_settings == null) Load();
            if (m_settings == null) UnityEngine.Debug.LogError("Error loading editor persistence settings file.");

            switch (data)
            {
                case VaultData.CurrentTypeName: return m_settings.CurrentTypeName;
                case VaultData.BreadcrumbBarGuids: return m_settings.BreadcrumbBarGuids;
                case VaultData.SelectedAssetGuids: return m_settings.SelectedAssetGuids;
                case VaultData.SearchType: return m_settings.SearchType;
                case VaultData.SearchAssets: return m_settings.SearchAssets;
            }

            UnityEngine.Debug.LogError("Error Getting string data.");
            return null;
        }

        public static void SetInt(VaultData data, int value)
        {
            if (m_settings == null) Load();
            if (m_settings == null) UnityEngine.Debug.LogError("Error loading editor persistence settings file.");

            switch (data)
            {
                case VaultData.CurrentAssetIndex:
                    m_settings.CurrentAssetIndex = value; 
                    break;
            }

            EditorUtility.SetDirty(m_settings);
        }
        public static void SetString(VaultData data, string value)
        {
            if (m_settings == null) Load();
            if (m_settings == null) UnityEngine.Debug.LogError("Error loading editor persistence settings file.");

            switch (data)
            {
                case VaultData.CurrentTypeName:
                    m_settings.CurrentTypeName = value;
                    break;
                case VaultData.BreadcrumbBarGuids:
                    m_settings.BreadcrumbBarGuids = value;
                    break;
                case VaultData.SelectedAssetGuids:
                    m_settings.SelectedAssetGuids = value;
                    break;
                case VaultData.CurrentAssetIndex: break;
                case VaultData.SearchType:
                    m_settings.SearchType = value;
                    break;
                case VaultData.SearchAssets:
                    m_settings.SearchAssets = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data), data, null);
            }

            EditorUtility.SetDirty(m_settings);
        }
    }
}