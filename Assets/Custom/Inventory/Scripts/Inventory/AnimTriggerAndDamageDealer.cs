using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using FirstGearGames.SmoothCameraShaker;


namespace StupidHumanGames
{
    public class AnimTriggerAndDamageDealer : MonoBehaviour
    {
        [SerializeField] AudioSource _audioSource;
        Rigidbody rb;
        public Vector3 colliderSize = Vector3.one;
        public Vector3 colliderCenter = Vector3.zero;
        public KeyCode _keyCode = KeyCode.Mouse0;
        public int damagePower = 5;
        public string _animTriggerName;
        public string _animIntName;
        public string _animBoolName;
        public int _animIntValue;
        public float delayImpact = .3f;
        [SerializeField] Animator _animator;
        bool canDoStuff = false;
        public ShakeData MyShake;
        public AudioClip weaponSwingSound;
        [SerializeField, Range(0f,1f)] float swingVolume = 1;

        private void OnEnable()
        {
            if (_animBoolName != null) _animator.SetBool(_animBoolName, true);
        }
        private void OnDisable()
        {
            if (_animBoolName != null) _animator.SetBool(_animBoolName, false);
        }
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
            if (other.CompareTag("Lootable") && canDoStuff)///
            {
                canDoStuff = false;
                var lootable = other.gameObject.GetComponent<LootableItem>();
               
                if(lootable != null)
                {
                    _animator.SetTrigger("Interrupt");
                    _animator.StopPlayback();
                    CameraShakerHandler.Shake(MyShake);

                    lootable.TakeDamage(damagePower);
                   
                }
            }
        }
        void Update()
        {

            if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
            {
               if(_animIntName != null) _animator.SetInteger(_animIntName, _animIntValue);
               if(_animTriggerName != null) _animator.SetTrigger(_animTriggerName);
             
            }
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
        }
        public void OnAnimStart()
        {
            canDoStuff = true;
           Invoke(nameof(OnAnimEnd), .1f);
        }
        public void OnAnimEnd()
        {
            canDoStuff = false;
        }
        public void OnWeaponSwing()
        {
            
            if (weaponSwingSound != null)
            {
                _audioSource.PlayOneShot(weaponSwingSound, swingVolume);
            }
        }
    }
}
