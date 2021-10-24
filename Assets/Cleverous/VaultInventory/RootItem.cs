// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;
using UnityEngine;

namespace Cleverous.VaultInventory
{
    public abstract class RootItem : DataEntity
    {
        [Header("[Item Properties]")]
        public ItemRarity Rarity;
        [AssetDropdown(typeof(SlotRestriction))]
        public SlotRestriction Restriction;
        public Sprite UiIcon;
        public GameObject ArtPrefab;
        [AssetDropdown(typeof(Interaction))]
        public Interaction[] Interactions = new Interaction[3];
        
        public int MaxStackSize;
        public int Value;

        protected override void Reset()
        {
            base.Reset();
            Description = "Upon further examination, you find the cryptic words 'Lorum ipsum'.";

            Rarity = ItemRarity.Common;
            Restriction = null;
            UiIcon = null;
            ArtPrefab = null;
            MaxStackSize = 99;
            Value = 100;
        }

        public virtual Color GetRarityColor()
        {
            return ItemRarityColors.RarityColors[(int) Rarity];
        }
        public virtual string GetUiTitle()
        {
            string x = ColorUtility.ToHtmlStringRGB(GetRarityColor());
            return $"<color=#{x}>{Title}</color>";
        }
        public virtual string GetDescriptionSimple()
        {
            return Description;
        }
        public virtual string GetDescriptionComplex()
        {
            string restriction = Restriction == null ? "<color=white>Generic Item</color>" : $"<color=white>{Restriction.Title}</color>";
            return $"{GetDescriptionSimple()}\n" +
                   $"{restriction}\n" +
                   $"<color=lime> ${Value}</color>";
        }
    }
}