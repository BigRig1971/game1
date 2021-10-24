// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Cleverous.VaultSystem;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Assembly = System.Reflection.Assembly;
using Object = UnityEngine.Object;

namespace Cleverous.VaultDashboard
{
    public class VaultDashboard : EditorWindow
    {
        // critical private
        private const string UxmlAssetName = "vault_dashboard_uxml";

        // critical public
        public static DataEntity CurrentAsset;
        public static Type CurrentTypeFilter;
        public static VaultTypeButton CurrentTypeButtonSelected;
        public static VaultAssetColumn AssetColumn;
        public static VaultAssetInspector AssetInspector;
        public static Historizer Historizer;

        public static ToolbarSearchField TypeSearch;
        public static ToolbarSearchField AssetSearch;
        public static List<Type> AllValidTypesCache;
        public static List<VaultTypeButton> AllButtonsCache;

        // actions
        public static Action OnTypeFilterChanged;
        public static Action OnCurrentAssetChanged;
        public static Action OnAssetSearch;
        public static Action OnTypeSearch;

        // local stuff
        /// <summary>
        /// <para>Vault will not read types from any assemblies starting with these prefixes.</para>
        /// <para>This is done to improve compile times by ignoring namespaces that will never
        /// contain a Type which inherits from DataEntity.</para>
        /// <para>Please check your assembly names if you aren't seeing content in the Type List.</para>
        /// </summary>
        private readonly string[] m_blacklist =
        {
            "System",
            "Mono.",
            "Unity.",
            "UnityEngine",
            "UnityEditor",
            "mscorlib",
            "SyntaxTree",
            "netstandard",
            "nunit"
        };
        protected static List<VaultTypeFoldoutEntry> TypeColumnElements;

        private string m_assetSearchCache;
        private string m_typeSearchCache;
        private string m_curTypeName;

        // assembly information
        protected static Assembly VaultAssy;
        protected static AssemblyName VaultAssyName;

        // wrappers for views
        protected static ScrollView TypeListWrapper;
        protected static VisualElement AssetListWrapper;
        protected static VisualElement InspectorWrapper;

        private static Button m_newButton;
        private static Button m_deleteButton;
        private static Button m_cloneButton;

        private static VaultDashboard m_editorWindow;

        [MenuItem("Tools/Cleverous/Vault Dashboard %#i", priority = 0)]
        public static void Open()
        {
            m_editorWindow = GetWindow<VaultDashboard>();
            m_editorWindow.minSize = new Vector2(850, 200);
        }
        public void OnEnable()
        { 
            Rebuild();
        }
        public void OnDisable()
        {
            AssemblyReloadEvents.afterAssemblyReload -= Rebuild;
        }
        public void OnGUI()
        {
            if (AssetSearch != null && AssetSearch.value != m_assetSearchCache)
            {
                m_assetSearchCache = AssetSearch.value;
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SearchAssets, m_assetSearchCache);
                OnAssetSearch?.Invoke();
            }

            if (TypeSearch != null && TypeSearch.value != m_typeSearchCache)
            {
                m_typeSearchCache = TypeSearch.value;
                VaultEditorSettings.SetString(VaultEditorSettings.VaultData.SearchType, m_typeSearchCache);
                OnTypeSearch?.Invoke();
            }

            // BUG required for some reason - state lost on domain reload
            if (CurrentTypeFilter != null && CurrentTypeFilter.IsAbstract) m_newButton.SetEnabled(false);
        }
        public void Rebuild()
        {
            if (m_editorWindow == null) Open();

            m_editorWindow.titleContent.text = "Vault Dashboard";
            m_editorWindow.Show();

            rootVisualElement.Clear();  
            LoadUxmlTemplate();

            TypeSearch.SetValueWithoutNotify(VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SearchType));
            m_typeSearchCache = TypeSearch.value;
 
            RefreshTypeColumn();
            FilterTypeColumn();

            AssetSearch.value = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.SearchAssets);
            m_assetSearchCache = AssetSearch.value;
             
            RefreshAssetColumn();

            InspectorWrapper.Clear();
            AssetInspector = new VaultAssetInspector();
            AssetInspector.ReDraw();
            InspectorWrapper.Add(AssetInspector);
        }

        public static void SetTypeFilter(Type t)
        {
            if (CurrentTypeFilter == t) return;

            CurrentTypeFilter = t;
            bool isEnabled = !t.IsAbstract;
            if (m_newButton.enabledSelf != isEnabled) m_newButton.SetEnabled(isEnabled);

            if (!Vault.IsReady) Vault.InitData();

            AssetSearch.value = string.Empty;
            OnTypeFilterChanged?.Invoke();
        }
        public static void SetCurrentInspectorAsset(DataEntity asset)
        {
            CurrentAsset = asset;
            OnCurrentAssetChanged?.Invoke();
        }
        public static void InspectAssetRemote(Object asset, Type t)
        {
            if (asset == null && t == null) return;
            if (t == null) return;

            GetWindow<VaultDashboard>().Focus();
            AssetSearch.SetValueWithoutNotify(string.Empty);

            VaultTypeButton button = TypeListWrapper.Q<VaultTypeButton>(t.Name);
            button.SetAsFilter();
            TypeListWrapper.ScrollTo(button);

            if (asset != null) AssetColumn.ChooseAssetExternally((DataEntity)asset);
        }

        /// <summary>
        /// Creates a new asset of the provided type, then focuses the dashboard on it.
        /// </summary>
        /// <param name="t">Type to create. Must derive from DataEntity.</param>
        /// <returns>The newly created asset object</returns>
        public static DataEntity NewAsset(Type t)
        {
            Vault.InitData();
            if (t == null) return null;

            const string prefix = "Data-";
            const string suffix = ".asset";
            string timeHash = Math.Abs(DateTime.Now.GetHashCode()).ToString();
            string filename = $"{prefix}{t.Name}-{timeHash}{suffix}";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{Vault.VaultItemPath}{filename}");

            ScriptableObject asset = CreateInstance(t);

            AssetDatabase.CreateAsset(asset, assetPathAndName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            DatabaseBuilder.BuildDatabase();

            DataEntity real = (DataEntity)AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathAndName);

            AssetSearch.SetValueWithoutNotify(string.Empty);
            if (t != CurrentTypeFilter) SetTypeFilter(t);

            AssetColumn.Rebuild();

            int i = Mathf.Clamp(AssetColumn.ListElement.itemsSource.IndexOf(real), 0, AssetColumn.ListElement.itemsSource.Count);
            VaultEditorSettings.SetInt(VaultEditorSettings.VaultData.CurrentAssetIndex, i);
            AssetColumn.ChooseAssetExternally(real);
            AssetColumn.ListElement.ScrollToItem(i);

            return real;
        }

        /// <summary>
        /// Creates a new asset of the current type.
        /// </summary>
        public static void NewAsset()
        {
            NewAsset(CurrentTypeFilter);
        }
        public static void CloneAsset() 
        {
            if (CurrentTypeFilter == null) return;

            const string prefix = "Data-";
            const string suffix = ".asset";
            string timeHash = Math.Abs(DateTime.Now.GetHashCode()).ToString();
            string filename = $"{prefix}{CurrentAsset.GetType().Name}-{timeHash}{suffix}";
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath($"{Vault.VaultItemPath}{filename}");

            AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(CurrentAsset), assetPathAndName);
            DataEntity newEntity = AssetDatabase.LoadAssetAtPath<DataEntity>(assetPathAndName);
            newEntity.Title += " (CLONED)";
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            DataEntity real = (DataEntity)AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPathAndName);
            AssetSearch.SetValueWithoutNotify(string.Empty);
            AssetColumn.Rebuild();

            int i = Mathf.Clamp(AssetColumn.ListElement.itemsSource.IndexOf(real), 0, AssetColumn.ListElement.itemsSource.Count);
            AssetColumn.ListElement.ScrollToItem(i);
            AssetColumn.ListElement.selectedIndex = i;
        }
        public static void DeleteAsset()
        {
            StringBuilder sb = new StringBuilder();
            if (AssetColumn.CurrentSelections.Count == 0) return;

            foreach (DataEntity asset in AssetColumn.CurrentSelections)
            {
                if (asset == null) continue;
                sb.Append(asset.Title + "\n");
            }
            bool confirm = EditorUtility.DisplayDialog("Purge warning!", $"Delete assets from the disk?\n\n{sb}", "Yes", "Cancel");
            if (!confirm) return;

            AssetInspector.InspectNothing();
            foreach (DataEntity asset in AssetColumn.CurrentSelections)
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            }
            CurrentAsset = null;
            AssetColumn.Rebuild();
        }

        private void LoadUxmlTemplate()
        {   
            // load uxml and elements
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>(UxmlAssetName);
            visualTree.CloneTree(rootVisualElement);

            // find important parts and reference them
            TypeListWrapper = rootVisualElement.Q<ScrollView>("TYPE_COLUMN");
            AssetListWrapper = rootVisualElement.Q<VisualElement>("ASSET_COLUMN");
            InspectorWrapper = rootVisualElement.Q<VisualElement>("INSPECT_COLUMN");
            TypeSearch = rootVisualElement.Q<ToolbarSearchField>("TYPE_SEARCH");
            AssetSearch = rootVisualElement.Q<ToolbarSearchField>("ASSET_SEARCH");
            Historizer = new Historizer();
            rootVisualElement.Q<VisualElement>("TB_HISTORY").Add(Historizer);

            m_newButton = AssetListWrapper.Q<Button>("BUTTON_NEW");
            m_deleteButton = AssetListWrapper.Q<Button>("BUTTON_DELETE");
            m_cloneButton = AssetListWrapper.Q<Button>("BUTTON_CLONE");

            m_newButton.clicked += NewAsset;
            m_deleteButton.clicked += DeleteAsset;
            m_cloneButton.clicked += CloneAsset;
            OnTypeSearch += FilterTypeColumn;

            rootVisualElement.Q<ToolbarButton>("TB_RELOAD").clicked += Rebuild;
            rootVisualElement.Q<ToolbarButton>("TB_HELP").clicked += Help;

            // reload assembly things
            VaultAssy = Assembly.GetAssembly(typeof(DataEntity));
            VaultAssyName = VaultAssy.GetName();
        }
        private void RefreshTypeColumn()
        {
            AllValidTypesCache = new List<Type>();
            AllButtonsCache = new List<VaultTypeButton>();

            TypeColumnElements?.Clear();
            TypeColumnElements = new List<VaultTypeFoldoutEntry>();
            
            foreach (Assembly assy in AppDomain.CurrentDomain.GetAssemblies())
            {
                string assyName = assy.GetName().Name;
                if (assy.IsDynamic) continue; // ignore dynamics
                if (m_blacklist.Any(ignored => assyName.StartsWith(ignored))) continue; // ignore blacklisted
                if (TypeColumnElements.Any(n => n.AssemblyName == assyName)) continue; // ignore duplicates

                // ** ASSEMBLY LEVEL
                foreach (AssemblyName q in assy.GetReferencedAssemblies())
                {
                    if (q.Name != VaultAssyName.Name) continue; // ignore anything not explicitly referencing Vault.
                    ProcessAssembly(assy, q, assyName);
                }
            }

            // Include the Vault assembly, which doesn't reference itself and wouldn't normally be included.
            ProcessAssembly(VaultAssy, VaultAssyName, VaultAssyName.Name);

            // alphabetically sort the list
            TypeColumnElements.Sort((x, y) => string.CompareOrdinal(x.AssemblyName, y.AssemblyName));

            // 006) add everything in the list to the wrapper so it can be seen.
            foreach (VaultTypeFoldoutEntry e in TypeColumnElements)
            { 
                TypeListWrapper.Add(e);
            }

            // persistence
            m_curTypeName = VaultEditorSettings.GetString(VaultEditorSettings.VaultData.CurrentTypeName);
            foreach (VaultTypeFoldoutEntry x in TypeColumnElements)
            {
                VaultTypeButton result = x.Q<VaultTypeButton>(m_curTypeName);
                if (result == null) continue;

                result.SetAsFilter();
                break;
            }
        }

        private void ProcessAssembly(Assembly assy, AssemblyName q, string assyName)
        {
            bool includeAssyInView = false;

            VaultTypeFoldoutEntry assyFoldout = new VaultTypeFoldoutEntry(assy.GetName().Name);
            assyFoldout.viewDataKey = assy.GetName().Name;

            IEnumerable<IGrouping<string, Type>> groups = assy.GetExportedTypes()
                .Where(t => t.IsSubclassOf(typeof(DataEntity)) || t == typeof(DataEntity))
                .GroupBy(t => t.Namespace);

            // ** NAMESPACE LEVEL
            foreach (IGrouping<string, Type> namespaceGroup in groups)
            {
                // this switch is to handle when the Assembly is the same name
                // as the Namespace. Using AsmDefs this is 100% of the time.
                // This will just skip creating a redundant folder for the NS.
                // Minor code duplication...
                if (namespaceGroup.Key == assyName)
                {
                    // ** TYPE LEVEL
                    foreach (Type type in namespaceGroup)
                    {
                        includeAssyInView = true;
                        VaultTypeButton b = assyFoldout.AddTypeButton(type, assyFoldout);
                        AllValidTypesCache.Add(type);
                        AllButtonsCache.Add(b);
                    }
                }
                else
                {
                    VaultTypeFoldoutEntry namespaceFoldout = new VaultTypeFoldoutEntry($"{namespaceGroup.Key ?? "Global Namespace"} ({namespaceGroup.Count()})");
                    namespaceFoldout.viewDataKey = namespaceGroup.Key;
                    assyFoldout.Add(namespaceFoldout);

                    // ** TYPE LEVEL
                    foreach (Type type in namespaceGroup)
                    {
                        includeAssyInView = true;
                        VaultTypeButton b = namespaceFoldout.AddTypeButton(type, namespaceFoldout);
                        AllValidTypesCache.Add(type);
                        AllButtonsCache.Add(b);
                    }
                }
            }

            if (includeAssyInView) TypeColumnElements.Add(assyFoldout);
        }
        private void RefreshAssetColumn()
        {
            AssetColumn = new VaultAssetColumn();
            AssetListWrapper.Add(AssetColumn);
        }

        private void FilterTypeColumn()
        {
            if (string.IsNullOrEmpty(TypeSearch.value))
            {
                foreach (VaultTypeButton x in AllButtonsCache)
                {
                    x.SetVisible(true);
                } 
            }
            else
            {
                foreach (VaultTypeButton x in AllButtonsCache)
                {
                    x.SetVisible(x.name.ToLower().Contains(TypeSearch.value.ToLower()));
                }
            }
        }

        public static void Help()
        {
            Application.OpenURL("https://app.gitbook.com/@lanefox/s/vault/");
        }
    }

    public class VaultTypeFoldoutEntry : Foldout
    {
        public string AssemblyName;
        public List<Type> OwnedTypes;

        public VaultTypeFoldoutEntry(string assemblyName)
        {
            AssemblyName = assemblyName;
            text = assemblyName;
            style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            value = false;
            OwnedTypes = new List<Type>(); 
        }

        public VaultTypeButton AddTypeButton(Type t, Foldout parentFoldout)
        {
            OwnedTypes.Add(t);
            string label = t.Name;

            VisualElement wrapper = new VisualElement();
            wrapper.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            wrapper.style.alignItems = new StyleEnum<Align>(Align.Stretch);
            wrapper.style.justifyContent = new StyleEnum<Justify>(Justify.FlexStart);

            VaultTypeButton interactButton = new VaultTypeButton(t, parentFoldout);
            interactButton.text = label;
            interactButton.clicked += interactButton.SetAsFilter;
            interactButton.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft);
            interactButton.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Normal);
            interactButton.style.flexGrow = 1;

            interactButton.style.borderBottomLeftRadius = 0;
            interactButton.style.borderBottomRightRadius = 0;
            interactButton.style.borderTopLeftRadius = 0;
            interactButton.style.borderTopRightRadius = 0;

            Label prefix = new Label(t.IsAbstract ? "○" : "●");
            prefix.style.color = t.IsAbstract ? Color.gray : Color.green;

            if (interactButton.SourceType == VaultDashboard.CurrentTypeFilter) interactButton.SetAsFilter();

            wrapper.Add(prefix);
            wrapper.Add(interactButton);
            Add(wrapper);

            return interactButton;
        }

        public void Reveal()
        {
            value = true;
        } 
    }
    public class VaultTypeButton : Button
    {
        public Type SourceType;
        public Foldout Foldout;

        private static Color InactiveColor  => EditorGUIUtility.isProSkin ? DarkInactive : LightInactive;
        private static Color ActiveColor    => EditorGUIUtility.isProSkin ? DarkActive : LightActive;

        private static readonly Color LightInactive     = new Color(0.8941177f, 0.8941177f, 0.8941177f);
        private static readonly Color LightActive       = new Color(0.5664399f, 0.8584906f, 0.3644536f);
        private static readonly Color DarkInactive      = new Color(0.3647059f, 0.3647059f, 0.3647059f);
        private static readonly Color DarkActive        = new Color(0.1602882f, 0.3647059f, 0.1568235f);

        public VaultTypeButton(Type sourceType, Foldout parentFoldout)
        {
            Foldout = parentFoldout;
            name = sourceType.Name;
            viewDataKey = sourceType.Name;
            SourceType = sourceType;
            style.backgroundColor = new StyleColor(InactiveColor);
        }

        public void SetVisible(bool state)
        {
            if (state)
            {
                Foldout.value = true;
                parent.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.Flex);
            }
            else
            {
                parent.style.display = new StyleEnum<DisplayStyle>(DisplayStyle.None);
            }
        }
        public void SetAsFilter()
        {
            if (VaultDashboard.CurrentTypeButtonSelected != null)
            {
                VaultDashboard.CurrentTypeButtonSelected.style.backgroundColor = InactiveColor;
            }

            style.backgroundColor = ActiveColor;
            VaultEditorSettings.SetString(VaultEditorSettings.VaultData.CurrentTypeName, SourceType.Name);
            VaultDashboard.CurrentTypeButtonSelected = this;
            VaultDashboard.SetTypeFilter(SourceType);
        }
    }
}