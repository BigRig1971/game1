using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace StupidHumanGames
{
	public class AiGeneric : MonoBehaviour
	{
		[SerializeField] EnemyAttackAnimTrigger attackAnimTrigger;
		[SerializeField] bool canTame = false;
		AudioSource _audioSource;
		Collider[] hitColliders;
		Vector3 wayPoint, homePosition;
		Quaternion targetRot;
		float posY;
		float _previousTurnSpeed, _previousSpeed, _wayPointDistance, _blendMultiplier = 1f;
		bool wayPointIsSet = false;
		[SerializeField] Transform playerAttackPoint;
		public enum state { Patrol, Chase, Attack, Lost, Idle };
		[SerializeField] float scale = 1f;
		[SerializeField] bool randomScale;
		[SerializeField] float minScale = 1f;
		[SerializeField] float maxScale = 1f;
		[SerializeField] float meshSize;
		[SerializeField] Animator _animator;
		[SerializeField] AudioClip[] randomSound;
		[SerializeField] float randomSoundvolume;
		[SerializeField] AudioClip idleSound;
		[SerializeField] float idleVolume;
		[SerializeField] AudioClip[] FootstepAudioClips;
		[SerializeField] float FootstepAudioVolume;
		[SerializeField] bool rootMotion = false;
		[SerializeField] bool canSwimOrFly = false, groundHugging = false;
		[SerializeField] float _groundHuggingTweak = .01f;
		[SerializeField] bool patrol = true, chase = true, attack = true, idle = true, die = true;
		[SerializeField] LayerMask groundLayer, playerLayer, obstacleLayer;
		[SerializeField] float _turnSpeed = 3f, _moveSpeed = 3f;
		[SerializeField] float _attackRange = 2f;
		[SerializeField] float _obstacleRange = 3f;
		[SerializeField] float _sightRange = 5f;
		[SerializeField] float _maxRange = 10f;
		float attackRange;
		float obstacleRange;
		float sightRange;
		float maxRange;
		[SerializeField] float maxAltitude = 200f, minAltitude = 0f;
		[SerializeField] state _currentState;
		[SerializeField] int rndAttackCount = 3;
		[SerializeField] int rndIdleCount = 3;
		float m_internalTimer = 1f;
		private void Awake()
		{
			
			_currentState = state.Patrol;
		}
		void Start()
		{
			SetScale();
			
			if(playerAttackPoint == null) playerAttackPoint = FindObjectOfType<ThirdPersonController>().transform;
			_audioSource = GetComponent<AudioSource>();
			_previousSpeed = _moveSpeed;
			_previousTurnSpeed = _turnSpeed;
			if(_animator == null) _animator = GetComponent<Animator>();

			transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0f, 360f), 0));
			if (_animator != null) _animator.SetFloat("BlendMultiplier", _blendMultiplier);
			_wayPointDistance = 10f;

			if (rootMotion)
			{
				if (_animator != null) _animator.applyRootMotion = true;
			}
			else
			{
				if (_animator != null) _animator.applyRootMotion = false;
			}
			StartCoroutine(State());
		}
		private void Update()
		{
			if (canSwimOrFly) SwimOrFly(); else MoveOnGround();

			OnHitObstacle();
			OutOfBounds();
		}
		
		void SetScale()
		{
			homePosition = GetComponent<Transform>().position;
			if (!randomScale)
			{
				transform.localScale = Vector3.one * scale;
			}
			else
			{
				transform.localScale = Vector3.one * (Random.Range(minScale, maxScale));
			}
		
			meshSize = GetComponentInChildren<SkinnedMeshRenderer>().bounds.size.magnitude / 2;

			attackRange = _attackRange * meshSize;
			obstacleRange = _obstacleRange * meshSize;	
			sightRange= _sightRange * meshSize;
			maxRange = _maxRange * meshSize;
		}
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
			Gizmos.DrawWireSphere(transform.position, obstacleRange);
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, attackRange);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, sightRange);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(homePosition, maxRange);
		}
#if UNITY_EDITOR

		private void OnValidate()
		{
			//SetScale();
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
			var qto = Quaternion.LookRotation(wayPoint - transform.position).normalized;
			transform.rotation = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * _turnSpeed);

			if (!rootMotion) transform.position = Vector3.MoveTowards(transform.position + transform.forward * .05f, wayPoint, step);
		}
		void OnYPosition()
		{
			Vector3 position = transform.position;
			RaycastHit hit;
			if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + .2f, transform.position.z),
				-transform.up, out hit, 2, groundLayer))
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
	
		bool OnHitObstacle()
		{
			bool _hit = false;
			hitColliders = Physics.OverlapSphere(transform.position + transform.up, obstacleRange, obstacleLayer);
			foreach (Collider col in hitColliders)
			{
				_hit = true;
				if (canSwimOrFly)
				{
					if (col.transform.position.y <= OnGround(transform.position).y)
					{
						transform.position += new Vector3(0, .1f, 0);
						wayPoint = transform.position + transform.forward * 10 + transform.up * 10; //up
					}
				}
				else
				{
					
					Vector3 delta = (col.ClosestPoint(transform.position) - transform.position).normalized;
					Vector3 cross = Vector3.Cross(delta, transform.forward);
					if (cross.y > 0f && cross.y < .5f) wayPoint = transform.position + transform.forward * 10 + transform.right * 10; //left
					if (cross.y < 0f && cross.y > -.5f) wayPoint = transform.position + transform.forward * 10 - transform.right * 10; //right
				}
			}
			return _hit;
		}
		Vector3 OnGround(Vector3 position)
		{
			Vector3 _pos = position;
			_pos.y = Terrain.activeTerrain.SampleHeight(_pos);
			return _pos;
		}
		#endregion
		#region RNG

		void GetRandomWaypoint()
		{
			if (wayPointIsSet) { return; }
			wayPointIsSet = true;
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
		void ResetWayPoint()
		{

			float distance = Vector3.Distance(transform.position, wayPoint);
			if (distance < 1f) wayPointIsSet = false;
			m_internalTimer -= Time.deltaTime;
			m_internalTimer = Mathf.Max(m_internalTimer, 0f);
			if (m_internalTimer == 0f)
			{
				wayPointIsSet = false;
				m_internalTimer = distance / 3f;

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
		}
		void RandomAttackAnimations()
		{
			int rnd = Random.Range(0, rndAttackCount);
			attackAnimTrigger.OnAttack(rnd);
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
		bool OutOfBounds()
		{
			bool _outOfBounds = false;
			float distance = Vector3.Distance(homePosition, transform.position);
			if (distance > maxRange)
			{
				wayPoint = homePosition;
				_outOfBounds = true;

				var restrictedMaxRange = homePosition + (transform.position - homePosition).normalized * maxRange;
				transform.position = new Vector3(restrictedMaxRange.x, transform.position.y, restrictedMaxRange.z);
			}
			if (transform.position.y > maxAltitude)
			{
				wayPoint = homePosition;			
				_outOfBounds = true;

				transform.position = new Vector3(transform.position.x, maxAltitude, transform.position.z);
			}
			if (transform.position.y < minAltitude)
			{
				wayPoint = homePosition;
				_outOfBounds = true;

				transform.position = new Vector3(transform.position.x, minAltitude, transform.position.z);
			}
			return _outOfBounds;
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
			yield return new WaitForSeconds(2f);
			wayPointIsSet = false;
			if (OnCanPatrol()) SetAnimation(1, 1, 1);
			while (OnCanPatrol() && !OutOfBounds())
			{
				GetRandomWaypoint();
				ResetWayPoint();
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
				if (RandomBool(300) && idle)
				{
					yield return new WaitForEndOfFrame();
					_currentState = state.Idle;
					yield break;
				}
				yield return null;
			}
			_currentState = state.Chase;
		}
		IEnumerator Chase()
		{
			
			if (OnCanChase()) SetAnimation(1, 1, 2);
			while (OnCanChase() && !OnHitObstacle())
			{
				if (OutOfBounds())
				{
					SetAnimation(0, 1, 0);
				}
				wayPoint = playerAttackPoint.position;
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
			while (OnCanAttack() && IsFacingObject())
			{
				wayPoint = playerAttackPoint.position;
				SetAnimation(0, 0, 3);
				RandomAttackAnimations();
				//yield return new WaitForSeconds(_attackDelay);

				
				if (!canSwimOrFly)
				{
					
					yield return new WaitForSeconds(attackAnimTrigger.animLength);
					yield return new WaitForSeconds(attackAnimTrigger.afterAnimDelay);
					yield break;
					
				}
				else
				{
					yield return new WaitForSeconds(.5f);
					if (RandomBool(2))
					{
						wayPoint = transform.position - transform.right * 100f;
						yield return new WaitForSeconds(attackAnimTrigger.animLength);
						yield return new WaitForSeconds(attackAnimTrigger.afterAnimDelay);
					}
					else
					{
						wayPoint = transform.position + transform.right * 100f;
						yield return new WaitForSeconds(attackAnimTrigger.animLength);
						yield return new WaitForSeconds(attackAnimTrigger.afterAnimDelay);
					}
				}
				SetAnimation(1, 1, 3);
				yield return null;
			}
			
			_currentState = state.Patrol;
		}
		IEnumerator Lost()
		{
			
			SetAnimation(1, 1, 1);
			while (OutOfBounds())
			{
				wayPoint = homePosition;
				yield return null;
			}			
			_currentState = state.Patrol;
		}
		IEnumerator Idle()
		{
			SetAnimation(0, 1, 0);
			RandomIdleAnimations();
			yield return new WaitForSeconds(Random.Range(3, 6));
			_currentState = state.Patrol;
			yield break;

		}
		void SetAnimation(float blend, float moveSpeedMultiplier, float turnSpeedMultiplier)
		{
			_animator.SetFloat("Blend", 1 * blend);
			_moveSpeed = _previousSpeed * moveSpeedMultiplier;
			_animator.SetFloat("BlendMultiplier", _moveSpeed);
			_turnSpeed = _previousTurnSpeed * turnSpeedMultiplier;
		}

		#endregion
		#region Gizmos
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
			Vector3 toOther = transform.position - playerAttackPoint.position;
			if (Vector3.Dot(forward, toOther) > 0)
			{
				return false; //behind
			}
			else
			{
				return true; //in front
			}
		}
		private bool OnIsFacing()
		{
			RaycastHit hit;
			bool canAttack = false;
			if (Physics.Raycast(transform.position + transform.up * 1f, transform.TransformDirection(Vector3.forward), out hit, 10, playerLayer))
			{
				canAttack = true;
				Debug.DrawRay(transform.position + transform.up * 1f, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
			}
			return canAttack;
		}

	}
}

