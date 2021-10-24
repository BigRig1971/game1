// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;
using Mirror;
using UnityEngine;

namespace Cleverous.VaultInventory.Example
{
    public class VaultExampleChestAdvanced : VaultExampleChestSimple, IUseInventory
    {
        public Transform MyTransform => transform;
        public Inventory Inventory
        {
            get => m_inventory;
            set => m_inventory = value;
        }

        [Header("Advanced Chest Configuration")]
        [SerializeField]
        private Inventory m_inventory;
        [AssetDropdown(typeof(LootTable))]
        public LootTable StartingLoot;

        protected bool SvrLocked;
        private GameObject m_chestInventoryUi;

        public override void OnStartClient()
        {
            if (!m_inventory.IsInitialized) Inventory.Initialize(this, Inventory.Configuration);
        }
        public override void OnStartServer()
        {
            if (!m_inventory.IsInitialized) Inventory.Initialize(this, Inventory.Configuration);
            if (StartingLoot == null) return;

            int count = Random.Range(0, StartingLoot.Items.Length);
            for (int i = 0; i < count; i++)
            {
                m_inventory.DoAdd(StartingLoot.GetLoot());
            }
        }

        public override void OnTriggerEnter(Collider col)
        {
            if (isServer && !SvrLocked)
            {
                SvrOpen(col);
            }
        }
        public override void OnTriggerExit(Collider col)
        {
            if (isServer && SvrLocked)
            {
                SvrClose(col);
            }
        }

        [Server]
        private void SvrOpen(Collider col)
        {
            // on the server make sure the box is not in use by someone else, then callback to players.
            SvrLocked = true;
            NetworkIdentity interactor = col.GetComponent<NetworkIdentity>();
            GetComponent<NetworkIdentity>().AssignClientAuthority(interactor.connectionToClient);
            RpcOpenCallback();
        }
        [Server]
        private void SvrClose(Collider col)
        {
            // on the server make sure the touching player has authority to close the box.
            NetworkIdentity interactor = col.GetComponent<NetworkIdentity>();
            if (interactor.connectionToClient != netIdentity.connectionToClient) return;

            // if they're the one interacting, then they are in control and can exit, closing the box.
            SvrLocked = false;
            GetComponent<NetworkIdentity>().RemoveClientAuthority();
            RpcCloseCallback();
            UiContextMenu.Instance.HideContextMenu();
        }

        [ClientRpc]
        private void RpcOpenCallback()
        {
            // in this global callback all players will open the box
            AudioSource.clip = SoundOpen;
            AudioSource.Play();
            Lid.transform.localRotation = Quaternion.Euler(90, 0, 0);

            // but only the one with authority (the one touching it was assigned authority by server) will get a UI for it.
            if (hasAuthority) m_chestInventoryUi = VaultInventory.SpawnInventoryUi(Inventory);
        }
        [ClientRpc]
        private void RpcCloseCallback()
        {
            // in this global callback all players will close the lid.
            AudioSource.clip = SoundClose;
            AudioSource.Play();
            Lid.transform.localRotation = Quaternion.Euler(0, 0, 0);
            UiContextMenu.Instance.HideContextMenu();
            Cleanup();
        }

        protected virtual void Cleanup()
        {
            if (m_chestInventoryUi != null) Destroy(m_chestInventoryUi);
        }

        public void OnDestroy()
        {
            Cleanup();
        }
    }
}