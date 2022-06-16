using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;
public class LobbyManager : MonoBehaviourPunCallbacks
{
	public TMP_InputField roomInputField;
	public GameObject lobbyPanel;
	public GameObject roomPanel;
	public TMP_Text roomName;

	public RoomItem roomItemPrefab;
	List<RoomItem> roomItemsList = new List<RoomItem>();
	public Transform contentObject;

	public float timeBetweenUpdates = 1.5f;
	float nexteUpdateTime;



	private void Start()
	{
		if(roomPanel != null)
		{
			roomPanel.SetActive(false);
		}
		PhotonNetwork.JoinLobby();
	}

	public void OnClickCreate()
	{
		if (roomInputField.text.Length >= 1)
		{
			PhotonNetwork.CreateRoom(roomInputField.text, new RoomOptions() { MaxPlayers = 20 });
		}
	}
	public override void OnJoinedRoom()
	{
		/*lobbyPanel.SetActive(false);
		roomPanel.SetActive(true);
		roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;*/
		PhotonNetwork.LoadLevel("MultiPlayer");

	}
	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		if(Time.time >= nexteUpdateTime)
		{
			UpdateRoomList(roomList);
			nexteUpdateTime = Time.time + timeBetweenUpdates;
		}
		
	}
	void UpdateRoomList(List<RoomInfo> list)
	{
		foreach (RoomItem item in roomItemsList)
		{
			Destroy(item.gameObject);
		}
		roomItemsList.Clear();

		foreach (RoomInfo room in list)
		{
			RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
			newRoom.SetRoomName(room.Name);
			roomItemsList.Add(newRoom);
		}
	}
	public void JoinRoom(string roomName)
	{
		PhotonNetwork.JoinRoom(roomName);
	}
	public void OnClickLeaveRoom()
	{
		PhotonNetwork.LeaveRoom();
	}
	public override void OnLeftRoom()
	{
		roomPanel.SetActive(false);
		lobbyPanel.SetActive(true);
	}
	public override void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby();
	}
	
}
