// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Cleverous.VaultDashboard
{
    internal class DataEntityDropdownItem : AdvancedDropdownItem
    {
        public DataEntity Entity;
        public DataEntityDropdownItem(string name, DataEntity entity) : base(name)
        {
            Entity = entity;
        }
    }

    internal class VaultAdvancedDropdown : AdvancedDropdown
    {
        public SerializedProperty TargetProperty;
        public VaultAdvancedDropdown(AdvancedDropdownState state) : base(state)
        {
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new AdvancedDropdownItem(AssetDropdownDrawer.CurrentFilterType.ToString());
            foreach (DataEntity data in AssetDropdownDrawer.CurrentContent)
            {
                root.AddChild(new DataEntityDropdownItem(data.Title, data));
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            DataEntity entity = ((DataEntityDropdownItem) item).Entity;
            AssetDropdownDrawer.ItemSelected(TargetProperty, entity);
            TargetProperty.serializedObject.ApplyModifiedProperties();
        }
    }
}