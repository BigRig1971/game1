using FirstGearGames.SmoothCameraShaker;
using StupidHumanGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StupidHumanGames
{
	public class EnemyAttackAnimTrigger : MonoBehaviour
	{

		[SerializeField] AudioSource _audioSource;
		Rigidbody rb;
		public Vector3 colliderSize = Vector3.one;
		public Vector3 colliderCenter = Vector3.zero;
		int damagePower = 5;
		public string _animTriggerName;
		public string _animIntName;
		[SerializeField] Animator _animator;
		bool canHit = false;
		bool canAttack = false;
		AudioClip attackSound;
		float attackSoundVolume = 1;
		[System.Serializable]
		public class Ability
		{
			public int _attackInt;
			public float delayCollider = .3f;
			public int _attackPower = 5;
			public AudioClip attackAudio;
		}
		public List<Ability> abilities;
		private void Start()
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			boxCollider.size = colliderSize;
			boxCollider.center = (Vector3.zero + colliderCenter);
			boxCollider.isTrigger = true;
		}
		private void OnTriggerEnter(Collider other)
		{
			if (other.TryGetComponent<DamageInterface>(out var damage) && canHit)
			{
				canHit = false;
				damage.Damage(damagePower);
				_animator.SetTrigger("Interrupt");
			}
		}
		public void OnAttack(int _int)
		{
			foreach (var ability in abilities)
			{
				if (ability._attackInt == _int)
				{
					if (canAttack) return;
					damagePower = ability._attackPower;
					attackSound = ability.attackAudio;
					StartCoroutine(Attacking(ability));
				}
			}
		}
		void TriggerAction(Ability ability)
		{
			if (_animIntName != null) _animator.SetInteger(_animIntName, ability._attackInt);
			if (_animTriggerName != null) _animator.SetTrigger(_animTriggerName);
		}
		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero + colliderCenter, colliderSize);
		}
		IEnumerator Attacking(Ability ability)
		{
			canAttack = true;
			TriggerAction(ability);
			if (InventoryManager.IsOpen()) yield break;
			yield return new WaitForSeconds(ability.delayCollider);
			canHit = true;
			if (attackSound != null) _audioSource?.PlayOneShot(attackSound, attackSoundVolume);
			yield return new WaitForSeconds(.5f);
			canHit = false;
			canAttack = false;
		}
	}
}


