using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAI : MonoBehaviour
{
    public float scale = 1f;
    public bool canSwimOrFly = false;
    Quaternion targetRot;
    public bool patrol = true, chase = true, attack = true, idle = true;
    public LayerMask whatIsGround, whatIsPlayer, obstacleLayer;
    public float sightRange, attackRange, boundSize, maxRange, maxAltitude, minAltitude, turnSpeed = 3f, attackSpeed = 5f, animationMultiplier = 5f;
    Vector3 wayPoint;
    Vector3 homePosition;
    float posY;
    private float distanceToPlayer;
    public enum state { patrol, chase, attack, idle, home, backup };
    private state _state;
    private Transform player;
    private bool canSeePlayer = false, canAttackPlayer = false, wayPointIsSet = false;
    bool playerInSightRange, playerInAttackRange, obstacleRange;
    public float speed =1, wayPointDistance;
    float previousSpeed;
    bool wentTooFar = false;
    bool canChangeState = true;
   
    Animator animator;
    public SkinnedMeshRenderer meshReference;
    
    

    void Start()
    {
        animator = GetComponent<Animator>();
        meshReference = GetComponentInChildren<SkinnedMeshRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        scale = scale * Random.Range(.5f, 1.5f);
        transform.localScale = new Vector3(scale,scale,scale);
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
        _state = state.patrol;
        boundSize =(meshReference.bounds.size.magnitude);

        animationMultiplier = (speed * animationMultiplier) / boundSize;
        animator.SetFloat("AnimationSpeed", animationMultiplier);
        wayPointDistance = boundSize * 5f;
        homePosition = transform.parent.position;
        previousSpeed = speed;
        

    }
    private void FixedUpdate()
    {
        
    }
    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        obstacleRange = Physics.CheckSphere(transform.position, attackRange, obstacleLayer);
        switch (_state)
        {
            //in loop
            case state.patrol:
                PatrolState();
                break;
            case state.chase:
                ChaseState();
                break;
            case state.attack:
                AttackState();
                break;
            //out of loop
            case state.idle:
                IdleState();
                break;
            case state.home:
                GoHomeState();
                break;
            case state.backup:
                BackUpState();
                break;
        }
    }
    void PatrolState()
    {
        if (!patrol) NextState();
        RandomWaypoint();
        
        Move(previousSpeed);
        if (canChangeState)
        {         
            Debug.Log("patrol");
            canChangeState = false;
            animator.SetFloat("Blend", 1f);
        }
        if (playerInSightRange || playerInAttackRange) NextState();
        

        if (RandomBool(500))
        {
            canChangeState = true;
            _state = state.idle;
        }
        if (WentTooFar())
        {
            canChangeState = true;
            _state = state.home;
        };
    }
    void GoHomeState()
    {
        
        Move(previousSpeed);

        wayPoint = homePosition;
        if (canChangeState)
        {
            
            Debug.Log("go home");
            canChangeState = false;
            animator.SetFloat("Blend", 1f);        
        }
        if (playerInSightRange || playerInAttackRange || RandomBool(100)) NextState();
    }
    
    bool WentTooFar()
    {
        float distanceToHome = Vector3.Distance(homePosition, transform.position);
        if (distanceToHome >= maxRange || transform.position.y >= maxAltitude || transform.position.y <= minAltitude)
        {
            return true;
            
        }
        else
        {
            return false;
        } 
        
    }
    void ChaseState()
    {     
        if (!chase) NextState();
        wayPoint = player.position;
        Move(previousSpeed * 1.2f);
        if (canChangeState)
        {
            Debug.Log("chase");
            canChangeState = false;
            animator.SetFloat("Blend", 1f);
        }
        if (!playerInSightRange || playerInAttackRange) NextState();
        if (WentTooFar() || obstacleRange)
        {
            canChangeState = true;
            _state = state.home;
        };       
    }
    void AttackState()
    {
        if (!attack) NextState();      
        if (!playerInAttackRange) NextState();


        Move(attackSpeed);
        if (canChangeState)
        {          
            Debug.Log("attack");
            animator.SetTrigger("Attack");
            canChangeState = false;
            animator.SetFloat("Blend", 0f);
            Invoke(nameof(AttackStateReset), .1f);
        }
        
        if (WentTooFar() || obstacleRange)
        {
            canChangeState = true;
            _state = state.home;
        };
    }
    void AttackStateReset()
    {
        canChangeState = true;
        _state = state.backup;
    }
    void BackUpState()
    {
        Move(-previousSpeed);
        if (canChangeState)
        {
            
            Debug.Log("backup");
            canChangeState = false;
            animator.SetFloat("Blend", -1f);
            Invoke(nameof(BackupStateReset), .5f);
        }
    }
    void BackupStateReset()
    {  
        NextState();
    }
    void IdleState()
    {
        if (!idle) NextState();
        Move(0f);
        
        if (canChangeState)
        {
            animator.SetFloat("Blend", 0f);
            Debug.Log("idle");
            canChangeState = false;        
            Invoke(nameof(IdleStateReset), 2f);
        }
        
        if (playerInSightRange || playerInAttackRange) NextState();
    }
    void IdleStateReset()
    {
        NextState();
    }
   
    void NextState()
    {
        canChangeState = true;
        _state++;
        if ((int)_state >= 3)
        {
            _state = state.patrol;
        }    
    }
    void Move(float speed)
    {
        if (canSwimOrFly)
        {
            var qto = Quaternion.LookRotation(wayPoint - transform.position).normalized;
            qto = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            transform.rotation = qto;

            transform.position += transform.forward * Time.deltaTime * speed;
        }
        else
        {
            HugTheGround();
            Vector3 newWay = new Vector3(wayPoint.x, transform.position.y, wayPoint.z);
            var qto = Quaternion.LookRotation(newWay - transform.position).normalized;
            qto = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            transform.rotation = qto;
            transform.position += transform.forward * Time.deltaTime * speed;       
        }
    }
    void HugTheGround()
    {
        Vector3 position = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z),
            -transform.up, out hit, 20, whatIsGround))
        {
            targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / 0.15f);
            position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .02f;
            transform.position = position;
        }
    }
    void RandomWaypoint()
    {
        if (wayPointIsSet) { return; }
        wayPointIsSet = true;       
        float randomX = Random.Range(-wayPointDistance, wayPointDistance);
        float travelTime = Random.Range(1f, 3f);
        Invoke(nameof(ResetWaypoint), travelTime);
        if (canSwimOrFly)
        {
            posY = Random.Range(-wayPointDistance*.5f, wayPointDistance*.5f);
            wayPoint = new Vector3(transform.position.x + randomX, transform.position.y + posY, transform.position.z) + transform.forward * wayPointDistance;
          
        }
        else
        {
            posY = Terrain.activeTerrain.SampleHeight(wayPoint);
            wayPoint = new Vector3(transform.position.x + randomX, posY, transform.position.z) + transform.forward * wayPointDistance;
    
        }
    }
    void ResetWaypoint()
    {
        wayPointIsSet = false;       
    }
    static bool RandomBool(int rn)
    {
        int rnd = Random.Range(0, rn);
        if (rnd == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnDrawGizmos()
    {         
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, boundSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, maxAltitude, transform.position.z), new Vector3(maxRange*2f, .01f, maxRange*2f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, minAltitude, transform.position.z), new Vector3(maxRange*2f, .01f, maxRange*2f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, (minAltitude+maxAltitude)/2, transform.position.z - maxRange), new Vector3(maxRange *2f, maxAltitude - minAltitude, .01f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, (minAltitude + maxAltitude) / 2, transform.position.z + maxRange), new Vector3(maxRange*2f, maxAltitude - minAltitude, .01f));
        Gizmos.DrawWireSphere(wayPoint, .5f);
        Gizmos.color = Color.red;    
    }
}
