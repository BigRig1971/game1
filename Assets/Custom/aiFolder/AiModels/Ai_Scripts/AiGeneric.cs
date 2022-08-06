using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StupidHumanGames
{
    public class AiGeneric : MonoBehaviour
    {
        [SerializeField] bool canTame = false;
        AudioSource _audioSource;
        Collider[] hitColliders;
        [SerializeField] Rigidbody rb;
        public Vector3 wayPoint, homePosition;
        Quaternion targetRot;
        float posY;
        float _previousTurnSpeed, _previousSpeed, _wayPointDistance, _blendMultiplier = 1f;
        bool wayPointIsSet = false;
        [SerializeField] Transform player;
        Vector3 sizeOfObject = Vector3.one;
        [Range(.1f, 50f)] public float scale = 1f;

        public enum state { Patrol, Chase, Attack };
        [SerializeField] bool _rigidBody = true;
        [SerializeField] Animator _animator;
        [SerializeField] AudioClip[] randomSound;
        [SerializeField] float randomSoundvolume;
        [SerializeField] AudioClip attackSound;
        [SerializeField] float attackVolume;
        [SerializeField] int _randomIdleDelay = 1000;
        [SerializeField] AudioClip idleSound;
        [SerializeField] float idleVolume;
        [SerializeField] AudioClip[] FootstepAudioClips;
        [SerializeField] float FootstepAudioVolume;
        [SerializeField] bool rootMotion = false;
        [SerializeField] bool canSwimOrFly = false, groundHugging = false;
        [SerializeField] float _groundHuggingTweak = .01f;
        [SerializeField] bool patrol = true, chase = true, attack = true, idle = true, die = true;
        [SerializeField] LayerMask groundLayer, playerLayer, obstacleLayer;
        [SerializeField, Range(0f, 10)] float _turnSpeed = 3f, _moveSpeed = 3f;
        [SerializeField, Range(0f, 10)] float obstacleRadius = 1f;
        [SerializeField] Vector3 attackCenter = Vector3.zero;
        [SerializeField] float _delayAfterAttack = .5f;
        [SerializeField, Range(0f, 50)] float attackRange = 2f;
        [SerializeField, Range(0f, 100)] float sightRange = 5f;
        [SerializeField, Range(0f, 300)] float maxRange = 20f, maxAltitude = 200f, minAltitude = 0f;
        [SerializeField] state _currentState;
        [SerializeField] SkinnedMeshRenderer meshReference;
        [SerializeField, Range(0f, 10f)] int rndAttackCount = 3;
        [SerializeField, Range(0f, 10f)] int rndIdleCount = 3;
        [SerializeField, Range(0f, 5f)] float reverseTime = 2f;
        private void Awake()
        {
            _currentState = state.Patrol;

        }
        private void Update()
        {
            
        }
        private void FixedUpdate()
        {
            if (canSwimOrFly) SwimOrFly(); else MoveOnGround();
            OutOfBounds();
        }

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _previousSpeed = _moveSpeed;
            _previousTurnSpeed = _turnSpeed;
            _animator = GetComponent<Animator>();
            player = FindObjectOfType<ThirdPersonController>().transform;
            transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
            if (_animator != null) _animator.SetFloat("BlendMultiplier", _blendMultiplier);
            _wayPointDistance = 10f;
            homePosition = transform.position;
            sightRange = attackRange + sightRange;
            sizeOfObject = new Vector3(scale, scale, scale) * Random.Range(.9f, 1.1f);
            transform.localScale = sizeOfObject;
            if (rootMotion)
            {
                if (_animator != null) _animator.applyRootMotion = true;
            }
            else
            {
                if (_animator != null) _animator.applyRootMotion = false;
            }
            if (!TryGetComponent<Rigidbody>(out rb) && _rigidBody)
            {
                rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                rb.isKinematic = false;
            }
            StartCoroutine(State());
        }
#if UNITY_EDITOR

        private void OnValidate()
        {
            homePosition = transform.position;
            sizeOfObject = new Vector3(scale, scale, scale);
            transform.localScale = sizeOfObject;
        }
#endif
        #region Movement
        void SwimOrFly()
        {
            var step = _moveSpeed * Time.deltaTime;
            var qto = Quaternion.LookRotation(wayPoint - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * _turnSpeed);
            transform.position = Vector3.MoveTowards(transform.position + transform.forward * .05f, wayPoint, step);
        }
        void MoveOnGround()
        {
            var step = _moveSpeed * Time.deltaTime;
            OnYPosition();
            Vector3 newWay = new Vector3(wayPoint.x, transform.position.y, wayPoint.z);
            var qto = Quaternion.LookRotation(newWay - transform.position).normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * _turnSpeed);

            if (!rootMotion) transform.position = Vector3.MoveTowards(transform.position + transform.forward * .05f, wayPoint, step);
        }
        void OnYPosition()
        {
            Vector3 position = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.up.y, transform.position.z),
                -transform.up, out hit, 20, groundLayer))
            {
                if (groundHugging)
                {
                    targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / _groundHuggingTweak);
                }
                position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .01f;
                transform.position = position;
            }
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
        #endregion
        #region RNG
        void GetRandomWaypoint()
        {
            if (wayPointIsSet) { return; }
            wayPointIsSet = true;
            StartCoroutine(OnResetWayPoint());
            float randomX = Random.Range(-_wayPointDistance, _wayPointDistance);
            if (canSwimOrFly)
            {
                posY = Random.Range(-_wayPointDistance * .5f, _wayPointDistance * .5f);
                wayPoint = new Vector3(transform.position.x + randomX, transform.position.y + posY, transform.position.z) + transform.forward * _wayPointDistance;
            }
            else
            {
                wayPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z) + transform.forward * _wayPointDistance;
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
            if (_animator != null) _animator.SetInteger("IdleInt", rnd);
            if (_animator != null) _animator.SetTrigger("IdleTrigger");
            return;
        }
        void RandomAttackAnimations()
        {
            int rnd = Random.Range(0, rndAttackCount);
            if (_animator != null) _animator.SetInteger("AttackInt", rnd);
            if (_animator != null) _animator.SetTrigger("AttackTrigger");
            return;
        }
        #endregion
        #region Conditions
        bool OnCanPatrol()
        {
            if (patrol && !Physics.CheckSphere(transform.localPosition, sightRange, playerLayer) && !Physics.CheckSphere(transform.localPosition, attackRange, playerLayer)) return true; else return false;
        }
        bool OnCanChase()
        {
            if (chase && Physics.CheckSphere(transform.localPosition, sightRange, playerLayer) && !Physics.CheckSphere(transform.localPosition, attackRange, playerLayer)) return true; else return false;
        }
        bool OnCanAttack()
        {
            if (attack && Physics.CheckSphere(transform.localPosition, attackRange, playerLayer) && Physics.CheckSphere(transform.localPosition, sightRange, playerLayer)) return true; else return false;
        }
    
        void OutOfBounds()
        {
            
            float XZmax = Vector3.Distance(homePosition, transform.position);
            if (XZmax > maxRange)
            {
                wayPoint = homePosition;
                var restrictedMaxRange = homePosition + (transform.position - homePosition).normalized * maxRange;
                transform.position = new Vector3(restrictedMaxRange.x, transform.position.y, restrictedMaxRange.z);
            }
            else
            {
            }
            if (transform.position.y > maxAltitude)
            {
                wayPoint = homePosition;
                transform.position = new Vector3(transform.position.x, maxAltitude, transform.position.z);
            }
            else
            {
            }
            if (transform.position.y < minAltitude)
            {
                wayPoint = homePosition;
                transform.position = new Vector3(transform.position.x, minAltitude, transform.position.z);
            }
            else
            {
            }
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
            if (OnCanPatrol()) SetAnimation(1, 1, 1); wayPointIsSet = false;
            while (OnCanPatrol())
            {
                OnHitObstacle();
                GetRandomWaypoint();
                float distance = Vector3.Distance(transform.position, wayPoint);
                if (distance < 1f) wayPointIsSet = false;
                if (RandomBool(200))
                {
                    if (rootMotion)
                    {
                        float rnd = Random.Range(.5f, 1f);
                        SetAnimation(rnd, 1, 1);
                    }
                    else
                    {
                        float rnd = Random.Range(.8f, 1f);
                        SetAnimation(rnd, rnd, 1);
                    }
                }
                if (RandomBool(_randomIdleDelay) && idle)
                {
                    RandomIdleAnimations();
                }
                yield return null;
            }
            _currentState = state.Chase;
        }
        IEnumerator Chase()
        {
            if (OnCanChase()) SetAnimation(1, 1, 2);
            while (OnCanChase())
            {
                OnHitObstacle();
               wayPoint = player.position;
                if (RandomBool(200))
                {
                    if (RNDSound() != null) _audioSource.PlayOneShot(RNDSound(), randomSoundvolume);
                }
                yield return null;
            }
            _currentState = state.Attack;
        }
        IEnumerator Attack()
        {
            if (OnCanAttack())
            {
                if (!IsFacingObject())
                {
                    if (!rootMotion)
                    {
                        SetAnimation(1, 1, 5);
                        RandomAttackAnimations();
                        if (attackSound != null) _audioSource.PlayOneShot(attackSound, attackVolume);
                        yield return new WaitForSeconds(.3f);
                        SetAnimation(1, 1, 2);
                        if (RandomBool(2))
                        {
                            wayPoint = transform.position - transform.right * 100f;
                        }
                        else
                        {
                            wayPoint = transform.position + transform.right * 100f;
                        }

                        yield return new WaitForSeconds(_delayAfterAttack);
                        SetAnimation(1, 1, 1);
                        yield break;
                    }
                    else
                    {
                        SetAnimation(1, 1, 5);
                        RandomAttackAnimations();
                        if (attackSound != null) _audioSource.PlayOneShot(attackSound, attackVolume);
                        SetAnimation(1, 1, 2);
                       
                        yield return new WaitForSeconds(.3f);
                        SetAnimation(1, -1, 1);
                        
                        yield return new WaitForSeconds(_delayAfterAttack);

                        SetAnimation(1, 1, 1);
                        
                        yield break;
                    }
                }
            }
            while (OnCanAttack())
            {

                wayPoint = player.position;
                yield return null;
            }
            _currentState = state.Patrol;
        }
        void SetAnimation(float blend, float moveSpeedMultiplier, float turnSpeedMultiplier)
        {
            _animator.SetFloat("Blend", 1 * blend);

            _moveSpeed = _previousSpeed * moveSpeedMultiplier;
            _animator.SetFloat("BlendMultiplier", _moveSpeed);
            _turnSpeed = _previousTurnSpeed * turnSpeedMultiplier;
        }
        IEnumerator OnResetWayPoint()
        {
            float distance = Vector3.Distance(transform.position, wayPoint);
            yield return new WaitForSeconds(distance / 3);
            wayPointIsSet = false;
        }
        #endregion
        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(new Vector3(transform.position.x, maxAltitude, transform.position.z), .2f);
            Gizmos.DrawSphere(new Vector3(transform.position.x, minAltitude, transform.position.z), .2f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(wayPoint, .5f);
            Gizmos.color = Color.red;
           // Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, obstacleRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange + sightRange);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(homePosition, maxRange);
        }
        #endregion
        private void OnFootstep(AnimationEvent OnfootStep)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                if (OnfootStep.animatorClipInfo.weight > .8f) _audioSource.PlayOneShot(FootstepAudioClips[index], FootstepAudioVolume);
            }
        }
        private bool IsFacingObject()
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 toOther = transform.position - wayPoint;
            if (Vector3.Dot(forward, toOther) < 0)
            {
                return false; //behind
            }
            else
            {
                return true; //in front
            }

        }

    }
}

