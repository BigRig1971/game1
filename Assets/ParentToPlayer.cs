using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentToPlayer : MonoBehaviour
{
    Transform player;
    [SerializeField] float yOffset = 1f;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        StartCoroutine(ParentPlayer());
    }
    IEnumerator ParentPlayer()
    {
        yield return new WaitForSeconds(.5f);
        transform.SetParent(player);
        transform.localPosition = Vector3.zero + transform.up * yOffset;
        transform.localRotation = Quaternion.identity;
        yield return null;
    }
}
