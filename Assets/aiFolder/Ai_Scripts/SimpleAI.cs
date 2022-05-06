using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SimpleAI : MonoBehaviour


{
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
    public float speed = 5f, turnSpeed = 5f;
    float previousSpeed;

    public Vector3 wayPoint;
    public bool wayPointSet = false;
    float wayPointRange = 10f;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked = false;
    public GameObject projectile;

    //States
    public float sightRange, attackRange, ObstacleHit, maxRange, maxAltitude, minAltitude;
    public bool playerInSightRange, playerInAttackRange, obstacle, OutOfRange;
    //animation
    Animator animator;


    private void Start()
    {
        transform.localScale = new Vector3(scale, scale, scale) * Random.Range(.5f, 2f);
        speed = speed * Random.Range(.9f, 1.2f);
        homePosition = transform.parent.position;
        previousSpeed = speed;
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        AiController();
        MoveAndRotate();
    }
    private void FixedUpdate()
    {
        RandomSound();

    }

    private void Patrolling()
    {


        if (!wayPointSet)
        {
            ChangeAnimation(previousSpeed);
            WayPoint();
        }
    }

    void WayPoint()
    {
        wayPointSet = true;
        float distance = Vector3.Distance(transform.position, wayPoint);
        float randomX = Random.Range(-wayPointRange, wayPointRange);
        float travelTime = Random.Range(1f, 3f);
        
        if (canSwimOrFly)
        {
            posY = Random.Range(-wayPointRange, wayPointRange);
            wayPoint = new Vector3(transform.position.x + randomX, transform.position.y + posY, transform.position.z) + transform.forward * 5f;
           

            if (distance < .3f) wayPointSet = false;
            Invoke(nameof(ResetWaypoint), travelTime);
           
        }
        else
        {
            posY = Terrain.activeTerrain.SampleHeight(wayPoint);
            wayPoint = new Vector3(transform.position.x + randomX, posY, transform.position.z) + transform.forward * 5f;
           
            if (distance < .3f) wayPointSet = false;
            Invoke(nameof(ResetWaypoint), travelTime);

        }
    }
    void ResetWaypoint()
    {
        wayPointSet = false;
    }

    void ChasePlayer()
    {
        ChangeAnimation(previousSpeed * 2f);
        wayPoint = player.transform.position;
    }

    void AttackPlayer()
    {
        ChangeAnimation(0);

        if (!alreadyAttacked)
        {
            attackSound?.Play();
            /* ///Attack code here
             Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
             rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
             rb.AddForce(transform.up * 8f, ForceMode.Impulse);
             ///End of attack code*/

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks * Random.Range(.8f, 1.2f));
        }
    }
    void Idle()
    {

        if (RandomNumberGenerator(1000) == 5)
        {
            StartCoroutine(IdleTime());
        }

    }
    void ResetAttack()
    {
        alreadyAttacked = false;
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

    void ChangeAnimation(float animSpeed)
    {
        bool changeAnim = true;
        speed = animSpeed;
        if (changeAnim)
        {
            changeAnim = false;
            animator.SetFloat("Blend", speed);
        }
    }
    void MoveAndRotate()
    {

        if (canSwimOrFly)
        {
            var qto = Quaternion.LookRotation(wayPoint - transform.position).normalized;
            qto = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            transform.rotation = qto;
            Debug.Log(qto);
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        else
        {
            AlignWithGround();
            Vector3 newWay = new Vector3(wayPoint.x, transform.position.y, wayPoint.z);
            var qto = Quaternion.LookRotation(newWay - transform.position).normalized;
            qto = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            transform.rotation = qto;
            transform.position += transform.forward * Time.deltaTime * speed;

        }
    }
    void AlignWithGround()
    {
        Vector3 position = transform.position;

        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z),
            -transform.up, out hit, 20, whatIsGround))
        {
            targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / 0.05f);
            position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .01f;
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
        if (!playerInSightRange && !playerInAttackRange && idle) Idle();
        if (playerInSightRange && !playerInAttackRange && chase && dist <= maxRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange && attack && dist <= maxRange) AttackPlayer();
        if (obstacle && !playerInAttackRange && !playerInSightRange) GoToHomePosition();
        if (dist >= maxRange) GoToHomePosition();
        if (transform.position.y >= maxAltitude && !playerInSightRange) GoToHomePosition();
        if (transform.position.y <= minAltitude && !playerInSightRange) GoToHomePosition();
    }
    void GoToHomePosition()
    {
        wayPoint = homePosition;
    }
    IEnumerator IdleTime()
    {
        ChangeAnimation(0f);
        yield return new WaitForSeconds(Random.Range(3f, 10f));
        speed = previousSpeed;
    }
    static int RandomNumberGenerator(int rn)
    {
        return Random.Range(1, rn);
    }
    void RandomSound()
    {
        if (RandomNumberGenerator(200) == 1) randomSound?.Play();
    }

}



