using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using FirstGearGames.SmoothCameraShaker;


namespace StupidHumanGames
{
    public class AnimTriggerAndDamageDealer : MonoBehaviour
    {
        [SerializeField] bool groundHugging = false;
        [SerializeField] Transform lookTransform;
        [SerializeField] Transform cameraRoot;
        [SerializeField] float cameraRootOffset;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] GameObject[] _itemsToDisable;
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
        [SerializeField] ThirdPersonController _tpc;
        bool canDoStuff = false;
        public ShakeData MyShake;
        public AudioClip weaponSwingSound;
        [SerializeField, Range(0f, 1f)] float swingVolume = 1;
        bool toggle = false;
        float previousCamOffset;
       bool hugTheGround = false;


        private void Start()
        {
            _tpc = GameObject.FindObjectOfType<ThirdPersonController>();
           
            previousCamOffset = cameraRoot.transform.position.y;
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

                if (lootable != null)
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
           
            if (hugTheGround) _tpc.OnYPosition();

            if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
            {
                toggle = !toggle;
                if (_animIntName != null) _animator.SetInteger(_animIntName, _animIntValue);
                if (_animTriggerName != null) _animator.SetTrigger(_animTriggerName);



                if (_animBoolName != null) _animator.SetBool(_animBoolName, toggle);
                if (toggle)
                {
                    hugTheGround = true;
                    cameraRoot.position =  new Vector3(cameraRoot.position.x, cameraRoot.position.y + cameraRootOffset, cameraRoot.position.z);

                }
                else
                {
                    hugTheGround= false;
                    cameraRoot.position = new Vector3(cameraRoot.position.x, cameraRoot.position.y + cameraRootOffset * -1, cameraRoot.position.z);
                     _tpc.transform.rotation = Quaternion.Euler(0f, _tpc.transform.rotation.eulerAngles.y, 0f);
                }

                foreach(GameObject go in _itemsToDisable)
                {
                    go.SetActive(!toggle);
                }


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
