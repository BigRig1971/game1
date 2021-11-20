using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
public class ItemAnimationTrigger : MonoBehaviour
{
    public KeyCode _keyCode = KeyCode.Mouse0;
    public string _triggerName;
    public Animator _animator;

    void Update()
    {
		if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
		{         
          if(_animator)  _animator.SetTrigger(_triggerName);
		}
    }
}
