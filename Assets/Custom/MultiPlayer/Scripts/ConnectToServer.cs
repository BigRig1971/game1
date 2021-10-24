using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public TMP_InputField nickName;
    public TMP_Text buttonText;

    public void OnClickConnect()
	{
        if(nickName.text.Length >= 1)
		{
			PhotonNetwork.NickName = nickName.text;
			buttonText.text = "Connecting...";
			PhotonNetwork.ConnectUsingSettings();
		}
	}
	public override void OnConnectedToMaster()
	{
		SceneManager.LoadScene("Lobby");
	}
}
