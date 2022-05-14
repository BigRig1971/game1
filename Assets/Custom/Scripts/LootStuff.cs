using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
public class LootStuff : MonoBehaviour
{
	Animator anim;
	LootableItem itemPickup;
	ItemPickupable droppedItemPickup;
	[SerializeField] LayerMask lootableLayer;
	[SerializeField] float colliderSize = .5f;
	[SerializeField] Vector3 colliderPosition = Vector3.zero;

	public string scriptToPause;
	private void Start()
	{
		anim = GetComponent<Animator>();

	}
    private void Update()
    {
		//OnCollisionDetection();

	}
    	private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Lootable"))
            {
                Debug.Log("lootstuff");
                itemPickup = other.gameObject.GetComponent<LootableItem>();
                droppedItemPickup = other.gameObject.GetComponent<ItemPickupable>();
                if(itemPickup.readyToLoot) StartCoroutine(LootItem());
            }

        }
  /*  void OnCollisionDetection()
    {
		Collider[] hitColliders = Physics.OverlapSphere(colliderPosition, colliderSize, lootableLayer, QueryTriggerInteraction.Collide);
		foreach (Collider col in hitColliders)
        {
			//col.ClosestPoint(transform.position);
			//if (!col.isTrigger) continue;
			itemPickup = col.gameObject.GetComponent<LootableItem>();
			droppedItemPickup = col.gameObject.GetComponent<ItemPickupable>();
			if (itemPickup.readyToLoot) StartCoroutine(LootItem());
			col.isTrigger = false;
		}

	}*/
	private IEnumerator LootItem()
	{
		(gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = false;
		if (itemPickup != null) itemPickup.LootableItems();
		if (droppedItemPickup != null) droppedItemPickup.LootableItems();
		anim.SetBool("Pickup", true);

		yield return new WaitForSeconds(.3f);

		(gameObject.GetComponent(scriptToPause) as MonoBehaviour).enabled = true;

		anim.SetBool("Pickup", false);
		
	}

    private void OnDrawGizmos()
    {
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(Vector3.zero + colliderPosition, colliderSize);
	}

}
