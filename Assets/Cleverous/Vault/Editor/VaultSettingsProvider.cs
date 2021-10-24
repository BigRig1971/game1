// (c) Copyright Cleverous 2020. All rights reserved.

using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public static class VaultSettingsRegister
    {
        [SettingsProvider]
        public static SettingsProvider MakeVaultSettingsProvider()
        {
            SettingsProvider provider = new SettingsProvider("Project/Cleverous/Vault", SettingsScope.Project)
            {
                label = "Vault",
                activateHandler = DrawContent,
                keywords = new HashSet<string>(new[] { "CurrentAssetIndex", "StoragePath", "CurrentTypeName", "BreadcrumbBarGuids", "SelectedAssetGuids" })
            };
            return provider;
        }

        private static void DrawContent(string searchContext, VisualElement rootElement)
        {
            SerializedObject settings = VaultSettings.GetSerializedSettings();

            Label title = new Label {text = "  Vault", style = { fontSize = 20, unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold)}};
            title.AddToClassList("title");
            rootElement.Add(title);
            
            VisualElement properties = new VisualElement {style = {flexDirection = FlexDirection.Column}};
            PropertyField index = new PropertyField(settings.FindProperty("CurrentAssetIndex"));
            PropertyField breadcrumbGuids = new PropertyField(settings.FindProperty("BreadcrumbBarGuids"));
            PropertyField curTypeName = new PropertyField(settings.FindProperty("CurrentTypeName"));
            PropertyField storagePath = new PropertyField(settings.FindProperty("StoragePath"));
            PropertyField selectedGuids = new PropertyField(settings.FindProperty("SelectedAssetGuids"));
            PropertyField searchType = new PropertyField(settings.FindProperty("SearchType"));
            PropertyField searchAssets = new PropertyField(settings.FindProperty("SearchAssets"));

            selectedGuids.AddToClassList("property-value");
            properties.AddToClassList("property-list");
            storagePath.AddToClassList("property-value");
            curTypeName.AddToClassList("property-value");
            breadcrumbGuids.AddToClassList("property-value");
            index.AddToClassList("property-value");
            searchType.AddToClassList("property-value");
            searchAssets.AddToClassList("property-value");

            rootElement.Add(properties);
            properties.Add(storagePath);
            properties.Add(curTypeName);
            properties.Add(breadcrumbGuids);
            properties.Add(selectedGuids);
            properties.Add(index);
            properties.Add(searchType);
            properties.Add(searchAssets);

            index.Bind(settings);
            breadcrumbGuids.Bind(settings);
            curTypeName.Bind(settings);
            storagePath.Bind(settings);
            selectedGuids.Bind(settings);
            searchType.Bind(settings);
            searchAssets.Bind(settings);
        }
    }
}