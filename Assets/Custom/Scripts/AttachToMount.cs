using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StupidHumanGames
{
    public class AttachToMount : MonoBehaviour
    {
        bool isMounted = false;
        StarterAssetsInputs _input;
        Transform mount;
        Vector3 offset;
        private void Start()
        {
            _input = GetComponent<StarterAssetsInputs>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Mountable"))
            {
                mount = other.GetComponent<Transform>();
                isMounted = true;
                
                transform.SetParent(other.transform);
                transform.localPosition = new Vector3(0, 1f, 0);
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
        private void FixedUpdate()
        {
            if (isMounted)
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    transform.localPosition = new Vector3(0, .2f, 0);
                }
                if (_input._move == Vector2.zero)
                {
                    transform.localPosition = new Vector3(offset.x ,.2f ,offset.z); 
                }
                else
                {
                    offset = transform.localPosition;
                }
            }
           
        }
    }
    
}
