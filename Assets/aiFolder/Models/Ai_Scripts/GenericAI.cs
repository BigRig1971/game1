using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAI : MonoBehaviour
{ 
    
    Vector3 wayPoint, homePosition ;
    Quaternion targetRot;
    float posY;
    float previousSpeed, wayPointDistance, boundSize;
    bool wentTooFar = false, canChangeState = true;
    bool canSeePlayer = false, canAttackPlayer = false, wayPointIsSet = false;
    Transform player;
    Animator animator;
    Vector3 sizeOfObject = Vector3.one;
    [Range(.1f, 50f)] public float scale = 1f;
    public enum state { patrol, chase, attack, idle };
    [SerializeField] AudioSource[] randomSound;
    [SerializeField] AudioSource attackSound;
    [SerializeField] AudioSource idleSound;
    [SerializeField] AudioSource footStep;
    

    [SerializeField] bool rootMotion = false;
    [SerializeField] bool canSwimOrFly = false, groundHugging = false;
    [SerializeField] bool patrol = true, chase = true, attack = true, idle = true;
    [SerializeField] LayerMask groundLayer, playerLayer, obstacleLayer;
    [SerializeField, Range(0f, 10)] float    turnSpeed = 3f, attackSpeed = 3f, animationSpeed = 3f, moveSpeed = 3f;
    [SerializeField, Range(0f, 50)] float attackRange = 2f;
    [SerializeField, Range(0f, 100)] float sightRange = 5f;
    [SerializeField, Range(0f, 300)] float maxRange = 20f, maxAltitude = 200f, minAltitude = 0f;
    [SerializeField] state _state;
    [SerializeField] SkinnedMeshRenderer meshReference;

    void Start()
    {
        
        boundSize = (GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.z);
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
        _state = state.patrol;     
        animator.SetFloat("AnimationSpeed", animationSpeed);
        wayPointDistance = boundSize * 50f;
        homePosition = transform.parent.position;
        previousSpeed = moveSpeed;
        attackRange = (boundSize / 2) + attackRange;
        sightRange = (boundSize/1.5f) + sightRange;
        sizeOfObject = new Vector3(scale, scale, scale) * Random.Range(.7f, 1.3f);
        transform.localScale = sizeOfObject;

        if (rootMotion)
        {
            animator.applyRootMotion = true;
        }
        else
        {
            animator.applyRootMotion = false;
        }

    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        
        boundSize = (GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.z);
        sizeOfObject = new Vector3(scale, scale, scale);
      
        transform.localScale = sizeOfObject;
        
        
    }
#endif
    private void FixedUpdate()
    {
        if (canSwimOrFly) SwimOrFly(); else MoveOnGround();
        OnHitObstacle();
    }
    void Update()
    {
        switch (_state)
        {
            case state.patrol:
                if (OnCanPatrol()) PatrolState();
                break;
            case state.chase:
                if (OnCanChase()) ChaseState();
                break;
            case state.attack:
                if (OnCanAttack()) AttackState();
                break;
            case state.idle:
                IdleState();
                break;
        }
    }
    bool OnCanAttack()
    {
        if (attack && Physics.CheckSphere(transform.position, attackRange, playerLayer) && attack && Physics.CheckSphere(transform.position, sightRange, playerLayer)) return true; else NextState(); return false;
    }
    bool OnCanChase()
    {
        if (chase && Physics.CheckSphere(transform.position, sightRange, playerLayer) && !Physics.CheckSphere(transform.position, attackRange, playerLayer)) return true; else NextState(); return false;
    }
    bool OnCanPatrol()
    {
        if (patrol && !Physics.CheckSphere(transform.position, sightRange, playerLayer) && !Physics.CheckSphere(transform.position, attackRange, playerLayer)) return true; else NextState(); return false;
    }
    void PatrolState()
    {
        RandomWaypoint();
        if (canChangeState)
        {
            Debug.Log("patrol");
            canChangeState = false;
            moveSpeed = previousSpeed * .5f;
            animator.SetFloat("Blend", .5f);
        }

        if (RandomBool(200))
        {
            float rnd = Random.Range(.5f, 1f);
            moveSpeed = previousSpeed * rnd;
            animator.SetFloat("Blend", rnd);
        }
        if (idle && RandomBool(1500))
        {
            canChangeState = true;
            _state = state.idle;
        }
        if(RandomBool(100))
        {
            RNDSound()?.Play();
        }
        if (WentTooFar())
        {
            wayPoint = homePosition;
            NextState();
        };
        
          
    }
    bool WentTooFar()
    {
        float distanceToHome = Vector3.Distance(homePosition, transform.position);
        if (distanceToHome >= maxRange || transform.position.y > maxAltitude || transform.position.y < minAltitude)
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
        wayPoint = player.position;
        moveSpeed = previousSpeed;
        if (canChangeState)
        {
            animator.SetTrigger("Interrupt");
            Debug.Log("chase");
            canChangeState = false;
            animator.SetFloat("Blend", 1f);
        }
        if (WentTooFar() && canSwimOrFly)
        {
            wayPoint = homePosition;
            NextState();
        };
    }
    void AttackState()
    {
        if (canChangeState)
        {
            moveSpeed = 0f;
            animator.SetFloat("Blend", 0f);
            
            Debug.Log("attack");
            //animator.SetTrigger("Attack");
            
            canChangeState = false;
        }
        if (WentTooFar() && canSwimOrFly)
        {
            wayPoint = homePosition;
            NextState();
        };
        RandomAttack();
        
    }
    void TimedStateReset()
    {
        NextState();
    }
    void IdleState()
    {
        moveSpeed = 0f;
        if (canChangeState)
        {
            RandomIdle();
            animator.SetFloat("Blend", 0f);
            Debug.Log("idle");
            canChangeState = false;
            Invoke(nameof(IdleStateReset), Random.Range(3f, 5f));
        }
        
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
    void SwimOrFly()
    {
        var qto = Quaternion.LookRotation(wayPoint - transform.position).normalized;
        qto = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
        transform.rotation = qto;
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }
    void MoveOnGround()
    {
        float groundSpeed = moveSpeed;
        Vector3 position = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.up.y, transform.position.z),
            -transform.up, out hit, 20, groundLayer))
        {
            if (groundHugging) targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / 0.15f);
            position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .01f;
            transform.position = position;
        }
        Vector3 newWay = new Vector3(wayPoint.x, position.y, wayPoint.z);
        var qto = Quaternion.LookRotation(newWay - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
        if (!rootMotion) transform.position += transform.forward * Time.deltaTime * groundSpeed;
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
            posY = Random.Range(-wayPointDistance * .5f, wayPointDistance * .5f);
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
        int rnd2 = Random.Range(0, rn);
        if (rnd == rnd2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnHitObstacle()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, boundSize, obstacleLayer);
        foreach(Collider col in hitColliders)
        {           
            Vector3 delta = (col.transform.position-transform.position).normalized;
            Vector3 cross = Vector3.Cross(delta, transform.forward);
            if (cross.y > 0) Debug.Log("left"); wayPoint = transform.forward *5 -transform.right * 1f; 
            if (cross.y < 0) Debug.Log("right"); wayPoint = transform.forward * 5+transform.right * 1f;
            // return cross.y;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,(boundSize/2f)+ attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, (boundSize/1.5f)+ sightRange );
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, boundSize);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.parent.position.x, maxAltitude, transform.parent.position.z), new Vector3(maxRange * 2f, .01f, maxRange * 2f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.parent.position.x, minAltitude, transform.parent.position.z), new Vector3(maxRange * 2f, .01f, maxRange * 2f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.parent.position.x, (minAltitude + maxAltitude) / 2, transform.parent.position.z - maxRange), new Vector3(maxRange * 2f, maxAltitude - minAltitude, .01f));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3(transform.parent.position.x, (minAltitude + maxAltitude) / 2, transform.parent.position.z + maxRange), new Vector3(maxRange * 2f, maxAltitude - minAltitude, .01f));
        Gizmos.DrawWireSphere(wayPoint, .5f);
        Gizmos.color = Color.red;
    }
    AudioSource RNDSound()
    {
        if (randomSound.Length == 0f) return null;
        AudioSource rs;
        rs = randomSound[Random.Range(0, randomSound.Length)];
       return rs;
    }
    public void AiFootSteps()
    {
        if (footStep == null) return;
        if (RandomBool(3)) footStep.pitch = Random.Range(.9f, 1f);
        footStep?.Play();
    }
    void RandomIdle()
    {
        if (animator == null) return;
        int rnd = Random.Range(0, 8);
       
        animator.SetInteger("IdleInt", rnd);
        animator.SetTrigger("IdleTrigger");
        Invoke(nameof(TimedStateReset), Random.Range(1f, 2f));
    }
    void RandomAttack()
    {
        if (animator == null) return;
        int rnd = Random.Range(0, 3);
        
        animator.SetInteger("AttackInt", rnd);
        animator.SetTrigger("AttackTrigger");
        Invoke(nameof(TimedStateReset), Random.Range(.3f, .5f));
    }
}
