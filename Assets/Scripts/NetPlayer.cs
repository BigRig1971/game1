using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetPlayer : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10f)] float moveSpeed;

    PhotonView photonView;

    void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;
        transform.Translate(Axis * moveSpeed * Time.deltaTime);
    }

    Vector3 direction => new Vector3(1, 0, 1);

    Vector3 Axis => new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
}
