using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentObject : MonoBehaviour
{
    [SerializeField] bool center = true;
    [SerializeField] Transform _object;
    [SerializeField] Transform desiredParent;
    Transform currentTransform;
    private void Awake()
    {
      currentTransform = transform;
        if(_object == null)
        {
            transform.SetParent(desiredParent);
            transform.localPosition = currentTransform.localPosition;
            if (center)
            {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
            }
        }
        else
        {
            _object.SetParent(desiredParent);
            transform.position = currentTransform.position;
            if (center)
            {
                _object.localPosition = Vector3.zero;
                _object.localRotation = Quaternion.identity;
            }
           
        }
       
    }

}
