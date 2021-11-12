using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using UnityEngine.InputSystem;


public class CameraDistance : MonoBehaviour
{
	CinemachineCameraOffset camOffset;
	

	// Start is called before the first frame update
	void Start()
	{
		
		camOffset = GetComponent<CinemachineCameraOffset>();

	}

	// Update is called once per frame
	void Update()
	{
		
		camOffset.m_Offset.z += Input.mouseScrollDelta.y;
		camOffset.m_Offset = new Vector3(0.0f, 0.0f, Mathf.Clamp(camOffset.m_Offset.z, -5f, 0f));
	}
}

