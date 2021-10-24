// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections.Generic;
using Cleverous.VaultSystem;
using Mirror;
using UnityEngine;

namespace Cleverous.VaultInventory
{
    /// <summary>
    /// This class manages the List side of the inventory and fires events when the list changes.
    /// </summary>
    public partial class Inventory : NetworkBehaviour
    {
        public bool IsInitialized { get; private set; }
        public IUseInventory Owner { get; set; }
        public Action<int> OnChanged;
        public Action OnDeath;
        public Action<int> OnNewItemAdded;

        [AssetDropdown(typeof(InventoryConfig))]
        public InventoryConfig Configuration;

        protected List<RootItemStack> Content { get; set; }

        protected List<SlotRestriction> Restrictions;
        public int MaxSlots
        {
            get => m_maxSlots;
            set
            {
                if (value < m_maxSlots || value == m_maxSlots) return;

                int oldCount = m_maxSlots;
                m_maxSlots = value;
                for (int i = oldCount; i < m_maxSlots; i++)
                {
                    Content.Add(null);
                    OnChanged?.Invoke(i);
                }
            }
        }
        private int m_maxSlots;

        /// <summary>
        /// Initialize the inventory.
        /// </summary>
        /// <param name="owner">The interface that owns this Inventory. If no one initializes it, then it doesn't work.</param>
        /// <param name="config">The slot count and slot restrictions for the inventory.</param>
        public virtual void Initialize(IUseInventory owner, InventoryConfig config = null)
        {
            // setup config
            if (config == null && Configuration != null) config = Configuration;
            else if (config != null) Configuration = config;
            else config = new InventoryConfig();
            m_maxSlots = config.SlotRestrictions.Length > 0 ? config.SlotRestrictions.Length : 1;
            
            // new list
            Content = new List<RootItemStack>();

            // list must always be at capacity and all empty nodes must be null.
            for (int i = 0; i < config.SlotRestrictions.Length; i++) Content.Add(null); 
            Owner = owner;

            // setup slot restrictions
            Restrictions = new List<SlotRestriction>();
            for (int i = 0; i < config.SlotRestrictions.Length; i++) Restrictions.Add(null); // add default values
            if (config.SlotRestrictions != null)
            {
                for (int i = 0; i < config.SlotRestrictions.Length; i++) Restrictions[i] = config.SlotRestrictions[i];
            }

            IsInitialized = true;
            // Debug.Log("<color=green>Initialized Inventory</color>", this);
        }

        public override void OnStopServer()
        {
            m_maxSlots = 0;
            Content = new List<RootItemStack>();
            Restrictions = new List<SlotRestriction>();
            IsInitialized = false;
        }        
        public override void OnStopClient()
        {
            m_maxSlots = 0;
            Content = new List<RootItemStack>();
            Restrictions = new List<SlotRestriction>();
            IsInitialized = false;
        }

        // Public Queries
        /// <summary>
        /// Read the content at the given index.
        /// </summary>
        /// <param name="index">The index to get</param>
        /// <returns>The content of this Inventory at the given index</returns>
        public virtual RootItemStack Get(int index)
        {
            return Content[index] == null ? new RootItemStack(null, 0) : Content[index];
        }

        /// <summary>
        /// Searches the Inventory to see if it has an amount of a specific item by matching title.
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <param name="amount">The minimum amount required to return true</param>
        /// <returns>True if the Inventory contains at least the amount of items provided. False otherwise.</returns>
        public virtual bool Contains(RootItem item, int amount)
        {
            List<int> indices = new List<int>();
            int verifiedCount = 0;

            for (int i = 0; i < MaxSlots; i++)
            {
                if (Get(i) == null) continue;
                if (Get(i).Source.Title == item.Title)
                {
                    indices.Add(i);
                }
            }

            for (int i = 0; i < indices.Count; i++)
            {
                verifiedCount += Get(indices[i]).StackSize;
            }

            return verifiedCount >= amount;
        }

        /// <summary>
        /// Get all slots with the matching restriction
        /// </summary>
        /// <param name="restriction"></param>
        /// <returns>All indexes that match the provided SlotRestriction</returns>
        public virtual int[] GetAllSlotIndexOfType(SlotRestriction restriction)
        {
            List<int> results = new List<int>();
            for (int i = 0; i < Configuration.SlotRestrictions.Length; i++)
            {
                if (Configuration.SlotRestrictions[i] == restriction) results.Add(i);
            }
            return results.ToArray();
        }

        /// <summary>
        /// Get the first slot with the matching restriction
        /// </summary>
        /// <param name="restriction"></param>
        /// <param name="mustBeEmpty">If true, only empty slots are returned.</param>
        /// <returns>The first found index that matches the provided SlotRestriction. If no match is found then -1 is returned.</returns>
        public virtual int GetFirstSlotIndexOfType(SlotRestriction restriction, bool mustBeEmpty)
        {
            for (int i = 0; i < Configuration.SlotRestrictions.Length; i++)
            {
                if (Configuration.SlotRestrictions[i] == restriction)
                {
                    if (mustBeEmpty && Content[i] == null) return i;
                    if (!mustBeEmpty) return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Count how many of the given item are in this inventory.
        /// </summary>
        /// <param name="item">The item you want to count.</param>
        /// <returns>Available stack size of the item in the inventory.</returns>
        public virtual int GetCountOfItem(RootItem item)
        {
            return GetItemCount(item.Title);
        }

        /// <summary>
        /// Count how many of the given item are in this inventory.
        /// </summary>
        /// <param name="itemTitle">The Title of the item you want to count.</param>
        /// <returns>Available stack size of the item in the inventory.</returns>
        public virtual int GetCountOfItem(string itemTitle)
        {
            return DoCount(itemTitle);
        }

        /// <summary>
        /// Count the number of slots with no content in them.
        /// </summary>
        /// <returns>The number of empty slots in this Inventory.</returns>
        public virtual int GetEmptySlotCount()
        {
            int emptyCount = 0;
            foreach (RootItemStack x in Content)
            {
                if (x == null) emptyCount++;
            }
            return emptyCount;
        }

        // Client Requests
        /// <summary>
        /// Client requests to move an item in this inventory from slot A to B.
        /// </summary>
        /// <param name="originObjNetId">The NetId of the originating Inventory</param>
        /// <param name="goalObjNetId">The NetId of the target/goal Inventory</param>
        /// <param name="fromSlotIndex">The index of the item in the originating inventory</param>
        /// <param name="toSlotIndex">The index of the item in the target/goal inventory</param>
        /// <param name="originBehaviorIndex">The index of the Behavior of the originating Inventory within its owning NetworkIdentity's NetworkBehaviours list.</param>
        /// <param name="goalBehaviorIndex">The index of the Behavior of the target/goal Inventory within its owning NetworkIdentity's NetworkBehaviours list.</param>
        [Command] public virtual void CmdRequestMove(uint originObjNetId, uint goalObjNetId, int fromSlotIndex, int toSlotIndex, int originBehaviorIndex, int goalBehaviorIndex)
        {
            // Debug.Log($"<color=red>SERVER Origin is index {originBehaviorIndex}, goal is index {goalBehaviorIndex}</color> and action is Move");
            if (!IsInitialized) return;

            Inventory originInventory = (Inventory)NetworkIdentity.spawned[originObjNetId].NetworkBehaviours[originBehaviorIndex];
            Inventory goalInventory = (Inventory)NetworkIdentity.spawned[goalObjNetId].NetworkBehaviours[goalBehaviorIndex];

            DoMove(originInventory, goalInventory, fromSlotIndex, toSlotIndex);
        }

        /// <summary>
        /// Send a specific slot update to the client.
        /// </summary>
        /// <param name="index">Which index the client wants an update on.</param>
        [Command] public virtual void CmdRefreshSlotFromServer(int index)
        {
            int vaultIndex;
            int size;

            if (Content[index] != null)
            {
                vaultIndex = Vault.GetIndex(Content[index].Source.Title);
                size = Content[index].StackSize;
            }
            else
            {
                vaultIndex = -1;
                size = 0;
            }

            RpcRemoteUpdateSlot(index, vaultIndex, size, false);
        }

        /// <summary>
        /// Send the entire inventory to the client. Does not require authority, could be and expoitable thing if contents are considered private/sensitive.
        /// </summary>
        [Command(requiresAuthority = false)] public virtual void CmdRefreshAllFromServer()
        {
            // Debug.Log($"<color=orange>||||||||||||||||||||||| Client Requested Full Inventory Refresh ||||||||||||||||||||||| </color>");

            for (int i = 0; i < Content.Count; i++)
            {
                int vaultIndex;
                int size;

                if (Content[i] != null)
                {
                    vaultIndex = Vault.GetIndex(Content[i].Source.Title);
                    size = Content[i].StackSize;
                }
                else
                {
                    vaultIndex = -1;
                    size = 0;
                }

                // Debug.Log($"<color=orange>... SERVER sending update for slot {i}, item id {vaultIndex}, count {size}</color>");
                RpcRemoteUpdateSlot(i, vaultIndex, size, false);
            }
        }

        /// <summary>
        /// Client requests to split a stack of items.
        /// </summary>
        /// <param name="index">The index of the item in this inventory.</param>
        [Command] public virtual void CmdRequestSplit(int index)
        {
            DoSplit(index);
        }

        /// <summary>
        /// Client requests the item stack at index be dropped into the world.
        /// </summary>
        /// <param name="index">The item index in this inventory.</param>
        [Command] public virtual void CmdRequestDrop(int index)
        {
            DoDiscard(index);
        }



        // Server Processes
        /// <summary>
        /// SILENTLY replaces a content index to the provided RootItemStack. Generally not used, other Do[xyz] methods should be used. Only use if you know exactly why you should be setting the item silently.
        /// </summary>
        /// <param name="index">The content index to set</param>
        /// <param name="item">The RootItemStack to put in the index</param>
        [Server] public virtual void Set(int index, RootItemStack item)
        {
            Content[index] = item;
        }

        /// <summary>
        /// Immediately add an item to this inventory, if possible.
        /// </summary>
        /// <param name="item">What item to add.</param>
        /// <param name="tryToMerge">Try to merge into existing inventory stacks?</param>
        /// <returns>The amount that could *not* be added. A return of 0 is total success, the amount was fully added.</returns>
        [Server] public virtual int DoAdd(RootItemStack item, bool tryToMerge = true)
        {
            if (!IsInitialized)
            {
                Debug.LogWarning("Cannot Add to an uninitialized inventory.", this);
                return -1;
            }
            if (item == null || item.Source == null)
            {
                return -1;
            }
            if (item.StackSize == 0) item.StackSize = 1;
           
            int hotStack = item.StackSize;

            #region Merge Path
            if (tryToMerge && item.Source.MaxStackSize > 1)
            {
                bool merging = true;
                while (merging)
                {
                    if (hotStack <= 0) return 0;

                    // Try to find a suitable slot to merge into, exit this loop if we can't.
                    int mergeIndex = GetIndexOfItemNotFull(item.Source.Title);
                    if (mergeIndex == -1)
                    {
                        merging = false;
                        continue;
                    }

                    // If found a matching slot, see how much doesn't fit.
                    int remain = GetMergeRemainder(
                        hotStack,
                        Content[mergeIndex].StackSize,
                        Content[mergeIndex].Source.MaxStackSize);

                    if (remain > 0) Content[mergeIndex].StackSize = Content[mergeIndex].Source.MaxStackSize;
                    else Content[mergeIndex].StackSize += hotStack;

                    // Update live stack and flag a change on this index.
                    hotStack = remain;
                    //OnChanged?.Invoke(mergeIndex);
                    RpcRemoteUpdateSlot(mergeIndex, Vault.GetIndex(Content[mergeIndex].Source.Title), Content[mergeIndex].StackSize, true);
                }
            }
            #endregion

            #region Open Slot Path
            while (true)
            {
                // Find the first open slot. If there isn't one, or there's no amount left, we're done.
                int openSlot = GetValidNullIndex(item.Source.Restriction);
                if (openSlot == -1 || hotStack <= 0) return hotStack;

                // Otherwise we can add a fresh new item to the list.
                Content[openSlot] = item;
                Content[openSlot].StackSize = 0;

                // And figure out if it fits. (duh, it does, but whatever, maybe StackSize > MaxStackSize because reasons?)
                int remain = GetMergeRemainder(
                    hotStack,
                    Content[openSlot].StackSize,
                    Content[openSlot].Source.MaxStackSize);

                // Then fill it up
                Content[openSlot].StackSize = remain > 0
                    ? Content[openSlot].Source.MaxStackSize
                    : hotStack;

                // And update the hot stack, flag a change and try another loop
                hotStack = remain;
                //OnChanged?.Invoke(openSlot);
                RpcRemoteUpdateSlot(openSlot, Vault.GetIndex(Content[openSlot].Source.Title), Content[openSlot].StackSize, true);
            }

            #endregion
        }

        /// <summary>
        /// Immediately move an item from one slot to another slot, even between <see cref="Inventory"/> classes.
        /// </summary>
        /// <param name="origin">Origin inventory, where the item is coming from.</param>
        /// <param name="goal">Goal/target inventory the item will be moved to.</param>
        /// <param name="fromIndex">The item's index in the Origin Inventory.</param>
        /// <param name="toIndex">The target index in the Goal Inventory</param>
        /// <returns>Any remainder count of items that could *not* be moved for some reason. Zero is flawless victory.</returns>
        [Server] public virtual int DoMove(Inventory origin, Inventory goal, int fromIndex, int toIndex)
        {
            if (!IsInitialized) return -1;
            if (origin == goal && fromIndex == toIndex) return -1;
            if (origin.Content[fromIndex] == null) return -1;

            // try merge - is goal empty?
            if (goal.Content[toIndex] != null)
            {
                // not empty apparently, so are they different items? if so we can try to swap them.
                if (origin.Content[fromIndex].Source != goal.Content[toIndex].Source)
                {
                    // is the goal slot restriction None or Same as the origin item going there?
                    if (goal.Restrictions[toIndex] != null &&
                        goal.Restrictions[toIndex] != origin.Content[fromIndex].Source.Restriction)
                    {
                        return -1;
                    }

                    // is the origin slot restriction None or Same as goal item going there?
                    if (origin.Restrictions[fromIndex] != null &&
                        origin.Restrictions[fromIndex] != goal.Content[toIndex].Source.Restriction)
                    {
                        return -1;
                    }

                    RootItemStack originCache = origin.Content[fromIndex];
                    RootItemStack goalCache = goal.Content[toIndex];

                    goal.Content[toIndex] = originCache;
                    origin.Content[fromIndex] = goalCache;

                    // for client
                    goal.RpcRemoteUpdateSlot(toIndex, Vault.GetIndex(goal.Content[toIndex].Source), goal.Content[toIndex].StackSize, false);
                    origin.RpcRemoteUpdateSlot(fromIndex, Vault.GetIndex(origin.Content[fromIndex].Source), origin.Content[fromIndex].StackSize, false);

                    return 0;
                }

                // alright, apparently not empty - but they're the same type! sooo see how many are left if origin stack merged into goal stack
                int remain = GetMergeRemainder(
                    origin.Content[fromIndex].StackSize,
                    goal.Content[toIndex].StackSize,
                    goal.Content[toIndex].Source.MaxStackSize);

                // they're the same type, so simply set the stack sizes on both slots
                if (remain <= 0)
                {
                    // if theres none remaining, the merge was 100% success
                    goal.Content[toIndex].StackSize += origin.Content[fromIndex].StackSize;
                    origin.Content[fromIndex] = null;

                    goal.RpcRemoteUpdateSlot(toIndex, Vault.GetIndex(goal.Content[toIndex].Source.Title), goal.Content[toIndex].StackSize, false);
                    origin.RpcRemoteUpdateSlot(fromIndex, -1, 0, false);
                    
                    return 0;
                }

                // otherwise, there is a remainder so max out the goal stack and set the origin stack size to the remainder.
                goal.Content[toIndex].StackSize = goal.Content[toIndex].Source.MaxStackSize;
                origin.Content[fromIndex].StackSize = remain;

                // for client
                goal.RpcRemoteUpdateSlot(toIndex, Vault.GetIndex(goal.Content[toIndex].Source.Title), goal.Content[toIndex].StackSize, false);
                origin.RpcRemoteUpdateSlot(fromIndex, Vault.GetIndex(origin.Content[fromIndex].Source.Title), origin.Content[fromIndex].StackSize, false);

                return remain;
            }

            // otherwise, direct move to empty cell.
            // does the slot accept this property?
            if (goal.Restrictions[toIndex] == null || goal.Restrictions[toIndex] == origin.Content[fromIndex].Source.Restriction)
            {
                goal.Content[toIndex] = origin.Content[fromIndex];
                origin.Content[fromIndex] = null;
                
                // for server
                //origin.OnChanged?.Invoke(fromIndex);
                //goal.OnChanged?.Invoke(toIndex);

                // for client
                goal.RpcRemoteUpdateSlot(toIndex, Vault.GetIndex(goal.Content[toIndex].Source.Title), goal.Content[toIndex].StackSize, false);
                origin.RpcRemoteUpdateSlot(fromIndex, -1, 0, false);
                return 0;
            }

            return origin.Content[fromIndex].StackSize;
        }

        /// <summary>
        /// Immediately remove, destroy and obliterate an item from this <see cref="Inventory"/>.
        /// </summary>
        /// <param name="index">The index you want to remove, destroy and obliterate.</param>
        /// <returns>Any remainder, but always zero. This can't fail unless the index is out of range.</returns>
        [Server] public virtual int DoErase(int index)
        {
            if (!IsInitialized) return -1;

            Content[index] = null;
            OnChanged?.Invoke(index);
            RpcRemoteUpdateSlot(index, -1, 0, false);
            return 0;
        }

        /// <summary>
        /// Immediately finds item's matching the title and remove an amount. Will scan entire inventory first.
        /// </summary>
        /// <param name="itemTitle">The title of the item.</param>
        /// <param name="amountToRemove">The stack size or amount/number of items to remove.</param>
        /// <returns>Any amount that could *not* be removed. Zero is flawless victory.</returns>
        [Server] public virtual int DoTake(string itemTitle, int amountToRemove)
        {
            if (!IsInitialized) return -1;

            //Debug.Log($"<color=red>Taking {amountToRemove} {itemTitle}</color>");
            int itemIndex = Vault.GetIndex(itemTitle);
            int removedSoFar = 0;
            int[] contentIndexes = GetAllIndexOfItem(itemTitle);
            foreach (int index in contentIndexes)
            {
                if (removedSoFar >= amountToRemove) break;
                int amountRemainingToRemove = amountToRemove - removedSoFar;

                // if the stack has enough for the rest...
                if (Content[index].StackSize > amountRemainingToRemove)
                {
                    Content[index].StackSize -= amountRemainingToRemove;
                    OnChanged?.Invoke(index);
                    RpcRemoteUpdateSlot(index, itemIndex, Content[index].StackSize, false);
                    removedSoFar = amountToRemove;
                    break;
                }

                // otherwise, nuke that slot.
                removedSoFar += Content[index].StackSize;
                DoErase(index);
            }

            return amountToRemove - removedSoFar;
        }

        /// <summary>
        /// Immediately count the number of a specific item in this <see cref="Inventory"/>.
        /// </summary>
        /// <param name="itemTitle">The item to look for</param>
        /// <returns>The combined stack size total of all items of the given name in the <see cref="Inventory"/>.</returns>
        [Server] public virtual int DoCount(string itemTitle)
        {
            if (!IsInitialized) return -1;

            return GetItemCount(itemTitle);
        }

        /// <summary>
        /// Immediately discard, or 'drop' an item by spawning it's Art Prefab and removing it's data from the <see cref="Inventory"/> class.
        /// </summary>
        /// <param name="index">Which index in this inventory to discard.</param>
        /// <returns>The spawned RuntimeItemProxy component</returns>
        [Server] public virtual RuntimeItemProxy DoDiscard(int index)
        {
            if (!IsInitialized) return null;

            // spawn new
            RuntimeItemProxy result = VaultInventory.SpawnWorldItem(Content[index].Source, Owner.MyTransform.position, Content[index].StackSize);

            // kill, flag and return.
            Content[index] = null;
            //OnChanged?.Invoke(index);
            RpcRemoteUpdateSlot(index, -1, 0, false);
            return result;
        }

        /// <summary>
        /// Immediately split a stack in half. One empty slot is required. TODO rounding.
        /// </summary>
        /// <param name="index">Index of this <see cref="Content"/> to split.</param>
        /// <returns>TRUE if successfully split and created a new item with half the stack size, FALSE if failure and no action was taken.</returns>
        [Server] public virtual bool DoSplit(int index)
        {
            if (!IsInitialized) return false;

            // Is there room?
            int openSlot = GetValidNullIndex(Content[index].Source.Restriction);
            if (openSlot == -1)
            {
                Debug.LogError("Inventory has no space to split stack");
                return false;
            }

            // Figure out the new slot size.
            int newSlotSize = Content[index].StackSize / 2;
            
            // Try adding it.
            int oudex = DoAdd(new RootItemStack(Content[index].Source, newSlotSize), false);
            if (oudex == -1) return false;

            // Success, so subtract the new slot amount from the original stack.
            Content[index].StackSize -= newSlotSize;

            //OnChanged?.Invoke(index);

            // RPC update (only old slot, new slot was RPC'd via DoAdd() method)
            int itemId = Vault.GetIndex(Content[index].Source);
            RpcRemoteUpdateSlot(index, itemId, Content[index].StackSize, false);
            return true;
        }



        // Server RPC completions. Every client **including the server client** gets this call. If you're trying to run headless pure server, uncomment the OnChanged?.Invoke code in the [Server] methods.
        [ClientRpc] public virtual void RpcRemoteUpdateSlot(int index, int itemId, int amount, bool isNew)
        {
            // Debug.Log($"<color=green>Inventory || Received RpcUpdateSlot({index}, {itemId}, {amount}) for {gameObject.name}</color>", this);
            if (amount == 0 || itemId == -1) Content[index] = null;
            else
            {
                RootItemStack item = new RootItemStack((RootItem)Vault.Get(itemId), amount);
                Content[index] = item;
                Content[index].StackSize = amount;
            }
            OnChanged?.Invoke(index);
            if (isNew) OnNewItemAdded?.Invoke(index);
        }



        // Internal Gets
        /// <summary>
        /// Looks for the first open slot with the matching restriction and returns it's index.
        /// </summary>
        /// <param name="restriction">The slot restriction.</param>
        /// <returns>The index of the slot found, if any. returning -1 on failure.</returns>
        protected virtual int GetValidNullIndex(SlotRestriction restriction)
        {
            for (int i = 0; i < Content.Count; i++)
            {
                if (Content[i] == null && (Restrictions[i] == restriction || Restrictions[i] == null)) return i;
            }
            return -1;
        }

        /// <summary>
        /// Looks for an item in this inventory by string title.
        /// </summary>
        /// <param name="itemTitle">The title of the item.</param>
        /// <param name="startIndex">The index to start searching from.</param>
        /// <returns>The index of an item that matches the title parameter</returns>
        protected virtual int GetIndexOfItem(string itemTitle, int startIndex = 0)
        {
            for (int i = startIndex; i < Content.Count; i++)
            {
                if (Content[i] == null) continue;
                if (Content[i].Source.Title == itemTitle) return i;
            }
            return -1;
        }

        /// <summary>
        /// Find every index that contains the item with a given title.
        /// </summary>
        /// <param name="itemTitle">The title to search for.</param>
        /// <returns>An array of indexes where this item exists.</returns>
        protected virtual int[] GetAllIndexOfItem(string itemTitle)
        {
            List<int> t = new List<int>();
            int curIndex = 0;
            while (curIndex < Content.Count)
            {
                if (Content[curIndex] != null && Content[curIndex].Source.Title == itemTitle) t.Add(curIndex);
                curIndex++;
            }

            return t.ToArray();
        }

        /// <summary>
        /// Looks for a slot that contains a specific item and is not full.
        /// </summary>
        /// <param name="itemTitle">The title of the item.</param>
        /// <param name="startIndex">The index to start searching from.</param>
        /// <returns></returns>
        protected virtual int GetIndexOfItemNotFull(string itemTitle, int startIndex = 0)
        {
            for (int i = startIndex; i < Content.Count; i++)
            {
                if (Content[i] == null) continue;
                if (Content[i].Source.Title == itemTitle && Content[i].StackSize < Content[i].Source.MaxStackSize)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Shorthand math method to get the amount of items leftover after merging two stacks.
        /// </summary>
        /// <param name="fromSize">The origin's stack size. Eg `itemBeingMoved.StackSize`</param>
        /// <param name="destinationSize">The destination's current stack size. Eg `someTargetItem.StackSize`</param>
        /// <param name="maxSize">The destination's maximum stack size. Eg `someTargetItem.MaxStackSize`</param>
        /// <returns>How many should remain at the origin because they don't fit in the destination.</returns>
        protected virtual int GetMergeRemainder(int fromSize, int destinationSize, int maxSize)
        {
            int spaceAvailable = maxSize - destinationSize;
            return Mathf.Clamp(fromSize - spaceAvailable, 0, fromSize);
        }

        /// <summary>
        /// Find the total quantity of an item in this Inventory by scanning all of the slots.
        /// </summary>
        /// <param name="itemTitle">The item to count.</param>
        /// <returns>The total stack count of the item in the entire inventory.</returns>
        protected virtual int GetItemCount(string itemTitle)
        {
            int result = 0;
            foreach (RootItemStack x in Content)
            {
                if (x == null) continue;
                if (x.Source.Title == itemTitle)
                {
                    result += x.StackSize;
                }
            }
            return result;
        }

        protected virtual void OnDestroy()
        {
            OnDeath?.Invoke();
        }
    }
}