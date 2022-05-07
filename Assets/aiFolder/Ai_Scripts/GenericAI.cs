using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAI : MonoBehaviour
{
    public LayerMask whatIsPlayer;
    private float distanceToPlayer;
    public enum state { patrol, chase, attack, idle };
    private state _state;
    private Transform player;
    public float attackRange = 1f, sightRange = 5f, maxDistance = 10f;
    private bool canSeePlayer = false, canAttackPlayer = false;
    bool playerInSightRange;
    float canChase;
    bool playerInAttackRange;
    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        _state = state.patrol;
    }

    // Update is called once per frame
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        switch (_state)
        {
            case state.patrol:
               if(playerInSightRange || playerInAttackRange) ChangeState();
                Debug.Log("patrol");
                break;
            case state.chase:
                if(!playerInSightRange || playerInAttackRange) ChangeState();    
                Debug.Log("follow");
                break;
            case state.attack:
                if(!playerInAttackRange) ChangeState(); 
                Debug.Log("attack");
                break;
           /* case state.idle:

                
                Debug.Log("idle");
                break;*/

        }
        void ChangeState()
        {
            _state++;
            if ((int)_state >= 3)
            {
                _state = state.patrol;
            }
        }
       
    }
}
