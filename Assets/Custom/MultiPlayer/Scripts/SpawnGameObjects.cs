using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
public class SpawnGameObjects : MonoBehaviour
{
	
	[SerializeField]
	private GameObject player;
	[SerializeField]
	private CinemachineVirtualCamera vcam;
	private void Start()
	{			
		PhotonNetwork.Instantiate(player.name, this.transform.position, Quaternion.identity);
		StartCoroutine(SpawnCamera());
	}
	private IEnumerator SpawnCamera()
	{

		yield return new WaitForSeconds(.2f);
		//PhotonNetwork.Instantiate(vcam.name, this.transform.position, Quaternion.identity);
		if (vcam != null)
		{
			vcam.m_Follow = GameObject.FindWithTag("CinemachineTarget").transform;
			vcam.m_LookAt = GameObject.FindWithTag("CinemachineTarget").transform;

		}
	}
}
