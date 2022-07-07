using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObject : MonoBehaviour
{
    [SerializeField] bool center;
    [SerializeField] Transform _object;
    [SerializeField] Transform desiredParent;
    private void Awake()
    {
        if(_object == null)
        {
            transform.SetParent(desiredParent);
            if (!center) return;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }
        else
        {
            _object.SetParent(desiredParent);
            if(!center) return;
            _object.localPosition = Vector3.zero;
            _object.localRotation = Quaternion.identity;
        }
       
    }

}
