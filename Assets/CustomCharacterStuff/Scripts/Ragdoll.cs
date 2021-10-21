using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ragdoll : MonoBehaviour
{
	public static Ragdoll instance;
	[Header("References")]
	[SerializeField] private Animator animator = null;
	private Rigidbody[] ragdollBodies;
	private Collider[] ragdollColliders;
	private Rigidbody rb;
	
	private void Awake()
	{
		instance = this;
	}

	void Start()
	{
		
		ragdollBodies = GetComponentsInChildren<Rigidbody>();
		ragdollColliders = GetComponentsInChildren<Collider>();
		ToggleRagdoll(false);
	
	}


	public void ToggleRagdoll(bool state)
	{
		
		animator.enabled = !state;
		//(camBlock.GetComponent(cam) as MonoBehaviour).enabled = !state;
		foreach (Rigidbody rb in ragdollBodies)
		{
			
			rb.angularDrag = 10;
			rb.drag = 2;
			rb.isKinematic = !state;

		}
		foreach (Collider collider in ragdollColliders)
		{
			collider.enabled = state;
			collider.isTrigger = !state;
		}
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			//(camBlock.GetComponent(cam) as MonoBehaviour).enabled = false;
			ToggleRagdoll(true);
			/////
		}
		if (Input.GetKeyDown(KeyCode.T))
		{
			//(camBlock.GetComponent(cam) as MonoBehaviour).enabled = true;
			ToggleRagdoll(false);
		}
	}
}
