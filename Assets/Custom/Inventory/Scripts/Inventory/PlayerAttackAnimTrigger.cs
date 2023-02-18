using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.InputSystem.Utilities;


namespace StupidHumanGames
{
	public class PlayerAttackAnimTrigger : MonoBehaviour
	{
		enum weaponType { Machete, PickAxe, Hatchet, Axe };
		[SerializeField] weaponType currentWeaponType;
		[SerializeField] Transform hitPoint;
		[SerializeField] LayerMask mask;
		[SerializeField] bool groundHugging = false;
		[SerializeField] Transform cameraRoot;
		[SerializeField] float cameraRootOffset;
		[SerializeField] AudioSource _audioSource;
		[SerializeField] float _globalCooldown = .5f;
		[SerializeField] GameObject[] clothesToRemove;
		[SerializeField] Vector3 colSize = Vector3.one;
		int damagePower = 5;
		public string _animTriggerName;
		public string _animIntName;
		public string _animBoolName;
		public Transform player;
		[SerializeField] Animator _animator;
		[SerializeField] ThirdPersonController _tpc;
		bool canHit = false;
		bool canSwing = true;
		public AudioClip weaponSwingSound;
		[SerializeField, Range(0f, 1f)] float swingVolume = 1;
		bool toggle = false;
		float previousCamOffset;
		[System.Serializable]
		public class Ability
		{
			public int _int;
			public float swingTime = .1f;
			public float hitTime = .4f;
			public float animationLength = 1f;
			public int attackPower = 5;
		}
		public List<Ability> abilities;
		private void Start()
		{
			_tpc = GameObject.FindObjectOfType<ThirdPersonController>();
			previousCamOffset = cameraRoot.transform.position.y;
		}
		private void Update()
		{
			KeyPress();
			BoolAction();
		}
		void HitColliders()
		{
			Collider[] hitColliders = Physics.OverlapBox(hitPoint.position, colSize, Quaternion.identity, mask);
			foreach (var other in hitColliders)
			{
				if (!other.TryGetComponent<DamageInterface>(out var damage)) return;
				damage.Damage(damagePower);
				canSwing = true;
				_animator.SetTrigger("Interrupt");
			}
		}
		void KeyPress()
		{

			if (InventoryManager.IsOpen()) return;

			if (currentWeaponType == weaponType.Machete)
			{
				foreach (var ability in abilities)
				{
					if (!Input.GetKey(KeyCode.Mouse0)) return;
					if (!canSwing) return;
					canSwing = false;
					TriggerAction(ability);
					_animator.SetTrigger("Machete");
					if (ability._int < abilities.Count)
					{
						ability._int++;
					}
					else
					{
						ability._int = 0;
					}
				}
			}
			if (currentWeaponType == weaponType.Axe)
			{
				_animator.SetTrigger("Axe");
			}
			if (currentWeaponType == weaponType.PickAxe)
			{
				_animator.SetTrigger("PickAxe");
			}
			if (currentWeaponType == weaponType.Hatchet)
			{
				_animator.SetTrigger("Hatchet");
			}

		}

		void TriggerAction(Ability ability)
		{
			damagePower = ability.attackPower;
			if (_animIntName != null) _animator.SetInteger(_animIntName, ability._int);
		//	Invoke(nameof(OnSwing), ability.swingTime);
		//	Invoke(nameof(OnHit), ability.hitTime);
		//	Invoke(nameof(OnCoolDown), ability.animationLength);
		}

		void BoolAction()
		{
			if (!Input.GetKeyDown(KeyCode.H)) return;
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
					go.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
				}
			}
		}

		void OnDrawGizmos()
		{
			if (hitPoint == null) return;
			Gizmos.color = Color.red;
			//Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(hitPoint.position, colSize);
		}
		public void OnSwing()
		{
			if (weaponSwingSound != null) _audioSource.PlayOneShot(weaponSwingSound, swingVolume);
		}
		public void OnHit()
		{
			HitColliders();
		}
		public void OnCoolDown()
		{
			canSwing = true;
		}
	}
}
