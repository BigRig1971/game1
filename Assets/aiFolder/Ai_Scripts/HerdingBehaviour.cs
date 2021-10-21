using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class HerdingBehaviour : MonoBehaviour
{

	[System.Serializable]
	public class MyEvent : UnityEvent<GameObject, int, float, bool>
	{
	}
	[SerializeField]
	private float navLength;
	RaycastHit hit;
	[SerializeField]
	int groupingFactor;
	[SerializeField]
	private float FuckItUpTime = 5f;
	[SerializeField]
	float minSpeed = 2f, maxSpeed = 10f;
	[SerializeField]
	float Speed;
	[SerializeField]
	private MyEvent herdingVariables;
	
	public static bool changeTarget = false;

	private void Start()
	{
		Speed = Random.Range(minSpeed, maxSpeed);
		if (herdingVariables == null)
			herdingVariables = new MyEvent();
		StartHerding();
		FuckItUp1();
	}
	void StartHerding()
	{	
		StartCoroutine(SrartHerding2());
	}
	IEnumerator SrartHerding2()
	{
		Ray checkForLeader = new Ray(transform.position, transform.transform.forward.normalized);
		Debug.DrawRay(checkForLeader.origin, checkForLeader.direction * navLength, Color.green);
		if (Physics.Raycast(checkForLeader, out hit, navLength) && hit.transform.gameObject.name == this.gameObject.name)
		{
			herdingVariables.Invoke(hit.transform.gameObject, groupingFactor, Speed, changeTarget);
		}
		yield return new WaitForSeconds(1f);
		StartHerding();
	}
	void FuckItUp1()
	{
		StartCoroutine(FuckItUp2());
	}
	IEnumerator FuckItUp2()
	{
		changeTarget = !changeTarget;
		Speed = Random.Range(minSpeed, maxSpeed);
		yield return new WaitForSeconds(FuckItUpTime);	
		FuckItUp1();
	}
}
