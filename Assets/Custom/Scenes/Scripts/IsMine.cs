using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class IsMine : MonoBehaviour
{
	PhotonView view;
	CharacterController controller;
	[SerializeField]
	private GameObject[] gameObjects;
	private void Awake()
	{
		view = GetComponent<PhotonView>();
		if (!view.IsMine)
		{
			foreach (GameObject _obj in gameObjects)
			{
				_obj.SetActive(false);
			}
			controller = GetComponent<CharacterController>();
			controller.enabled = false;
		}
		
	}
	
}
