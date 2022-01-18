using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZInventory;
public class AnimTriggerAndDamageDealer : MonoBehaviour
{
	public KeyCode _keyCode = KeyCode.Mouse0;
	public int damagePower = 5;
	public string _triggerName;
	public string _floatName;
	public string _boolName;
	public float delayImpact = .3f;
	public Animator _animator;
	LootableItem damagableGo;
	private bool canCauseDamage = false;
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
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Lootable") && canCauseDamage)
		{

			damagableGo = other.gameObject.GetComponent<LootableItem>();
			StartCoroutine(CauseSomeDamage());
		}
	}

	void Update()
	{
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName(_triggerName))
		{
			if(!_animator.GetBool("Attack"))
			_animator.SetBool("Attack", true);
		}
		else
		{
			if (_animator.GetBool("Attack"))
				_animator.SetBool("Attack", false);
		}
			if (Input.GetKeyDown(_keyCode) && !InventoryManager.IsOpen())
		{		
			if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_triggerName))
			{
				canCauseDamage = true;				
				_animator.SetTrigger(_triggerName);
				//_animator.SetBool("Attack", true);
			}		
		}
		/*else
		{
			if (!_animator.GetCurrentAnimatorStateInfo(0).IsName(_triggerName))
			{
				_animator.SetBool("Attack", false);
			}
		}*/
	}
	private IEnumerator CauseSomeDamage()
	{
		canCauseDamage = false;
		damagableGo.TakeDamage(damagePower);
		_animator.SetTrigger("Interrupt");
		_animator.speed = 0f;
		yield return new WaitForSeconds(.2f);
		_animator.speed = 1;
		if(damagableGo.health <= 0)
		{
			damagableGo.LootableItems();
		}
	}
}
