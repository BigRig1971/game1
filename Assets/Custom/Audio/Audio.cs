using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [SerializeField] Light light;
    [SerializeField] LayerMask ground;
    [SerializeField] AudioSource audioSource;
    [SerializeField] bool groundHugging;
    Quaternion targetRot;

   
    void Start()
    {
      if(audioSource != null)  audioSource.Play();
       OnYPosition();   
    }
    void OnYPosition()
    {
        if (!groundHugging) return;
        Vector3 position = transform.position;
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.up.y, transform.position.z),
            -transform.up, out hit, 20, ground))
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;      
            position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .01f;
            transform.position = position;
        }
    }
}
