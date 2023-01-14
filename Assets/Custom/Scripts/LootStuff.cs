using System.Collections;
using UnityEngine;
namespace StupidHumanGames
{
    public class LootStuff : MonoBehaviour
    {
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
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Lootable"))            
            {
                lootableItem = other.gameObject.GetComponent<LootableItem>();
               
				droppedItemPickup = other.gameObject.GetComponent<ItemPickupable>();
                if (lootableItem != null && !lootableItem.isDamagable) LootItem();
                if (droppedItemPickup != null) PickupItem();
            }
        }
	
		void LootItem()
        {
            lootableItem.LootableItems();
            if (lootSound != null) _audioSource.PlayOneShot(lootSound, lootSoundVolume);
            anim.SetTrigger("PickupItem");
        }
        void PickupItem()
        {
            droppedItemPickup.LootableItems();
        }

    }
}
