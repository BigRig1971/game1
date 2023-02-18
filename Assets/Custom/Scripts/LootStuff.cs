using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StupidHumanGames
{
	public class LootStuff : MonoBehaviour
	{
		bool canDo = true;
		[SerializeField] float lootDelay = .3f;
		[SerializeField] LayerMask mask;
		[SerializeField] float colliderSize = 1.5f;
		[SerializeField] AudioSource _audioSource;
		public AudioClip lootSound;
		public float lootSoundVolume;
		Animator anim;
		LootableItem lootableItem;
		ItemPickupable droppedItemPickup;
		private void Start()
		{
			anim = GetComponent<Animator>();
		}
		private void FixedUpdate()
		{
			EnableLootableItems();
			PickupLootableItems();
		}
		void EnableLootableItems()
		{
			Collider[] _colliders = Physics.OverlapSphere(transform.position, 10f, mask);

			foreach (Collider collider in _colliders)
			{
				if (collider.TryGetComponent<LootableItem>(out LootableItem healthBar))
				{
					healthBar.enabled = true;
				}
				
			}
		}
		void PickupLootableItems()
		{
			if (!Input.GetKey(KeyCode.E)) return;
			Collider[] _colliders = Physics.OverlapSphere(transform.position, colliderSize, mask);
			foreach (Collider collider in _colliders)
			{
				if (collider.TryGetComponent<LootableItem>(out lootableItem))
				{
					if (lootableItem.isDamagable) return;
					if (!canDo) return;
					canDo = false;
					anim.SetTrigger("PickupItem");
					Invoke(nameof(LootItem), lootDelay);
				}
			}
		}

		void LootItem()
		{
			if (lootSound != null) _audioSource.PlayOneShot(lootSound, lootSoundVolume);
			lootableItem.LootableItems();
			canDo = true;
		}
		void PickupItem()
		{
			if (lootSound != null) _audioSource.PlayOneShot(lootSound, lootSoundVolume);
			droppedItemPickup.LootableItems();
			canDo = true;
		}
	}
}
