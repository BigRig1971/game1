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
        public AudioSource weaponSwingSound;
        [SerializeField, Range(0f,1f)] float swingVolume = 1;

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

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Lootable") && canDamageStuff)///
            {
                var lootable = other.gameObject.GetComponent<LootableItem>();
                        
                _animator.SetTrigger("Interrupt");
                _animator.StopPlayback();
               
                CameraShakerHandler.Shake(MyShake);
                if(lootable != null)
                {
                    lootable.TakeDamage(damagePower);
                    lootable.LootableItems();
                    canDamageStuff = false;
                }
            }
        }
        void Update()
        {

            if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
            {
                _animator.SetTrigger(_triggerName);

              
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
        }
        public void OnWeaponDamageStart()
        {
            canDamageStuff = true;
            
        }
        public void OnWeaponSwing()
        {
            
            if (weaponSwingSound != null)
            {
                weaponSwingSound.volume = swingVolume;
                weaponSwingSound.pitch = (Random.Range(.9f, 1f));
                weaponSwingSound?.Play();
            }
                
                

        }

    }
}
