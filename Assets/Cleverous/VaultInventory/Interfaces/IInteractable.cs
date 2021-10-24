// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;

namespace Cleverous.VaultInventory
{
    /// <summary>
    /// <para>An Interactable should be something like a 'StructureInWorld.cs' or 'LootCrateInWorld.cs' or 'UiSpecialIcon.cs'...</para>
    /// </summary>
    public interface IInteractable
    {
        Transform MyTransform { get; }
        InteractionType InteractionSpace { get; }
        Interaction[] Interactions { get; }
        void Interact();
    }
}