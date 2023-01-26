using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.Utilities;


namespace StupidHumanGames
{
	public class PlayerAttackAnimTrigger : MonoBehaviour
	{
		[SerializeField] LayerMask mask;
		[SerializeField] bool groundHugging = false;
		[SerializeField] Transform cameraRoot;
		[SerializeField] float cameraRootOffset;
		[SerializeField] AudioSource _audioSource;
		[SerializeField] float _globalCooldown = .5f;
		[SerializeField] GameObject[] clothesToRemove;
		[SerializeField] float colSize = .2f;
		int damagePower = 5;
		public string _animTriggerName;
		public string _animIntName;
		public string _animBoolName;
		public Transform player;
		[SerializeField] Animator _animator;
		[SerializeField] ThirdPersonController _tpc;
		bool canHit = false;
		bool canSwing = false;
		public ShakeData MyShake;
		public AudioClip weaponSwingSound;
		[SerializeField, Range(0f, 1f)] float swingVolume = 1;
		bool toggle = false;
		float previousCamOffset;
		[System.Serializable]
		public class Ability
		{
			public KeyCode _key;
			public int _int;
			public float delayCollider = .3f;
			public int attackPower = 5;
		}
		public List<Ability> abilities;
		private void Start()
		{
			_tpc = GameObject.FindObjectOfType<ThirdPersonController>();
			previousCamOffset = cameraRoot.transform.position.y;
		}
		private void FixedUpdate()
		{
			GetKey();
			HitColliders();
		}
		void HitColliders()
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, colSize, mask);
			foreach (var other in hitColliders)
			{
				if (other.TryGetComponent<DamageInterface>(out var damage))
				{
					if (other.CompareTag("Player")) return;
					if (!canHit) return;
					canHit = false;
					//var _animName = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
					CameraShakerHandler.Shake(MyShake);
					damage.Damage(damagePower);
					_animator.SetTrigger("Interrupt");
				}
			}
		}
		void GetKey()
		{
			foreach (var ability in abilities)
			{
				if (Input.GetKeyDown(ability._key))
				{
					BoolAction();
					if (canSwing) return;
					damagePower = ability.attackPower;
					StartCoroutine(Attacking(ability));
				}
			}
		}
		void TriggerAction(Ability ability)
		{
			if (_animIntName != null) _animator.SetInteger(_animIntName, ability._int);
			if (_animTriggerName != null) _animator.SetTrigger(_animTriggerName);
		}
		void BoolAction()
		{
			if (!groundHugging) return;
			toggle = !toggle;
			if (toggle)
			{
				if (_animBoolName != null) _animator.SetBool(_animBoolName, true);
				cameraRoot.position = new Vector3(cameraRoot.position.x, cameraRoot.position.y + cameraRootOffset, cameraRoot.position.z);
				_tpc._canMove = false;
				foreach (GameObject go in clothesToRemove)
				{
					go.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
				}
			}
			else
			{
				if (_animBoolName != null) _animator.SetBool(_animBoolName, false);
				cameraRoot.position = new Vector3(cameraRoot.position.x, cameraRoot.position.y + cameraRootOffset * -1, cameraRoot.position.z);
				_tpc.transform.rotation = Quaternion.Euler(0f, _tpc.transform.rotation.eulerAngles.y, 0f);
				_tpc._canMove = true;
				foreach (GameObject go in clothesToRemove)
				{
					go.GetComponentInChildren <SkinnedMeshRenderer>().enabled = true;
				}
			}
		}
		
		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireSphere(Vector3.zero, colSize);
		}
		IEnumerator Attacking(Ability ability)
		{
			canSwing = true;
			TriggerAction(ability);
			if (InventoryManager.IsOpen()) yield break;
			yield return new WaitForSeconds(ability.delayCollider);
			canHit = true;
			if (weaponSwingSound == null) yield break;
			_audioSource.PlayOneShot(weaponSwingSound, swingVolume);
			yield return new WaitForSeconds(.5f);
			canHit = false;
			canSwing = false;
		}
	}
}
