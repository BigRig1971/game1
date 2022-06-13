using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToMount : MonoBehaviour
{
    Vector3 player;
    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = this.transform.position;
            other.transform.SetParent(transform);
        }
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
