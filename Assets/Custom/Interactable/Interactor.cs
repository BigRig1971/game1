using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class Interactor : MonoBehaviour
{

	public float rayRange = 10;
	ThirdPersonController _tpc;

	private void Start()
	{
		_tpc = GetComponent<ThirdPersonController>();
	}
	void Update()
	{
		CastRay();
	}

	void CastRay()
	{
		RaycastHit hitInfo = new RaycastHit();
		bool hit = Physics.Raycast(transform.position, transform.forward, out hitInfo, rayRange);
		if (hit && _tpc._input.interaction)
		{
			_tpc._input.interaction = false;
			GameObject hitObject = hitInfo.transform.gameObject;	
			hitObject.GetComponent<IGenericInteractable>().Interact();

		}
	}
}

