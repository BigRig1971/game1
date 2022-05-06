using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class LauncherMP : MonoBehaviourPunCallbacks
{
	
	public PhotonView playerPrefab;

    [SerializeField]
    private CinemachineVirtualCamera vcam;
	
	
	void Start()
    {
        //try to connect
        PhotonNetwork.ConnectUsingSettings();
		
	}

	public override void OnConnectedToMaster()
	{
        //we connected
        Debug.Log("connected");
        PhotonNetwork.JoinRandomOrCreateRoom();
	}
	public override void OnJoinedRoom()
	{
		Debug.Log("room joined");
        PhotonNetwork.Instantiate(playerPrefab.name, this.transform.position, this.transform.rotation);
		StartCoroutine(SetCamera());
	}
	private IEnumerator SetCamera()
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
