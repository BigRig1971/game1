// (c) Copyright Cleverous 2020. All rights reserved.

namespace Cleverous.VaultInventory
{
    public class ItemInteractionSplit : Interaction
    {
        protected override void Reset()
        {
            base.Reset();
            Title = "Interact Split";
            Description = "Split a stack of inventory items in half.";
            InteractLabel = "SPLIT";
        }

        public override bool IsValid(IInteractable target, object data = null)
        {
            if (target.InteractionSpace != InteractionType.UiInventory) return false;

            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return false;

            return plug.ReferenceVaultItemData.MaxStackSize > 1 && plug.Ui.TargetInventory.Get(plug.ReferenceInventoryIndex).StackSize > 1;
        }

        public override void DoInteract(IInteractable target, object data = null)
        {
            if (target.InteractionSpace != InteractionType.UiInventory) return;

            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return;

            plug.Ui.TargetInventory.CmdRequestSplit(plug.ReferenceInventoryIndex);
        }
    }
}