using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] UnityEvent item;

	private void OnTriggerEnter(Collider other)
	{
		if (item == null) return;
			
		item.Invoke();
		Destroy(gameObject);
		
	}
}
