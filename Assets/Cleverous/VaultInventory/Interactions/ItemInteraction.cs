// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;

namespace Cleverous.VaultInventory
{
    public abstract class Interaction : DataEntity
    {
        public string InteractLabel;

        /// <summary>
        /// Determine if an interaction is valid for the context.
        /// </summary>
        /// <param name="uiTarget">The target UI Plug</param>
        /// <param name="data">Optional data for context, populate with your own struct and recast.</param>
        /// <returns>Whether or not the action is valid.</returns>
        public abstract bool IsValid(IInteractable uiTarget, object data = null);
        /// <summary>
        /// Perform the designed Interaction.
        /// </summary>
        /// <param name="uiTarget">The target UI Plug</param>
        /// <param name="data">Optional data for context, populate with your own struct and recast.</param>
        public abstract void DoInteract(IInteractable uiTarget, object data = null);
    }
}