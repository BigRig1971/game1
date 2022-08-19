using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundationControl : MonoBehaviour
{
    [SerializeField] Collider[] collidersToKeep;
    [SerializeField] Collider[] allColliders;
    [SerializeField] LayerMask ground;

    private void Awake()
    {
        allColliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in allColliders)
        {
            collider.enabled = false;
        }
    }
    private void Start()
    {
        if (OnCheckGround())
        {
            foreach (Collider collider in allColliders)
            {
                collider.enabled = true;
            }
        }
        else
        {
            foreach (Collider collider in collidersToKeep)
            {
                collider.enabled = true;
            }
        }
        Destroy(this);
    }
    bool OnCheckGround()
    {
        if (Physics.CheckSphere(transform.localPosition, 1.5f, ground)) return true; else return false;
    }
}
