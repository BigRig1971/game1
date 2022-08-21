using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBuildParent : MonoBehaviour
{
  
    [SerializeField] LayerMask parent;
    Collider[] hitColliders;

    // Start is called before the first frame update
    void Start()
    {
       OnSetParent();
    }

    void OnSetParent()
    {

        hitColliders = Physics.OverlapSphere(transform.position, .3f, parent);
        foreach (Collider col in hitColliders)
        {
            if (!col.CompareTag("SnapPointSubFoundation"))
            {
                transform.SetParent(col.transform);
            }
            
        }
        Destroy(this);
    }
}
