using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingState : GenericState
{
	public ChaseState chaseState;
	private Vector3 wayPoint, projectedWaypoint;
	private Vector3 startingPosition;
	private bool _collision = false;
	private Vector3 forwardDirection, right, up, forward;
	private GameObject GO = null;
	private int groupingFactor;

	public Transform target;
	public float dirNum;
	public float bankFloat = 0f;
	public bool bankChange = false;
	public override GenericState RunCurrentState()
	{

		if (GSM.enemyDistance < GSM.followDistance)
		{
			return chaseState;
		}
		else
		{
			RoamAround();
			GSM.anim.SetFloat("Blend", GSM.animWalkSpeed);
			return this;
		}
	}

	private void Awake()
	{
		startingPosition = transform.position;

		right = transform.right;
		up = transform.up;
		forward = transform.forward;

	}

	private void Start()
	{
		
		GetNewWaypoint();


	}
	private static float InOutSine(float w)
	{
		return -0.5f * (Mathf.Cos(Mathf.PI * w) - 1f);
	}
	private void FixedUpdate()
	{


	}


	void RoamAround()
	{
		Vector3 fakeForward = transform.forward;
		fakeForward.y = 0.0f;
		fakeForward.Normalize();
		Vector3 dir = (this.transform.position - wayPoint).normalized;
		var qto = Quaternion.LookRotation(wayPoint - transform.position);
		qto = Quaternion.Slerp(transform.rotation, qto, 1 * Time.deltaTime * GSM.turnSpeed *.5f);
		GSM.rb.MoveRotation(qto);
		GSM.rb.MovePosition(transform.position + transform.forward * Time.deltaTime * GSM.walkSpeed);

		float dot = Vector3.Dot(transform.right, (wayPoint - transform.position).normalized);

		if (GSM.canFly)
		{
			Vector3 forward = transform.forward;
			float angle = Vector3.SignedAngle(dir, forward, Vector3.up);
			GSM.anim.SetFloat("Direction", dot, .1f, Time.deltaTime);

		}

	}



	float FindBankingAngle(Vector3 birdForward, Vector3 dirToTarget)
	{
		Vector3 cr = Vector3.Cross(birdForward, dirToTarget);
		float ang = Vector3.Dot(cr, Vector3.up);
		return ang;
	}

	public void GetNewWaypoint()
	{

		StartCoroutine(RoamingDelay());
	}
	public void TurnLeft()
	{

		projectedWaypoint = transform.position + (transform.right * -30 + (transform.forward * 10));
		StartCoroutine(DodgingDelay());
	}
	public void TurnRight()
	{

		projectedWaypoint = transform.position + (transform.right * 30) + (transform.forward * 10);
		StartCoroutine(DodgingDelay());
	}
	public void TurnDown()
	{
		projectedWaypoint = transform.position + (transform.up * -10) + (transform.forward * 30);
		StartCoroutine(DodgingDelay());
	}
	public void TurnUp()
	{

		projectedWaypoint = transform.position + (transform.up * 10) + (transform.forward * 30);
		StartCoroutine(DodgingDelay());
	}
	public void GoHome()
	{

		projectedWaypoint = GSM.homePosition;
		StartCoroutine(DodgingDelay());
	}
	public void GoStraight()
	{
		projectedWaypoint = transform.position + (transform.forward * 20);

	}

	IEnumerator Idle()
	{
		yield return new WaitForSeconds(Random.Range(10f, 20f));
		GSM.walkSpeed = 0f;
		GSM.anim.SetFloat("Blend", 0f);
		//GSM.anim.SetBool("Idle", true);
		yield return new WaitForSeconds(Random.Range(1f, 3f));
		//GSM.anim.SetBool("Idle", false);
		GSM.rb.angularDrag = 1;
		GSM.walkSpeed = GSM.previousSpeed;
	}
	IEnumerator RoamingDelay()
	{
		if (!_collision)
		{
			GoStraight();
			Vector3 fakeForward = transform.forward;
			fakeForward.y = 0.0f;
			fakeForward.Normalize();
			float rndRight = Random.Range(-10f, 10f);
			float rndUp = Random.Range(-10f, 10f);

			if (GSM.canSwim)
			{
				wayPoint = transform.position + (transform.right * rndRight) + (transform.up * rndUp) + (transform.forward * 20f);
			}
			else
			{
				wayPoint = transform.position + (transform.right * rndRight) + (fakeForward * 20f);
			}
			yield return new WaitForSeconds(Random.Range(3f, 5f));
			AttackWait();
			GetNewWaypoint();
		}
	}
	IEnumerator DodgingDelay()
	{
		if (!_collision)
		{
			_collision = true;
			wayPoint = projectedWaypoint;
			yield return new WaitForSeconds(GSM.rayDelay);
			_collision = false;
			GetNewWaypoint();
		}
	}
	IEnumerator HerdingWaypoint()
	{
		if (!_collision)
		{
			while (true)
			{
				float dist = Vector3.Distance(GO.transform.position, transform.position);
				Vector3 dir = (this.transform.position - GO.transform.position).normalized;
				//if (dist > groupingFactor)
				{
					wayPoint = GO.transform.position + (dir);
				}
				yield return null;

			}

		}
	}
	IEnumerator AttackWait()
	{

		yield return new WaitForSeconds(Random.Range(1, 3));

	}

	public void FollowTheLeader(GameObject v, int cz, float speed, bool ct)
	{
		GSM.walkSpeed = speed;
		GSM.turnSpeed = speed / 2;
		groupingFactor = cz;
		GO = v;
		StartCoroutine(HerdingWaypoint());
		if (ct)
		{
			GetNewWaypoint();
		}
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawSphere(wayPoint, .5f);
	}
}
