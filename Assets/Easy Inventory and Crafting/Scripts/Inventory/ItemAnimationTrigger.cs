using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
public class ItemAnimationTrigger : MonoBehaviour
{
	public KeyCode _keyCode = KeyCode.Mouse0;
	public string _triggerName;
	public string _floatName;
	public string _boolName;
	public Animator _animator;
	private void OnEnable()
	{
		if (_boolName == "") return;

		if (_animator) _animator.SetBool(_boolName, true);


	}
	private void OnDisable()
	{
		if (_boolName == "") return;

		if (_animator) _animator.SetBool(_boolName, false);


	}
	private void Start()
	{

	}
	void Update()
	{
		if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
		{
			if (_animator) _animator.SetTrigger(_triggerName);
		}
	}
}
