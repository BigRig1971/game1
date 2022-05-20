using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using FirstGearGames.SmoothCameraShaker;


namespace StupidHumanGames
{
    public class AnimTriggerAndDamageDealer : MonoBehaviour
    {
        Rigidbody rb;
        public Vector3 colliderSize = Vector3.one;
        public Vector3 colliderCenter = Vector3.zero;
        public KeyCode _keyCode = KeyCode.Mouse0;
        public int damagePower = 5;
        public string _triggerName;
        public string _animationName;
        public float delayImpact = .3f;
        [SerializeField] Animator _animator;
        bool canDamageStuff = false;
        public ShakeData MyShake;

        private void Start()
        {

            if (TryGetComponent<Rigidbody>(out rb))
            {
                rb = GetComponent<Rigidbody>();
            }
            else
            {
                rb = gameObject.AddComponent<Rigidbody>() as Rigidbody;
                rb.isKinematic = true;
            }
            BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.size = colliderSize;
            boxCollider.center = (Vector3.zero + colliderCenter);
            boxCollider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Lootable"))///
            {
                var lootable = other.gameObject.GetComponent<LootableItem>();
                if (!canDamageStuff) return;
                canDamageStuff = false;
                _animator.SetTrigger("Interrupt");
                CameraShakerHandler.Shake(MyShake);
                if(lootable != null)
                {
                    lootable.TakeDamage(damagePower);
                    lootable.LootableItems();
                }
            }
        }
        void Update()
        {

            if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
            {
                _animator.SetTrigger(_triggerName);
                canDamageStuff = true;
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
        }
    }
}
