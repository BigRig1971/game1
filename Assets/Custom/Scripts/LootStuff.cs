using System.Collections;
using UnityEngine;
namespace StupidHumanGames
{
    public class LootStuff : MonoBehaviour
    {
        Animator anim;
        LootableItem lootableItem;
        ItemPickupable droppedItemPickup;

        public string scriptToPause;
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
                if (lootableItem != null) StartCoroutine(LootItem());
                if (droppedItemPickup != null) StartCoroutine(PickupItem());
            }
        }
        private IEnumerator LootItem()
        {


            (gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = false;
            lootableItem.LootableItems();

            anim.SetBool("Pickup", true);
            yield return new WaitForSeconds(.3f);
            (gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = true;
            anim.SetBool("Pickup", false);

        }
        private IEnumerator PickupItem()
        {


            (gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = false;

            droppedItemPickup.LootableItems();
            anim.SetBool("Pickup", true);
            yield return new WaitForSeconds(.3f);
            (gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = true;
            anim.SetBool("Pickup", false);

        }
    }
}
