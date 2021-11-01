using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Storage : MonoBehaviour
{
    [SerializeField] UnityEvent storage;
	private bool isActive = false;
	void Start()
	{
		if (storage == null)
			storage = new UnityEvent();

	}
	private void OnTriggerEnter(Collider other)
	{
	

		if (other == null) return;
		storage?.Invoke();
		
	}
	private void OnTriggerExit(Collider other)
	{
		if (other == null) return;
		storage?.Invoke();
	}
	
}
