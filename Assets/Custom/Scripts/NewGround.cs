using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGround : MonoBehaviour
{


	[SerializeField] GameObject Player;
	bool parented = false;
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Player)
		{
			parented = true;
			Player.transform.parent = transform;
		}
	}
	
		
	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject == Player)
		{
			parented = false;
			Player.transform.parent = null;
		}	
	}

	private void FixedUpdate()
	{
		
		if (parented)
		{

			//Player.transform.position = transform.position;
		}
	}
}
