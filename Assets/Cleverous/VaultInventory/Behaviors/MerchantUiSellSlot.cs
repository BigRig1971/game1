// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;

namespace Cleverous.VaultInventory
{
    public class MerchantUiSellSlot : ItemUiPlug
    {
        [Header("Merchant")]
        public MerchantUi MerchantUiObject;

        public override void OnDrop(PointerEventData eventData)
        {
            base.OnDrop(eventData);

            MerchantUiObject.ClientSell(InventoryUi.DragOrigin.ReferenceInventoryIndex);

            if (InventoryUi.DragFloater != null) Destroy(InventoryUi.DragFloater);
            InventoryUi.DragOrigin = null;
            InventoryUi.DragDestination = null;
        }
    }
}