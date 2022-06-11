using System.Collections;
using UnityEngine;
namespace StupidHumanGames
{
    public class LootStuff : MonoBehaviour
    {
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
                anim.SetTrigger("PickupItem");
                lootableItem = other.gameObject.GetComponent<LootableItem>();
                droppedItemPickup = other.gameObject.GetComponent<ItemPickupable>();
                if (lootableItem != null && !lootableItem.isDamagable) LootItem();
                if (droppedItemPickup != null) PickupItem();
            }
        }
        void LootItem()
        {
            lootableItem.LootableItems();
            if (lootSound != null) AudioSource.PlayClipAtPoint(lootSound, transform.position, lootSoundVolume);
        }
        void PickupItem()
        {
            droppedItemPickup.LootableItems();
        }

    }
}
