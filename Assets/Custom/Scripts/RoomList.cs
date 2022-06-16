using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomList : MonoBehaviourPunCallbacks
{

	public InputField createInput;
	public InputField joinInput;
	[SerializeField]
	private GameObject roomListing;
	[SerializeField]
	private Transform content;

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(createInput.text);
	}
	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(joinInput.text);
	}
	public override void OnJoinedRoom()
	{
		PhotonNetwork.LoadLevel("Main");
	}
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		base.OnRoomListUpdate(roomList);
	}
}
