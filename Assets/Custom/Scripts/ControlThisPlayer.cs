using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Characters.ThirdPerson;
public class ControlThisPlayer : MonoBehaviour
{
    PhotonView view;
    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
		{
            (GetComponent("ThirdPersonUserControl") as MonoBehaviour).enabled = true;
            (GetComponent("SwimMode") as MonoBehaviour).enabled = true;
            
        }
    }

}
