// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using Cleverous.VaultInventory.Example;
using UnityEngine;

namespace Cleverous.VaultInventory
{
    public class ItemInteractionUse : Interaction
    {
        [Header("Sounds")]
        public AudioClip SoundChew;
        public AudioClip SoundEquip;

        protected override void Reset()
        {
            base.Reset();
            Title = "Interact Use";
            Description = "Use the item immediately.";
            InteractLabel = "USE";
        }

        public override bool IsValid(IInteractable target, object data = null)
        {
            // This is one implementation of "Use" and is restricted to the demo's Example Types
            // which are Consumable, Armor and Weapon. You should roll your own Interactions like
            // this to facilitate how you want your game to work.

            if (target.InteractionSpace != InteractionType.UiInventory) return false;

            // we should only be talking to an ItemUiPlug, so lets grab it.
            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return false;

            // You can only use this command from the Player inventory. Because reasons!
            VaultExampleCharacter player = plug.Ui.TargetInventory.GetComponent<VaultExampleCharacter>();
            if (player == null) return false;

            // now we need to see if the plug is representing a Useable type
            Type targetType = plug.ReferenceVaultItemData.GetType();
            if (targetType == typeof(WaffleConsumable) 
                || targetType == typeof(WaffleArmor)
                || targetType == typeof(WaffleWeapon))
            {
                // if it is, we can say yeah, this is "Use"able. Show this button.
                return true;
            }

            // otherwise, we can't "Use" it. Don't show this button.
            return false;
        }

        public override void DoInteract(IInteractable target, object data = null)
        {
            // This is just one way to filter the interactions and run some code based on the content.
            // You can override this and make new Interaction classes, do the interaction on the object,
            // pass additional information through the `data` argument, etc.
            //
            // This could also be multiple interactions, such as "Equip", "Consume", or whatever.
            // ... and that's probably the ideal way to do it!! But here we're doing it as an example
            // of how you could bundle these things.

            if (target.InteractionSpace != InteractionType.UiInventory) return;

            // We should only be talking to an ItemUiPlug, so lets grab it.
            ItemUiPlug plug = target.MyTransform.GetComponent<ItemUiPlug>();
            if (plug == null) return;

            // Lets see which type of content we're working with.
            Type targetType = plug.ReferenceVaultItemData.GetType();

            // Grab the sound source from the player holding the inventory.
            AudioSource audio = plug.Ui.TargetInventory.transform.GetComponent<AudioSource>();

            // for consumables, we can just eat it and be done.
            if (targetType == typeof(WaffleConsumable))
            {
                // In this nieve example, we simply drop the items instead of consuming them.
                //
                // The reason we don't "Consume" the item is because we want the example to be very generic and there are
                // many ways that developers may want to handle consumption of items, so we don't want to enforce anything here.
                //
                // You might choose, for example, to extend the partial Inventory class to include a "CmdRequestConsume()" method.
                // Doing so would allow you to follow the design pattern and non destructively add your own functionality.
                //
                // We recommend extending the Inventory functions to support whatever commands you need to facilitate your project.

                audio.clip = SoundChew;
                audio.Play();

                // Drop the item (explained above)
                plug.Ui.TargetInventory.CmdRequestDrop(plug.ReferenceInventoryIndex);

                return;
            }

            // for everything else, we will need the network behavior index of the inventory
            int behaviorIndex = 0;
            for (int i = 0; i < plug.Ui.TargetInventory.netIdentity.NetworkBehaviours.Length; i++)
            {
                if (plug.Ui.TargetInventory.netIdentity.NetworkBehaviours[i] == plug.Ui.TargetInventory)
                {
                    behaviorIndex = i;
                }
            }

            if (targetType == typeof(WaffleArmor))
            {
                // get the slot restriction
                SlotRestriction restriction = plug.ReferenceVaultItemData.Restriction;

                // find where those are at in the inventory
                int[] possibleSlots = plug.Ui.TargetInventory.GetAllSlotIndexOfType(restriction);
                int firstEmpty = plug.Ui.TargetInventory.GetFirstSlotIndexOfType(restriction, true);

                // if theres an empty spot, try to put it into it.
                if (firstEmpty != -1)
                {
                    plug.Ui.TargetInventory.CmdRequestMove(
                        plug.Ui.TargetInventory.netId,
                        plug.Ui.TargetInventory.netId,
                        plug.ReferenceInventoryIndex,
                        firstEmpty,
                        behaviorIndex,
                        behaviorIndex);

                    audio.clip = SoundEquip;
                    audio.Play();
                    return;
                }

                // otherwise, swap with the first available slot.
                if (possibleSlots.Length == 0) return;

                plug.Ui.TargetInventory.CmdRequestMove(
                    plug.Ui.TargetInventory.netId,
                    plug.Ui.TargetInventory.netId,
                    plug.ReferenceInventoryIndex,
                    possibleSlots[0],
                    behaviorIndex,
                    behaviorIndex);

                audio.clip = SoundEquip;
                audio.Play();
                return;
            }
            if (targetType == typeof(WaffleWeapon))
            {
                // get the slot restriction
                SlotRestriction restriction = plug.ReferenceVaultItemData.Restriction;

                // find where those are at in the inventory
                int targetSlotIndex = plug.Ui.TargetInventory.GetFirstSlotIndexOfType(restriction, true);
                if (targetSlotIndex == -1) return; // no empty slot for the weapon!

                // move the weapon!
                plug.Ui.TargetInventory.CmdRequestMove(
                    plug.Ui.TargetInventory.netId,
                    plug.Ui.TargetInventory.netId,
                    plug.ReferenceInventoryIndex,
                    targetSlotIndex,
                    behaviorIndex,
                    behaviorIndex);

                audio.clip = SoundEquip;
                audio.Play();
            }
        }
    }
}