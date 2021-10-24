// (c) Copyright Cleverous 2020. All rights reserved.

using UnityEngine;

namespace Cleverous.VaultInventory.Example
{
    public class WaffleConsumable : RootItem
    {
        [Header("[Consumable]")]
        public int HealthAddition;

        protected override void Reset()
        {
            base.Reset();
            HealthAddition = 80;
        }
    }
}