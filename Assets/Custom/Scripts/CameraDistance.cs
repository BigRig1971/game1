using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using StupidHumanGames;


public class CameraDistance : MonoBehaviour
{
	CinemachineVirtualCamera vcam;
	[SerializeField] float maxDistance = 5f;
	float prevDistance;

	void Start()
	{
		vcam = GetComponent<CinemachineVirtualCamera>();
		prevDistance = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraDistance;
	}

	
	void Update()
	{
		if (!InventoryManager.IsOpen())
		{
			Zoom();
		}
		
		
	}
	void Zoom()
	{
		var thirdperson = vcam.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
		thirdperson.CameraDistance -= Input.mouseScrollDelta.y;
		thirdperson.CameraDistance = (Mathf.Clamp(thirdperson.CameraDistance, prevDistance, maxDistance));
	}
}

