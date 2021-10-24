// (c) Copyright Cleverous 2020. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cleverous.VaultSystem;
using UnityEditor; 
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cleverous.VaultDashboard
{
    public class VaultAssetColumn : VisualElement
    {
        public ListView ListElement;
        private List<DataEntity> m_allAssetsOfFilteredType;
        private List<DataEntity> m_searchFilteredList;
        private bool m_isSearchFiltering;

        public List<DataEntity> CurrentSelections;

        public VaultAssetColumn()
        {
            Rebuild();
        }

        public void Rebuild()
        {
            Clear();
            m_allAssetsOfFilteredType = new List<DataEntity>();

            this.style.flexGrow = 1;
            this.name = "Asset List Wrapper";
            this.viewDataKey = "asset_list_wrapper";

            ListElement = new ListView(m_allAssetsOfFilteredType, 16, ListMakeItem, ListBindItem);
            ListElement.name = "Asset List View";
            ListElement.viewDataKey = "asset_list";
            ListElement.style.flexGrow = 1;
            ListElement.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
            ListElement.selectionType = SelectionType.Multiple;

            #if UNITY_2020_3_OR_NEWER
            ListElement.onSelectionChange += SelectAssetsInternal;
            ListElement.onItemsChosen += ChooseAssetInternal;
            #else
            ListElement.onSelectionChanged += SelectAssetsInternal;
            ListElement.onItemChosen += ChooseAssetInternal;
            #endif

            // plug into events for updates.
            VaultDashboard.OnAssetSearch += CallbackListBySearch;
            VaultDashboard.OnTypeFilterChanged += CallbackListByType;

            Add(ListElement);
            
            if (!string.IsNullOrEmpty(VaultDashboard.AssetSearch.value))
            {
                // must pre-load the type matches list first as the search filter uses it.
                ListAssetsByType();
                ListAssetsBySearch();
            } 
            else ListAssetsByType();

            GetSelectionPersistence();
        }

        private void CallbackListBySearch() {ListAssetsBySearch(true);}
        private void CallbackListByType() {ListAssetsByType(true);}
         
        public void ListAssetsByType(bool scrollToTop = false)
        {
            m_isSearchFiltering = false;
            if (!Vault.IsReady) Vault.InitData();
            if (Vault.Data.Items.Count != 0)
            {
                m_allAssetsOfFilteredType = DatabaseBuilder.GetAllAssetsInProject(VaultDashboard.CurrentTypeFilter).ToList();
                m_allAssetsOfFilteredType.Sort((x, y) => string.CompareOrdinal(x.Title, y.Title));
                ListElement.itemsSource = m_allAssetsOfFilteredType;
                ListElement.Refresh();
                if (scrollToTop) ListElement.ScrollToItem(0);
            }
        }
        public void ListAssetsBySearch(bool scrollToTop = false)
        {
            if (m_allAssetsOfFilteredType.Count == 0) ListAssetsByType();

            //Debug.Log($"<color=yellow>Searching {m_allAssetsOfFilteredType.Count} assets.</color>");

            m_isSearchFiltering = true;
            m_searchFilteredList = m_allAssetsOfFilteredType.FindAll(SearchMatchesItem);
            m_searchFilteredList.Sort((x, y) => string.CompareOrdinal(x.Title, y.Title));

            //Debug.Log($"<color=green>Found {m_searchFilteredList.Count} matches.</color>"); 

            ListElement.itemsSource = m_searchFilteredList;
            ListElement.Refresh(); 
            if (scrollToTop) ListElement.ScrollToItem(0);
        }
        
        /// <summary>
        /// ONLY for use when you want something external to change the list selection.
        /// This will change the list index and subsequently trigger the internal method
        /// to fire the global changed event so everything else catches up.
        /// </summary>
        /// <param name="asset"></param>
        public void ChooseAssetExternally(DataEntity asset)
        {
            // fail out
            if (asset == null) return;
            if (asset == VaultDashboard.CurrentAsset) return;

            // set index
            int index = ListElement.itemsSource.IndexOf(asset);
            ListElement.selectedIndex = index;
            ListElement.ScrollToItem(index);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.CurrentAssetIndex, ListElement.selectedIndex);
        }

        private static bool SearchMatchesItem(DataEntity entity)
        {
            bool result = entity.Title.ToLower().Contains(VaultDashboard.AssetSearch.value.ToLower());
            return result;
        }
        /// <summary>
        /// ONLY for use when the list has chosen something.
        /// </summary>
        /// <param name="obj"></param>
        private void ChooseAssetInternal(object obj)
        {
            // fail
            if (ListElement.selectedIndex < 0) return;
            if (obj == null) return;
            if ((DataEntity)obj == VaultDashboard.CurrentAsset) return;

            // set index in prefs
            int index = ListElement.itemsSource.IndexOf(obj);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.CurrentAssetIndex, ListElement.selectedIndex);

            // broadcast change
            VaultDashboard.SetCurrentInspectorAsset((DataEntity)obj);
        }
#if UNITY_2020_3_OR_NEWER
        private void SelectAssetsInternal(IEnumerable<object> input)
        {
            List<object> objs = (List<object>)input;
#else
        private void SelectAssetsInternal(List<object> objs)
        {
#endif
            CurrentSelections = objs.ConvertAll(asset => (DataEntity)asset);
            StringBuilder sb = new StringBuilder();
            foreach (DataEntity assetFile in CurrentSelections)
            {
                sb.Append(AssetDatabase.GetAssetPath(assetFile) + "|");
            }

            VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SelectedAssetGuids, sb.ToString());
            ChooseAssetInternal(objs[0]);
        }
        private void GetSelectionPersistence()
        {
            string selected = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SelectedAssetGuids);
            if (string.IsNullOrEmpty(selected)) return;

            CurrentSelections = new List<DataEntity>();
            string[] split = selected.Split('|');
            foreach (string path in split)
            {
                if (path == string.Empty || path.Contains('|')) continue;
                CurrentSelections.Add(AssetDatabase.LoadAssetAtPath<DataEntity>(path));
            }
            VaultDashboard.CurrentAsset = CurrentSelections[0];
        }
         
        private void ListBindItem(VisualElement element, int listIndex)
        {
            // find the serialized property
            Editor ed = Editor.CreateEditor(m_isSearchFiltering ? m_searchFilteredList[listIndex] : m_allAssetsOfFilteredType[listIndex]);
            SerializedObject so = ed.serializedObject;
            SerializedProperty prop = so.FindProperty("Title");

            // build a prefix
            ((Label) element.ElementAt(0)).text = listIndex.ToString(" ▪ ");

            // bind the label to the serialized target target property title
            ((Label) element.ElementAt(1)).BindProperty(prop);
        }
        private static VisualElement ListMakeItem()
        {
            VisualElement selectableItem = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                    flexBasis = 1,
                    flexShrink = 1,
                    flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap)
                }
            };
            selectableItem.Add(new Label {name = "Prefix", text = "error", style = {unityFontStyleAndWeight = FontStyle.Bold}});
            selectableItem.Add(new Label {name = "DB Title", text = "unknown"});
            return selectableItem;
        }
    }
}