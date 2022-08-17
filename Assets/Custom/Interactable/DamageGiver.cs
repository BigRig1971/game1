using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StupidHumanGames;


public class DamageGiver : MonoBehaviour
{
	
	public float damage = 1;
	public float rayRange = 10;
	public string enemyTag;
	ThirdPersonController _tpc;
	private void Start()
	{
		
		_tpc = GetComponent<ThirdPersonController>();
	}
	void Update()
	{
		Interact();
	}

	void Interact()
	{
	/*	if (_tpc._input.interaction)
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(transform.position + transform.up * 1, transform.forward, out hitInfo, rayRange);
			_tpc._input.interaction = false;
			if (hit)

			{
				
				GameObject hitObject = hitInfo.transform.gameObject;

				//hitObject.GetComponent<IGenericInteractable>().Interact();

				IGenericInteractable ig = hitObject.GetComponent<IGenericInteractable>();
				if (ig == null) return;
				ig.Interact();

				IDamagable id = hitObject.GetComponent<IDamagable>();
				if (id == null) return;				
				id.Damage(amount: damage);
				Debug.Log(id.Health);
			}
		}*/
		
	}
}

