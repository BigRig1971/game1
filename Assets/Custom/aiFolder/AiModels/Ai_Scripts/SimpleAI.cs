using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SimpleAI : MonoBehaviour, IDamagable


{
    Vector3 prevPos;
    Vector3 currVel;
    public AudioSource randomSound;
    public AudioSource attackSound;
    public bool patrol = true, chase = true, attack = true, idle = true;
    public float scale = 1;
    Quaternion targetRot;

    Vector3 homePosition;
    float posY = 0f;
    public bool canSwimOrFly = false;

    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, obstacleLayer;


    //Patroling
    public float turnSpeed;
    float previousSpeed, speed;

    Vector3 wayPoint;
    bool wayPointSet = false;
    
    //Attacking
    public float timeBetweenAttacks;
    bool canAttack = true;
    public GameObject projectile;

    //States
    public float sightRange, attackRange, ObstacleHit, maxRange, maxAltitude, minAltitude;
    bool playerInSightRange, playerInAttackRange, obstacle, OutOfRange;
    //animation
    Animator animator;
    bool changeAnim = false;
    Vector3 CreatureSize;

    public float Armor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    public float Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        
    }

    private void Start()
    {
     
        StartCoroutine(CalcVelocity());
        //transform.localScale = new Vector3(scale, scale, scale) * Random.Range(.5f, 2f);
       
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;    
        speed = ObstacleHit * 2 * Random.Range(.9f, 1.2f);
        homePosition = transform.parent.position;
        previousSpeed = speed;
    } 

    private void Update()
    {
        AiController();

    }
    private void FixedUpdate()
    {
        RandomSound();
        Move();
        
    }

    private void Patrolling()
    {
        speed = previousSpeed;

        if (!wayPointSet)
        {

            WayPoint();
        }
    }

    void WayPoint()
    {
        wayPointSet = true;
        float distance = Vector3.Distance(transform.position, wayPoint);
        float randomX = Random.Range(-sightRange/2f, sightRange/2f);
        float travelTime = Random.Range(1f, 3f);

        if (canSwimOrFly)
        {
            posY = Random.Range(-sightRange, sightRange);
            wayPoint = new Vector3(transform.position.x + randomX, transform.position.y + posY, transform.position.z) + transform.forward * sightRange;


            if (distance < .3f) wayPointSet = false;
            Invoke(nameof(ResetWaypoint), travelTime);

        }
        else
        {
            posY = Terrain.activeTerrain.SampleHeight(wayPoint);
            wayPoint = new Vector3(transform.position.x + randomX, posY, transform.position.z) + transform.forward * sightRange;

            if (distance < .3f) wayPointSet = false;
            Invoke(nameof(ResetWaypoint), travelTime);

        }
    }
    void MoveBack(float move)
    {
        wayPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z) -transform.forward * move;
        SetSpeed(previousSpeed);
    }
    void ResetWaypoint()
    {
        wayPointSet = false;
    }

    void ChasePlayer()
    {
        SetSpeed(previousSpeed * 1.5f);
        wayPoint = player.transform.position;
    }

    void AttackPlayer()
    {
        SetSpeed (0);
        if (canAttack)
        {
            canAttack = false;
            SetSpeed(0);
            StartCoroutine(AttackRoutine());
            /* ///Attack code here
             Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
             rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
             rb.AddForce(transform.up * 8f, ForceMode.Impulse);
             ///End of attack code*/
            
            
            
        }
    }
    IEnumerator Idle()
    {
        SetSpeed(0);
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        SetSpeed(previousSpeed);
    }
    IEnumerator AttackRoutine()
    {
        attackSound?.Play();
        animator.SetTrigger("Attack");
        while (!canAttack)
        {
            yield return new WaitForSeconds(.2f);
            MoveBack(5);
            yield return new WaitForSeconds(timeBetweenAttacks * Random.Range(1f, 2f));
            canAttack = true;
        }
        yield return null;
               
    }
    void SetSpeed(float _speed)
    {
        speed = _speed;     
    }
    void Move()
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
            Vector3 newWay = new Vector3(wayPoint.x, transform.position.y, wayPoint.z);
            var qto = Quaternion.LookRotation(newWay - transform.position).normalized;
            qto = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            transform.rotation = qto;
            transform.position += transform.forward * Time.deltaTime * speed;
            HugTheGround();
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

    void AiController()
    {
        float dist = Vector3.Distance(homePosition, transform.position);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);


        obstacle = Physics.CheckSphere(transform.position, ObstacleHit, obstacleLayer);
        if (!playerInSightRange && !playerInAttackRange && patrol) Patrolling();
        if (!playerInSightRange && !playerInAttackRange && idle && RandomBool(1000)) StartCoroutine(Idle());
        if (playerInSightRange && !playerInAttackRange && chase && dist <= maxRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && attack && dist <= maxRange) AttackPlayer();
        if (obstacle && !playerInAttackRange && !playerInSightRange) Home();
        if (dist >= maxRange) Home();
        if (transform.position.y >= maxAltitude && !playerInSightRange) Home();
        if (transform.position.y <= minAltitude && !playerInSightRange) Home();
    }
    void Home()
    {
        SetSpeed(previousSpeed * 1.5f);
        wayPoint = homePosition;
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
    void RandomSound()
    {
        if (RandomBool(500)) randomSound?.Play();
    }
    bool TimerReset()
    {
        return true;
    }
   
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.parent.position, maxRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, ObstacleHit);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(wayPoint, .5f);
    }
   
    IEnumerator CalcVelocity()
    {
        while (Application.isPlaying)
        {
            prevPos = transform.position;
            yield return new WaitForEndOfFrame();     
            currVel = (prevPos - transform.position) / Time.deltaTime;      
            animator.SetFloat("Blend", currVel.sqrMagnitude);
        }
    }

    public void Damage(float amount)
    {
        throw new System.NotImplementedException();
    }
}



