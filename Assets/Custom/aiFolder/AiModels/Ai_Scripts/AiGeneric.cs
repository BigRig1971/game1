using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StupidHumanGames
{
    public class AiGeneric : MonoBehaviour
    {
        Collider[] hitColliders;
        [SerializeField] Rigidbody rb;
        bool _moveTowards = true;
        public Vector3 wayPoint, homePosition;
        Quaternion targetRot;
        float posY;
        float previousSpeed, wayPointDistance;
        bool wayPointIsSet = false;
        Transform player;
        Vector3 sizeOfObject = Vector3.one;
        [Range(.1f, 50f)] public float scale = 1f;

        public enum state { Patrol, Chase, Attack, Lost };
        [SerializeField] Animator animator;
        [SerializeField] AudioClip[] randomSound;
        [SerializeField] float randomSoundvolume;
        [SerializeField] AudioClip attackSound;
        [SerializeField] float attackVolume;
        [SerializeField] AudioClip idleSound;
        [SerializeField] float idleVolume;
        [SerializeField] AudioClip footStep;
        [SerializeField] float footStepVolume;
        [SerializeField] bool rootMotion = false;
        [SerializeField] bool canSwimOrFly = false, groundHugging = false;
        [SerializeField] bool patrol = true, chaseOrFlee = true, attack = true, idle = true, die = true;
        [SerializeField, Range(1, 10)] int _ChaseOrFlee = 5;
        [SerializeField] LayerMask groundLayer, playerLayer, obstacleLayer;
        [SerializeField, Range(0f, 10)] float turnSpeed = 3f, attackSpeed = 3f, animationSpeed = 3f, moveSpeed = 3f;
        [SerializeField, Range(0f, 50)] float attackRange = 2f;
        [SerializeField, Range(0f, 100)] float sightRange = 5f;
        [SerializeField] Vector3 obstacleCenter = Vector3.zero;
        [SerializeField, Range(0f, 10)] float obstacleRadius = 1f;
        [SerializeField, Range(0f, 300)] float maxRange = 20f, maxAltitude = 200f, minAltitude = 0f;
        [SerializeField] state _currentState;
        [SerializeField] SkinnedMeshRenderer meshReference;
        [SerializeField, Range(0f, 10f)] int rndAttackCount = 3;
        [SerializeField, Range(0f, 10f)] int rndIdleCount = 3;
        [SerializeField, Range(0f, 5f)] float reverseTime = 2f;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            _currentState = state.Patrol;
            previousSpeed = moveSpeed;
        }
        private void FixedUpdate()
        {
            if (canSwimOrFly) SwimOrFly(); else MoveOnGround();
        }
        void Start()
        {
            StartCoroutine(State());
            player = GameObject.FindGameObjectWithTag("Player").transform;
            transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));

            animator.SetFloat("AnimationSpeed", animationSpeed);
            wayPointDistance = 10f;
            if (canSwimOrFly)
            {
                if (minAltitude > Terrain.activeTerrain.SampleHeight(transform.position))
                {
                    homePosition = new Vector3(transform.position.x, (maxAltitude + minAltitude) / 2, transform.position.z);
                    
                }
                else
                {
                    homePosition = new Vector3(transform.position.x, (maxAltitude + Terrain.activeTerrain.SampleHeight(transform.position)) / 2f, transform.position.z);
                }
            }
            else
            {
                homePosition = transform.parent.position;
            }


            sightRange = attackRange + sightRange;
            sizeOfObject = new Vector3(scale, scale, scale) * Random.Range(.9f, 1.1f);
            transform.localScale = sizeOfObject;
            if (rootMotion)
            {
                animator.applyRootMotion = true;
            }
            else
            {
                animator.applyRootMotion = false;
            }
            if (TryGetComponent<Rigidbody>(out rb))
            {
                rb = GetComponent<Rigidbody>();
            }
        }
#if UNITY_EDITOR

        private void OnValidate()
        {
            sizeOfObject = new Vector3(scale, scale, scale);
            transform.localScale = sizeOfObject;
        }
#endif
        #region Movement
        void SwimOrFly()
        {
            DistanceToWaypoint();
            if (_moveTowards)
            {
                var qto = Quaternion.LookRotation(wayPoint - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            }
            else
            {
                var qto = Quaternion.LookRotation(transform.position - wayPoint).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            }
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        void MoveOnGround()
        {
            OnYPosition();

            DistanceToWaypoint();

            Vector3 newWay = new Vector3(wayPoint.x, transform.position.y, wayPoint.z);
            if (_moveTowards)
            {
                var qto = Quaternion.LookRotation(newWay - transform.position).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            }
            else
            {
                var qto = Quaternion.LookRotation(transform.position - newWay).normalized;
                transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * turnSpeed);
            }
            if (!rootMotion) transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        void OnYPosition()
        {
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
        }
        void DistanceToWaypoint()
        {
            float distance = Vector3.Distance(transform.position, wayPoint);
            if (distance < 1f)
            {
                ResetWaypoint();
            }
        }
        void ResetWaypoint()
        {
            wayPointIsSet = false;
        }
        void OnHitObstacle()
        {
            hitColliders = Physics.OverlapSphere(transform.position + transform.up, obstacleRadius, obstacleLayer);
            foreach (Collider col in hitColliders)
            {
                if (canSwimOrFly)
                {
                    wayPoint = homePosition; return;
                }
                Vector3 delta = (col.ClosestPoint(transform.position) - transform.position).normalized;

                Vector3 cross = Vector3.Cross(delta, transform.forward);
                if (cross.y > 0f && cross.y < .5f) wayPoint = transform.position + transform.forward * 3 + transform.right * 3f; //left
                if (cross.y < 0f && cross.y > -.5f) wayPoint = transform.position + transform.forward * 3 - transform.right * 3f; //right
            }
        }
        void ChaseOrFlee()
        {
            if (!_moveTowards) return;
            int a = Random.Range(1, Random.Range(1, _ChaseOrFlee));
            if (a == 1) _moveTowards = true;
            else if (a != 1 || _ChaseOrFlee == 10) _moveTowards = false;
            Invoke(nameof(OnFleeReset), 3f);
        }
        void OnFleeReset()
        {
            _moveTowards = true;
        }
        #endregion
        #region RNG
        void RandomWaypoint()
        {
            if (wayPointIsSet) { return; }
            wayPointIsSet = true;
            float randomX = Random.Range(-wayPointDistance, wayPointDistance);
            Invoke(nameof(ResetWaypoint), 10f);
            if (canSwimOrFly)
            {
                posY = Random.Range(-wayPointDistance * .5f, wayPointDistance * .5f);
                wayPoint = new Vector3(transform.position.x + randomX, transform.position.y + posY, transform.position.z) + transform.forward * wayPointDistance;
            }
            else
            {
                wayPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z) + transform.forward * wayPointDistance;
                wayPoint.y = Terrain.activeTerrain.SampleHeight(wayPoint);
            }
        }
        bool RandomBool(int rn)
        {
            int rnd = Random.Range(0, rn);
            int rnd2 = Random.Range(0, rn);
            if (rnd == rnd2) return true; else return false;
        }
        AudioClip RNDSound()
        {
            if (randomSound.Length == 0f) return null;
            AudioClip rs;
            rs = randomSound[Random.Range(0, randomSound.Length)];
            return rs;
        }
        void RandomIdleAnimations()
        {
            int rnd = Random.Range(0, rndIdleCount);
            animator.SetInteger("IdleInt", rnd);
            animator.SetTrigger("IdleTrigger");
            return;
        }
        void RandomAttackAnimations()
        {
            int rnd = Random.Range(0, rndAttackCount);
            animator.SetInteger("AttackInt", rnd);
            animator.SetTrigger("AttackTrigger");
            return;
        }
        #endregion
        #region Conditions
        bool OnCanPatrol()
        {
            if (patrol && !OnLost() && !Physics.CheckSphere(transform.position, sightRange, playerLayer) && !Physics.CheckSphere(transform.position, attackRange, playerLayer)) return true; else return false;
        }
        bool OnCanChase()
        {
            if (chaseOrFlee && Physics.CheckSphere(transform.position, sightRange, playerLayer) && !Physics.CheckSphere(transform.position, attackRange, playerLayer)) return true; else return false;
        }
        bool OnCanAttack()
        {
            if (attack && Physics.CheckSphere(transform.position, attackRange, playerLayer) && attack && Physics.CheckSphere(transform.position, sightRange, playerLayer)) return true; else return false;
        }

        bool OnLost()
        {
            float distanceToHome = Vector3.Distance(homePosition, transform.position);
            if (distanceToHome >= maxRange || transform.position.y > maxAltitude || transform.position.y < minAltitude) return true; else return false;
        }
        #endregion
        #region States
        IEnumerator State()
        {
            while (true)
            {
                yield return StartCoroutine(_currentState.ToString());
            }
        }
        IEnumerator Patrol()
        {
            moveSpeed = previousSpeed * .5f;
            animator.SetFloat("Blend", .5f);
            while (OnCanPatrol())
            {

                OnHitObstacle();
                RandomWaypoint();
                if (RandomBool(200))
                {
                    float rnd = Random.Range(.5f, 1f);
                    moveSpeed = previousSpeed * rnd;
                    animator.SetFloat("Blend", rnd);
                }
                if (RandomBool(1000) && idle)
                {
                    RandomIdleAnimations();
                }

                yield return null;
            }
            _currentState = state.Chase;
        }
        IEnumerator Chase()
        {
            moveSpeed = previousSpeed;
            animator.SetFloat("Blend", 1f);

            while (OnCanChase())
            {
                OnHitObstacle();
                if (RandomBool(200))
                {
                    if (RNDSound() != null) AudioSource.PlayClipAtPoint(RNDSound(), transform.position, randomSoundvolume);
                }
                wayPoint = player.position;
                yield return null;
            }
            _currentState = state.Attack;
        }
        IEnumerator Attack()
        {
            _moveTowards = true;
            moveSpeed = 0f;
            animator.SetFloat("Blend", 0f);
            while (OnCanAttack())
            {
                wayPoint = player.position;
                RandomAttackAnimations();
                if (attackSound != null) AudioSource.PlayClipAtPoint(attackSound, transform.position, attackVolume);
                moveSpeed = -1f;

                animator.SetFloat("AnimationSpeed", -1f);
                animator.SetFloat("Blend", 1f);
                yield return new WaitForSeconds(reverseTime * Random.Range(.5f, 1f));
                animator.SetFloat("AnimationSpeed", animationSpeed);
                moveSpeed = 1f;
                yield return null;
            }
            _currentState = state.Lost;
        }
        IEnumerator Lost()
        {
            moveSpeed = previousSpeed * 2f;
            animator.SetFloat("Blend", 1f);
            while (OnLost())
            {
                OnHitObstacle();
                wayPoint = homePosition;
                yield return null;
            }
            _currentState = state.Patrol;
        }
        #endregion
        #region Event Functions
        public void AiFootStepsAudio() //animation trigger
        {

            if (footStep != null) AudioSource.PlayClipAtPoint(footStep, transform.position, footStepVolume);
        }
        public void OnIsDead()
        {
            if (!die) return;
            StopAllCoroutines();
            groundHugging = true;
            animator.SetFloat("Blend", 0f);
            animator.SetInteger("DeathInt", 1);
            animator.SetTrigger("DeathTrigger");
            wayPoint = transform.position;

        }
        public void OnTakeHit()
        {
            animator.SetTrigger("Hit");
        }
        #endregion
        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + obstacleCenter, obstacleRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, (attackRange) + sightRange);
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
        #endregion
        private bool IsFacingObject()
        {
            // Check if the gaze is looking at the front side of the object
            Vector3 forward = transform.forward;
            Vector3 toOther = (wayPoint - transform.position).normalized;
            if (Vector3.Dot(forward, toOther) < 0.7f)
            {
                return false; //not facing
            }

            return true; //facomg
        }
        void OnRandomSound()
        {

        }
    }
}

