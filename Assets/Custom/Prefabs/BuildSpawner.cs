using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class BuildSpawner : MonoBehaviour
{
    public GameObject buildModel;

    private void OnEnable()
    {
        PhotonNetwork.Instantiate(buildModel.name, this.transform.position, this.transform.rotation);
    }
    
}
