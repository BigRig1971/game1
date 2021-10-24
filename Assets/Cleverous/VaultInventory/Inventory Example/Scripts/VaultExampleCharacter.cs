// (c) Copyright Cleverous 2020. All rights reserved.

using Cleverous.VaultSystem;
using Mirror;
using UnityEngine;
using Random = System.Random;

namespace Cleverous.VaultInventory.Example
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(NetworkIdentity))]
    public class VaultExampleCharacter : NetworkBehaviour, IUseInventory
    {
        [AssetDropdown(typeof(LootTable))]
        public LootTable StartingItems;
        public CharacterController Controller;
        public Animator Animator;
        public MeshRenderer PlayerNode;
        [SyncVar] protected bool IsMoving;

        // we satisfy the interface here and provide a serialized backing field below.
        public Inventory Inventory
        {
            get => m_inventory;
            set => m_inventory = value;
        }
        [SerializeField] private Inventory m_inventory;

        public Transform MyTransform => transform;
        private Vector3 m_inputLast;

        public void Awake()
        {
            m_inputLast = Vector3.forward;
        }

        public override void OnStartLocalPlayer()
        {
            // lets sync up the inventory since we have some authority.
            Inventory.CmdRefreshAllFromServer();
            // and assign a random color to our player node.
            Random rng = new Random();
            PlayerNode.material.color = new Color((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble(), 1);
        }

        public override void OnStartClient()
        {
            Inventory.Initialize(this);

            // If this is happening on the server, lets give our character some starting items using a LootTable!
            if (isServer)
            {
                for (int i = 0; i < StartingItems.Items.Length; i++)
                {
                    if (StartingItems.Items[i] == null) continue;
                    Inventory.DoAdd(new RootItemStack(StartingItems.Items[i], StartingItems.Amounts[i]));
                }
            }

            if (isLocalPlayer)
            {
                // Tell Vault and the UI that we are here and ready.
                VaultInventory.OnPlayerSpawn.Invoke(this);
            }
        }

        public void Update()
        {
            // We don't need to do anything in Update() for the inventory. It is event based.
            MovementAndAnimation();
        }

        public void MovementAndAnimation()
        {
            if (!isLocalPlayer)
            {
                //Animator.SetBool("IsMoving", IsMoving);
                return;
            }

            Vector3 input = new Vector3();
            bool a = UnityEngine.Input.GetKey(KeyCode.A);
            bool d = UnityEngine.Input.GetKey(KeyCode.D);
            bool w = UnityEngine.Input.GetKey(KeyCode.W);
            bool s = UnityEngine.Input.GetKey(KeyCode.S);

            if (a) input.x -= 1;
            if (d) input.x += 1;
            if (w) input.z += 1;
            if (s) input.z -= 1;

            IsMoving = input.magnitude > 0;
            Animator.SetBool("IsMoving", IsMoving);
            transform.rotation = Quaternion.LookRotation(new Vector3(m_inputLast.x, 0, m_inputLast.z), Vector3.up);
            Controller.Move(input * 5 * Time.deltaTime);

            if (input != Vector3.zero) m_inputLast = input;
        }
    }
}