﻿// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;
using UnityEngine;

namespace Cleverous.VaultInventory.Example
{
    public class VaultExampleSoundManager : MonoBehaviour
    {
        [Header("For basic stuff")]
        public AudioClip ClipMoveBegin;
        public AudioClip ClipMoveEnd;
        public AudioClip ClipPickup;

        [Header("For rare pickups")]
        public AudioClip ClipVictory;

        [Header("Special item")]
        [AssetDropdown(typeof(RootItem))]
        public RootItem CoinItem;
        public AudioClip ClipCoin;


        protected AudioSource SoundPlayer;
        protected IUseInventory Player;

        private void Start()
        {
            // hook into the UI's event for when a player is spawned
            VaultInventory.OnPlayerSpawn += SpawnHook;
        }

        protected virtual void SpawnHook(IUseInventory player)
        {
            Player = player;
            SoundPlayer = Player.MyTransform.GetComponent<AudioSource>();

            // Now let's subscribe to some events to get some actionable callbacks!
            VaultInventory.OnMoveItemBegin += OnLift;
            VaultInventory.OnMoveItemEnd += OnDrop;
            Player.Inventory.OnNewItemAdded += OnNewAdded;
        }

        protected virtual void OnLift(ItemUiPlug originSlot)
        {
            SoundPlayer.clip = ClipMoveBegin;
            SoundPlayer.Play();
        }

        protected virtual void OnDrop(ItemUiPlug destinationSlot)
        {
            SoundPlayer.clip = ClipMoveEnd;
            SoundPlayer.Play();
        }

        protected virtual void OnNewAdded(int slotId)
        {
            // We can easily check what kind of item it is and play unique sounds.
            // Or check item weight, and play heavier type sounds.
            // Or put specific sounds onto the Item itself and reference those directly instead of defining them here.

            if (Player.Inventory.Get(slotId).Source == CoinItem) SoundPlayer.clip = ClipCoin;
            else if (Player.Inventory.Get(slotId).Source.Rarity == ItemRarity.Inconcievable) SoundPlayer.clip = ClipVictory;
            else SoundPlayer.clip = ClipPickup;

            SoundPlayer.Play();
        }
    }
}