using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace StupidHumanGames
{


    public class ParentPlayerToMount : MonoBehaviour
    {
        [SerializeField] string animationTrigger;
        Rigidbody rb;
        Animator _animator;
        bool isMounted = false;
        StarterAssetsInputs _input;
        Transform player;
        Vector3 offset;
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isMounted)
            {

                if (_animator.GetBool(animationTrigger))
                {
                    if (_input._move == Vector2.zero)
                    {
                        _animator.SetTrigger("Interrupt");
                    }
                    else
                    {
                        rb.AddForce(transform.forward *3f, ForceMode.Acceleration);                   
                    }
                   
                
                    player.localPosition = Vector3.zero;
                    transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, player.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);                
                    player.localRotation = Quaternion.identity;
                  
                }
               
                if (_input._move == Vector2.zero)
                {

                    player.transform.localPosition = new Vector3(offset.x, 0f, offset.z);
                }
                else
                {

                    offset = player.transform.localPosition;
                }

            }

        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
               
                isMounted = true;
                player = other.GetComponent<Transform>();
                _animator = other.GetComponent<Animator>();
                _input = other.GetComponent<StarterAssetsInputs>();
                player.transform.SetParent(transform);
                player.transform.localPosition = Vector3.zero;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isMounted = false;
                other.transform.SetParent(null);
            }

        }
    }
}