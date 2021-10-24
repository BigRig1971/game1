// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using Cleverous.VaultSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Cleverous.VaultInventory
{
    /// <summary>
    /// This class manages the UI side of the Inventory by listening to a target for OnChanged events and refreshing the UI slot that changed.
    /// </summary>
    public class InventoryUi : MonoBehaviour
    {
        public enum DragDropAction { Cancel, Move, Discard }

        [Header("References")]
        [Tooltip("The Grid Layout component that will host the Item Plugs in the UI.")]
        public GridLayoutGroup GridUi;
        [Tooltip("In order to put items in any of these slots the item must be one of these Restriction types.\n\nUsing None will always slot any item.")]
        [AssetDropdown(typeof(SlotRestriction))]
        public SlotRestriction[] Restrictions;
        [Tooltip("This will make the UI bind to the given interface when OnPlayerSpawn is triggered.\n\nCheck this only if this particular inventory ui is for the Player.")]
        public bool BindOnPlayerSpawn;

        [Header("Runtime Only")]
        public List<ItemUiPlug> Slots;
        public Inventory TargetInventory;
        /// <summary>
        /// A map for translating inventory indexes to grid slot indexes and vice versa. KEY is Inventory Array Index, VALUE is Grid UI Slot Index.
        /// </summary>
        public Dictionary<int, int> IndexMap;

        // STATIC STUFF
        public static ItemUiPlug DragOrigin;
        public static ItemUiPlug DragDestination;
        public static GameObject DragFloater;
        public static ItemUiPlug ClickedItem
        {
            get => m_clickedItem;
            set
            {
                m_clickedItem = value;
                OnClickedItemChanged?.Invoke();
            }
        }
        private static ItemUiPlug m_clickedItem;

        // STATIC EVENTS AND ACTIONS
        public static Action OnClickedItemChanged;

        // TODO 
        // Refactor as a single class that avoids using multiple IventoryUi components on separate grids.

        public virtual void Awake()
        {
            IndexMap = new Dictionary<int, int>();
            if (BindOnPlayerSpawn) VaultInventory.OnPlayerSpawn += LoadInventoryOfPlayer;
            VaultInventory.OnStartSceneChange += ClearUi;
        }

        public void LoadInventoryOfPlayer(IUseInventory avatar)
        {
            SetTargetInventory(avatar.Inventory);
        }
        public void SetTargetInventory(Inventory inv)
        {
            // Debug.Log($"<color=yellow>||| Inventory UI set target.</color>", this);

            if (inv == null)
            {
                Debug.LogError("Inventory UI tried to hook into target inventory but the Inventory is invalid!", this);
                return;
            }

            if (!inv.IsInitialized)
            {
                Debug.LogError("Inventory UI tried to hook into target inventory but the Inventory is not initialized!", this);
                return;
            }

            TargetInventory = inv;
            ClearUi();

            // Create new by looping through the target inventory slots
            for (int indexInv = 0; indexInv < inv.MaxSlots; indexInv++)
            {
                // compare each restriction for this UI grid to the target inventory's slot restriction on this index
                for (int restrictionId = 0; restrictionId < Restrictions.Length; restrictionId++)
                {
                    // if there's a match, then we can add a slot plug on this UI grid since its a restriction type we want to see here.
                    if (inv.Configuration.SlotRestrictions[indexInv] == Restrictions[restrictionId])
                    {
                        AddNewSlot(indexInv);
                    }
                }
            }

            // Subscribe new
            inv.OnChanged += UpdateUi;
            inv.OnDeath += ClearUi;
            inv.CmdRefreshAllFromServer();
        }

        private void AddNewSlot(int inventoryIndex)
        {
            GameObject x = Instantiate(VaultInventory.ItemSlotTemplate, GridUi.transform);
            ItemUiPlug comp = x.GetComponentInChildren<ItemUiPlug>();
            comp.ReferenceInventoryIndex = inventoryIndex;
            comp.Ui = this;
            Slots.Add(comp);
            int slotIndex = Slots.Count - 1;
            IndexMap.Add(inventoryIndex, slotIndex);
            // Debug.Log($"<color=cyan>||| Index {inventoryIndex} mapped to slot index {slotIndex}</color>");
            UpdateUi(inventoryIndex);
        }

        public void UpdateUi(int inventoryIndex)
        {
            // check for responsibilities. does this script handle this slot?
            if (!IndexMap.ContainsKey(inventoryIndex)) return;

            int slotIndex = IndexMap[inventoryIndex];
            Slots[slotIndex].UpdateUi(TargetInventory.Get(inventoryIndex), TargetInventory.Configuration.SlotRestrictions[inventoryIndex]);
        }

        public static void HandleDragEvent()
        {
            DragDropAction action = DetectDragDropAction();

            int originIndex = 0;
            int goalIndex = 0;

            if (DragDestination != null)
            {
                if (DragOrigin.Ui == null || DragOrigin.Ui.TargetInventory == null) return;
                for (int i = 0; i < DragOrigin.Ui.TargetInventory.netIdentity.NetworkBehaviours.Length; i++)
                {
                    if (DragOrigin.Ui.TargetInventory.netIdentity.NetworkBehaviours[i] == DragOrigin.Ui.TargetInventory)
                    {
                        originIndex = i;
                    }
                }

                if (DragDestination.Ui == null || DragDestination.Ui.TargetInventory == null) return;
                for (int i = 0; i < DragDestination.Ui.TargetInventory.netIdentity.NetworkBehaviours.Length; i++)
                {
                    if (DragDestination.Ui.TargetInventory.netIdentity.NetworkBehaviours[i] == DragDestination.Ui.TargetInventory)
                    {
                        goalIndex = i;
                    }
                }

                // Debug.Log($"<color=red>CLIENT REQUEST INVENTORY {action} ACTION - Origin is index {originIndex}, goal is index {goalIndex}</color>");

                switch (action)
                {
                    case DragDropAction.Cancel: 
                        break;
                    case DragDropAction.Move:
                        DragOrigin.Ui.TargetInventory.CmdRequestMove(
                            DragOrigin.Ui.TargetInventory.netId,
                            DragDestination.Ui.TargetInventory.netId,
                            DragOrigin.ReferenceInventoryIndex,
                            DragDestination.ReferenceInventoryIndex,
                            originIndex,
                            goalIndex);
                        break;

                    case DragDropAction.Discard:
                        DragOrigin.Ui.TargetInventory.DoDiscard(DragOrigin.ReferenceInventoryIndex);
                        break;

                    default: throw new ArgumentOutOfRangeException();
                }
            }

            if (DragFloater != null) Destroy(DragFloater);
            DragOrigin = null;
            DragDestination = null;
        }

        public static DragDropAction DetectDragDropAction()
        {
            if (DragOrigin != null && DragDestination != null) return DragDropAction.Move;
            if (DragOrigin != null && DragDestination == null) return DragDropAction.Discard;
            return DragDropAction.Cancel;
        }

        public void ClearUi()
        {
            if (TargetInventory != null) TargetInventory.OnChanged -= UpdateUi;

            foreach (ItemUiPlug x in Slots) Destroy(x.SlotOwnerObject);
            Slots = new List<ItemUiPlug>();
            IndexMap = new Dictionary<int, int>();
        }

        public void OnDisable()
        {
            if (DragFloater != null) Destroy(DragFloater);
        }
    }
}