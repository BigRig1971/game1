// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements; 
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultAssetInspector : ScrollView
    {
        protected SerializedObject TargetSerializedObject;

        public VaultAssetInspector()
        {
            TargetSerializedObject = GetSerializedObj();
            VaultDashboard.OnCurrentAssetChanged += ReDraw;

            this.name = "Asset Inspector";
            this.viewDataKey = "ASSET_INSPECTOR";
            this.style.flexShrink = 1f;
            this.style.flexGrow = 1f;
            this.style.paddingBottom = 10;
            this.style.paddingLeft = 10;
            this.style.paddingRight = 10;
            this.style.paddingTop = 10;

            ReDraw();
        }

        public void InspectNothing()
        {
            Clear();
            Add(new Label { text = " ⓘ Asset Inspector" });
            Add(new Label("\n\n    ⚠ Please select an asset from the column to the left."));
        }
        public void ReDraw()
        {
            Clear();
            if (VaultDashboard.CurrentAsset == null)
            {
                InspectNothing();
                return;
            }

            TargetSerializedObject = GetSerializedObj();

            bool success = BuildInspectorProperties(TargetSerializedObject, this);
            if (success) this.Bind(TargetSerializedObject);
        }
        private static SerializedObject GetSerializedObj()
        {
            return VaultDashboard.CurrentAsset == null 
                ? null 
                : Editor.CreateEditor(VaultDashboard.CurrentAsset).serializedObject;
        }
        private static bool BuildInspectorProperties(SerializedObject obj, VisualElement wrapper)
        {
            if (obj == null || wrapper == null) return false;

            wrapper.Add(new Label { text = " ⓘ Asset Inspector" });

            SerializedProperty iterator = obj.GetIterator();
            Type targetType = obj.targetObject.GetType();
            List<MemberInfo> members = new List<MemberInfo>(targetType.GetMembers());

            if (!iterator.NextVisible(true)) return false;
            do
            {
                PropertyField propertyField = new PropertyField(iterator.Copy())
                {
                    name = "PropertyField:" + iterator.propertyPath
                };

                MemberInfo member = members.Find(x => x.Name == propertyField.bindingPath);
                if (member != null)
                {
                    // TODO [Header()] and [Space()] are manually added until Unity supports them.
                    IEnumerable<Attribute> headers = member.GetCustomAttributes(typeof(HeaderAttribute));
                    IEnumerable<Attribute> spaces = member.GetCustomAttributes(typeof(SpaceAttribute));

                    foreach (Attribute x in headers)
                    {
                        HeaderAttribute actual = (HeaderAttribute)x;
                        Label header = new Label { text = actual.header };
                        header.style.unityFontStyleAndWeight = FontStyle.Bold;
                        wrapper.Add(new Label { text = " " });
                        wrapper.Add(header);
                    }
                    foreach (Attribute unused in spaces)
                    {
                        wrapper.Add(new Label { text = " " });
                    }
                }

                // if this property is the script field
                if (iterator.propertyPath == "m_Script" && obj.targetObject != null)
                {
                    // build the container
                    VisualElement container = new VisualElement();
                    container.style.flexGrow = 1;
                    container.style.flexShrink = 1;
                    container.style.alignItems = new StyleEnum<Align>(Align.Stretch);
                    container.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);

                    propertyField.SetEnabled(false);
                    propertyField.style.flexGrow = 1;
                    propertyField.style.flexShrink = 1;

                    // build the focus script button
                    Button focusButton = new Button(() => EditorGUIUtility.PingObject(obj.FindProperty("m_Script").objectReferenceValue));
                    focusButton.text = "☲";
                    focusButton.style.minWidth = 20;
                    focusButton.style.maxWidth = 20;
                    focusButton.tooltip = "Ping this Script";                    
                    
                    // build the focus object button
                    Button focusAsset = new Button(() => EditorGUIUtility.PingObject(obj.targetObject));
                    focusAsset.text = "☑";
                    focusAsset.style.minWidth = 20;
                    focusAsset.style.maxWidth = 20;
                    focusAsset.tooltip = "Ping this Asset";

                    // draw it
                    container.Add(propertyField);
                    container.Add(focusButton);
                    container.Add(focusAsset);
                    wrapper.Add(container);
                }
                // if it isn't the script field, just add the property field like normal.
                else wrapper.Add(propertyField);
            }
            while (iterator.NextVisible(false));
            return true;
        }
    }
}