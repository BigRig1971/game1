// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultInventory.Example;

namespace Cleverous.VaultInventory
{
    public class ItemInteractionDrop : Interaction
    {
        protected override void Reset()
        {
            base.Reset();
            Title = "Interact Drop";
            Description = "Drop the item immediately.";
            InteractLabel = "Drop";
        }

        public override bool IsValid(IInteractable target, object data = null)
        {
            if (target.InteractionSpace != InteractionType.UiInventory) return false;

            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return false;

            // You can only use this command from the Player inventory. Because reasons!
            VaultExampleCharacter player = plug.Ui.TargetInventory.GetComponent<VaultExampleCharacter>();
            return player != null;
        }

        public override void DoInteract(IInteractable target, object data = null)
        {
            if (target.InteractionSpace != InteractionType.UiInventory) return;

            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return;

            plug.Ui.TargetInventory.CmdRequestDrop(plug.ReferenceInventoryIndex);
        }
    }
}