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
               // Instantiate(lootableItem.healthBar);
                droppedItemPickup = other.gameObject.GetComponent<ItemPickupable>();
                if (lootableItem != null && !lootableItem.isDamagable) LootItem();
                if (droppedItemPickup != null) PickupItem();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Lootable"))
            {
                if(lootableItem.healthBar != null)
                {
                    Destroy(lootableItem.healthBar);
                }
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
