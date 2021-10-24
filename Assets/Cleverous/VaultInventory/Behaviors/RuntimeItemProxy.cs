// (c) Copyright Cleverous 2020. All rights reserved.

using System;
using System.Collections;
using Cleverous.VaultSystem;
using Mirror;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cleverous.VaultInventory
{
    [RequireComponent(typeof(Collider))]
    public class RuntimeItemProxy : NetworkBehaviour
    {
        public RootItemStack Data;
        public bool AutoPickup;
        public float LockedForSeconds;
        public LayerMask CanPickMeUp;
        public bool PopSpawn;
        private bool m_busy;

        private float m_spawnedAtTime;
        private GameObject m_model;

        [SyncVar] private Vector3 m_origin;
        [SyncVar] private Vector3 m_offset;
        [SyncVar] private int m_itemId;
        [SyncVar] private int m_stackSize;

        public static Action<RootItemStack> OnPickup;
        public static AnimationCurve MoundCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(0.5f, 1), new Keyframe(1, 0));

        protected virtual void Reset()
        {
            Data = null;
            AutoPickup = true;
            LockedForSeconds = 1;
            CanPickMeUp = 1;
            PopSpawn = true;
            m_busy = false;
        }
        protected virtual void OnEnable()
        {
            m_busy = false;
            m_spawnedAtTime = Time.time;
        }
        protected virtual void OnTriggerEnter(Collider col)
        {
            if (!isServer) return;

            if (m_spawnedAtTime + LockedForSeconds > Time.time) return;
            bool validTouch = ((1 << col.gameObject.layer) & CanPickMeUp) != 0;
            if (!validTouch) return;
            if (!AutoPickup) return;

            IUseInventory touchySubject = col.GetComponent<IUseInventory>();
            if (touchySubject != null) TryPickup(touchySubject);
        }

        public override void OnStartClient()
        {
            ClientInit();
        }

        public void ClientInit()
        {
            if (m_model != null) Destroy(m_model); // cleanup if necessary.

            RootItem source = (RootItem)Vault.Get(m_itemId);

            //Debug.Log($"<color=cyan>Spawned as [{m_itemId}] : ({m_stackSize}) {source.Title}</color>", this);

            Data = new RootItemStack(source, m_stackSize);
            m_model = Instantiate(Data.Source.ArtPrefab, transform, false);

            if (m_offset != Vector3.zero) StartCoroutine(SpawnPop(m_offset));
        }

        [Server]
        public virtual void SvrInitialize(RootItem sourceItem, int stackSize)
        {
            float mag = Random.value * 5;
            Vector2 rng = Random.insideUnitCircle;

            m_offset = new Vector3(rng.x * mag, transform.position.y, rng.y * mag);
            m_itemId = Vault.GetIndex(sourceItem);
            m_stackSize = stackSize;
            m_origin = transform.position;
        }

        public virtual void TryPickup(IUseInventory requestor)
        {
            // If the Inventory can't fit the content, it'll reflect back the amount.
            // We have to keep this object and update the stack size here in that case.
            if (m_busy) return;
            m_busy = true;

            int remainder = VaultInventory.TryGiveItem(requestor.Inventory, Data);
            if (remainder <= 0) DeSpawn();
            else Data.StackSize = remainder;

            m_busy = false;
        }

        // handles locally moving the object in a 'pop' motion.
        protected virtual IEnumerator SpawnPop(Vector3 offset)
        {
            Vector3 goal = m_origin + offset;

            const float totalTime = 0.5f;
            float time = 0;
            while (time < totalTime)
            {
                time += Time.deltaTime;
                Vector3 lerp = Vector3.Lerp(m_origin, goal, time);
                lerp.y = m_origin.y + MoundCurve.Evaluate(time / totalTime);
                transform.position = lerp;
                yield return null;
            }
        }

        public virtual void DeSpawn()
        {
            // todo pooling
            Destroy(gameObject);
        }
    }
}