using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StupidHumanGames
{

    public class Climbing : MonoBehaviour
    {
        [SerializeField] LayerMask climbLayer;
        ThirdPersonController _tpc;
        Animator _anim;
        Transform t;
        bool canClimb = false;
        // Start is called before the first frame update
        void Start()
        {
            _tpc = GetComponent<ThirdPersonController>();
            _anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                
                canClimb = !canClimb;
                if (canClimb)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position + transform.up * 1.5f, transform.TransformDirection(Vector3.forward), out hit, 2, climbLayer))
                    {
                        hit.transform.gameObject.AddComponent<UnityEngine.MeshRenderer>();
                    }
                        _anim.SetBool("Climb", true);
                   // _tpc.isMounted = true;
                   


                }
                else
                {
                  
                    _anim.SetBool("Climb", false);
                   // _tpc.isMounted = false;
                }

            }
            if (canClimb)
            {
                RaycastHit hit;
               
                if (Physics.Raycast(transform.position + transform.up * 1.5f, transform.TransformDirection(Vector3.forward), out hit, 2, climbLayer))
                {

                    Debug.DrawRay(transform.position + transform.up * 1.5f, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

                    // t.position = hit.point + (hit.normal * 10);
                    Vector3 position = transform.position;
                    Vector3 offsetToHit = position - hit.point;
                    offsetToHit.Normalize();
                    offsetToHit *= 1.5f;
                    position = hit.point + offsetToHit;
                    Vector3 lookPosition = new Vector3(position.x, transform.position.y, position.z);
                    transform.position = lookPosition;
                    transform.LookAt(lookPosition);
                   

                }
                else
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 5, Color.white);
                    Debug.Log("Did not Hit");

                    _anim.SetBool("Climb", false);
                   // _tpc.isMounted = false;
                }
               // t.position = new Vector3(t.position.x, transform.position.y, t.position.z);
               // transform.position = t.position - transform.forward;
                //transform.LookAt(t.position);
            }
           
            
        }
    }
}
