using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : GenericState
{
	public AttackState attackState;
	private bool _collision = false;
	private Vector3 wayPoint, projectedWaypoint;

	public override GenericState RunCurrentState()
	{
		if (GSM.enemyDistance < GSM.attackDistance || GSM.enemyDistance > GSM.followDistance)
		{

			return attackState;
		}
		else
		{
			Follow();
			return this;
		}
	}
	private void Awake()
	{
	}
	private void Start()
	{
	}
	void Follow()
	{
		SwitchToPlayer();
		var qto = Quaternion.LookRotation(projectedWaypoint - transform.position);
		qto = Quaternion.Slerp(transform.rotation, qto, 1 * Time.deltaTime * GSM.turnSpeed);
		GSM.rb.MoveRotation(qto);
		GSM.rb.MovePosition(transform.position + transform.forward * Time.deltaTime * GSM.speed);
	}
	public void TurnLeft()
	{
		wayPoint = transform.position + (transform.right * -10);
		StartCoroutine(DodgingWaypoint());
	}
	public void TurnRight()
	{
		wayPoint = transform.position + (transform.right * 10);
		StartCoroutine(DodgingWaypoint());
	}
	public void SwitchToPlayer()
	{
		if (!_collision)
		{
			projectedWaypoint = GSM.enemyGameObject.transform.position;
		}
	}
	IEnumerator DodgingWaypoint()
	{
		if (!_collision)
		{
			_collision = true;
			projectedWaypoint = wayPoint;
			yield return new WaitForSeconds(GSM.rayDelay);
			//Debug.Log("dodge");
			_collision = false;
		}

	}
	
	private void OnEnable()
	{
		
	}
	private void OnDisable()
	{
		
	}

	
}
