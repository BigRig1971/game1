using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    public PhotonView playerPrefab;
    public Transform playerStart;
    void Start()
    {
        //try to connect
        PhotonNetwork.ConnectUsingSettings();
    }

	public override void OnConnectedToMaster()
	{
        Debug.Log("connected");
        PhotonNetwork.JoinRandomOrCreateRoom();
	}
	public override void OnJoinedRoom()
	{
        Debug.Log("Joined A Room");
        PhotonNetwork.Instantiate(playerPrefab.name, playerStart.position, playerStart.rotation);
	}
}
