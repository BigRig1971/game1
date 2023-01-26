using System.Collections;
using UnityEngine;
namespace StupidHumanGames
{
    public class LootStuff : MonoBehaviour
    {
        bool canDo = true;
        [SerializeField] float lootDelay = .3f;
        [SerializeField] LayerMask mask;
        [SerializeField] float colliderSize = 1f;
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
			HitColliders();
		}
		
		void HitColliders()
		{
			Collider[] hitColliders = Physics.OverlapSphere(transform.position, colliderSize, mask);

			int i = 0;
			while (i < hitColliders.Length)
			{
				lootableItem = hitColliders[i].GetComponent<LootableItem>();
				droppedItemPickup = hitColliders[i].GetComponent<ItemPickupable>();
                if (lootableItem != null && !lootableItem.isDamagable)
                {
                    if (!canDo) return;
                    canDo = false;
					anim.SetTrigger("PickupItem");
					Invoke(nameof(LootItem), lootDelay);
                }
                if (droppedItemPickup != null)
                {
					if (!canDo) return;
					canDo = false;
					anim.SetTrigger("PickupItem");
					Invoke(nameof(PickupItem), lootDelay);
                }
				i++;
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
