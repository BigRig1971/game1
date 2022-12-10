using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HeurekaGames.Utils
{
    public static class Heureka_ResourceLoader
    {
        private static readonly Dictionary<HeurekaPackage, string> iconPaths = new Dictionary<HeurekaPackage, string>()
        {
            {HeurekaPackage.SHARED, "com.heurekagames.utils" },
            {HeurekaPackage.AHP, "com.heurekagames.assethunterpro" },
            {HeurekaPackage.SB, "com.heurekagames.smartbuilder" },
            {HeurekaPackage.AFP, "com.heurekagames.assetfinderpro" }
        };
        public static class Content
        {
            public static GUIContent Previous = GetInternalContentWithTooltip("tab_prev", "Previous");
            public static GUIContent Next = GetInternalContentWithTooltip("tab_next", "Next");
        }

        public static class Icons
        {
            public static Texture Pick = EditorGUIUtility.IconContent("pick").image;
            public static Texture Clear = EditorGUIUtility.IconContent(Heureka_Utils.IsUnityVersionGreaterThan(2020) ? "clear" : "Toolbar Minus").image;
            public static Texture Previous = EditorGUIUtility.IconContent("tab_prev").image;
            public static Texture Next= EditorGUIUtility.IconContent("tab_next").image;
        }

        public static class IconNames
        {
            public static readonly string TabIconAHP = "tabIcon";
        }

        public enum HeurekaPackage
        {
            SHARED,
            AHP,
            SB,
            AFP
        }

        public static string GetIconPath(HeurekaPackage package, string iconName)
        {
            var path = $"Packages/{iconPaths[package]}/UI/Icons/{iconName}.png";
            return path;
        }

        public static Texture GetIcon(HeurekaPackage package, string iconName)
        {
            return EditorGUIUtility.TrIconContent(GetIconPath(package, iconName)).image;
        }

        public static GUIContent GetContentWithTitle(HeurekaPackage package, string iconName, string title)
        {
            return EditorGUIUtility.TrTextContentWithIcon(title, GetIconPath(package, iconName));
        }

        public static GUIContent GetContentWithTooltip(HeurekaPackage package, string iconName, string tooltip)
        {
            return EditorGUIUtility.TrIconContent(GetIconPath(package, iconName), tooltip);
        }

        public static GUIContent GetContent(HeurekaPackage package, string iconName, string title, string tooltip)
        {
            return EditorGUIUtility.TrTextContentWithIcon(title, tooltip, GetIconPath(package, iconName));
        }

        public static GUIContent GetInternalContentWithTooltip(string iconName, string tooltip)
        {
            return new GUIContent(EditorGUIUtility.IconContent(iconName).image, tooltip);
        }

        public static Texture GetInternalIcon(string iconName)
        {
            return EditorGUIUtility.IconContent(iconName).image;
        }
    }
}