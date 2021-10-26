using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCube : MonoBehaviour, IGenericInteractable
{
   public void Interact()
	{
		Destroy(gameObject);
	}
}
