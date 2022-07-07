using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace StupidHumanGames
{
    public class AttachToMount : MonoBehaviour
    {
        public UnityEvent controlMountEnable;
        public UnityEvent controlMountDisable;
        Animator _animator;
        bool isMounted = false;
        StarterAssetsInputs _input;
        Transform mount;
        Vector3 offset;
        private void Start()
        {
            if (controlMountEnable == null)
                controlMountEnable = new UnityEvent();
            if (controlMountDisable == null)
                controlMountDisable = new UnityEvent();
            _input = GetComponent<StarterAssetsInputs>();
            _animator = GetComponent<Animator>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Mountable"))
            {
                
            
                mount = other.GetComponent<Transform>();
                Rigidbody rb= mount.GetComponent<Rigidbody>();
                rb.maxLinearVelocity = 0f;
                
                isMounted = true;
                
                transform.SetParent(other.transform);
                
                transform.localPosition = new Vector3(0, 1f, 0);

                rb.maxLinearVelocity = 10f;
            }
        }
        private void OnTriggerExit(Collider other)
        {

            if (other.CompareTag("Mountable"))
            {
                isMounted = false;
                transform.SetParent(null);
            }
                
        }
        private void Update()
        {
            if (isMounted)
            {

                if (_animator.GetBool("CanRide"))
                {
                   
                    controlMountEnable?.Invoke();
                    if (_input._move.y < 0f)
                    {
                        _animator.SetFloat("SpeedMultiplier", -1f);
                    }
                    else
                    {
                        _animator.SetFloat("SpeedMultiplier", 1f);
                    }
                    transform.localPosition = Vector3.zero;
                    transform.localRotation = Quaternion.identity;
                   
                    
                }
                else
                {
                    
                  
                    controlMountDisable?.Invoke();
                    
                   
                }
                if (_input._move == Vector2.zero)
                {
                    transform.localPosition = new Vector3(offset.x, 0, offset.z);
                }
                else
                {
                    offset = transform.localPosition;
                }
                
            }
        }
    }
}
