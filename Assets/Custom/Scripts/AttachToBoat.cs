using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToBoat : MonoBehaviour
{
	public bool canMount = false;
	
	public BoatAlignNormal boat;
	Animator anim;
	private void Start()
	{
		anim = GetComponent<Animator>();
		
	}
	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag("Mountable"))
		{

			if (Input.GetKeyDown(KeyCode.M))
			{		
				canMount = !canMount;
				anim.SetBool("Mounted", canMount);			  
				boat._playerControlled = canMount;
				
				
			}
			if (canMount)
			{
				
				this.transform.position = other.transform.position;
				this.transform.rotation = other.transform.rotation;
			}
			
		}
		
	}
}
