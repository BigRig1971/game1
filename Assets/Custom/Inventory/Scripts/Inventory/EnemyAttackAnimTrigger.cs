using FirstGearGames.SmoothCameraShaker;
using StupidHumanGames;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace StupidHumanGames
{
	public class EnemyAttackAnimTrigger : MonoBehaviour
	{
		[SerializeField] AudioSource _audioSource;
		[SerializeField] LayerMask playerMask;
		DamageInterface _damage;
		[SerializeField] float colliderSize = 1f;
		int damagePower = 5;
		[SerializeField] Vector3 colliderCenter;
		[SerializeField] string _animTriggerName;
		public string _animIntName;
		public float animLength;
		public float afterAnimDelay;
		[SerializeField] Animator _animator;
		AudioClip attackSound;
		float attackSoundVolume = 1;
		[System.Serializable]
		public class Ability
		{
			public int _attackInt;
			public float _animLength = 1f;
			public float _animMultiplier = 1f;
			public float _soundDelay = .3f;
			public float _hitTime = .5f;
			public float _afterAnimDelay = .1f;
			public int _attackPower = 5;
			public AudioClip attackAudio;
		}
		public List<Ability> abilities;


		void HitColliders()
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward * colliderCenter.z, colliderSize, playerMask);
			foreach (var col in hitColliders)
			{
				_damage = col.GetComponent<DamageInterface>();
			}
		}
		void OnDrawGizmos()
		{
			Gizmos.color = Color.red;
			//Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireSphere(transform.position + transform.forward * colliderCenter.z, colliderSize);
		}
		public void OnAttack(int _int)
		{
			
			foreach (var ability in abilities)
			{
				
				if (ability._attackInt == _int)
				{
					damagePower = ability._attackPower;
					attackSound = ability.attackAudio;
					animLength = ability._animLength;
					afterAnimDelay= ability._afterAnimDelay;
					StartCoroutine(Attacking(ability));
				}
			}
		}
		void TriggerAction(Ability ability)
		{
			if (_animIntName != null) _animator.SetInteger(_animIntName, ability._attackInt);
			if (_animTriggerName != null) _animator.SetTrigger(_animTriggerName);
		}
		IEnumerator Attacking(Ability ability)
		{
			if (InventoryManager.IsOpen()) yield break;
			
			TriggerAction(ability);
			yield return new WaitForSeconds(ability._soundDelay / ability._animMultiplier);
			if (attackSound != null) _audioSource?.PlayOneShot(attackSound, attackSoundVolume);
			yield return new WaitForSeconds((ability._hitTime / ability._animMultiplier)- (ability._soundDelay / ability._animMultiplier));
			HitColliders();
			_damage?.Damage(damagePower);
			_damage = null;
		}
	}
}


