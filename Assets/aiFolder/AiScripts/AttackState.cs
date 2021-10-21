using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : GenericState
{
	public static event System.Action<int, AudioClip> EnemyHit;
	[SerializeField] private AudioSource Woosh;
	[SerializeField] private AudioClip enemyHitSound;
	[SerializeField] private float wooshDelay = .2f;
	[SerializeField] private float hitDelay = .1f;
	private bool canAttack = true;
	public RoamingState roamingState;
	public bool isInAttackRange;
	[SerializeField]
	private int attackPower = 30;
	

	private void Awake()
	{
		
	}
	public override GenericState RunCurrentState()
	{
		if (GSM.enemyDistance > GSM.attackDistance)
		{
			
			return roamingState;		
		}
		else
		{
			
			Attacking();
			return this;
		}	
	}
	private void Start()
	{
		//GSM.anim.SetFloat("Blend", 0);
	}
	void Attacking()
	{
		
		if (canAttack)
		{
			
			StartCoroutine(AttackEnemy());
		}
	}
	IEnumerator AttackEnemy()
	{

		canAttack = false;
		int randomNumber = Random.Range(1, GSM.numberOfAttacks);
		GSM.anim.SetTrigger("bite");
		GSM.anim.SetTrigger("Attack" + randomNumber);
		
		yield return new WaitForSeconds(wooshDelay);
		
		if (Woosh != null)
		{
			Woosh.pitch = Random.Range(.7f, 1f);
			if(Woosh != null) Woosh.Play();
		}
		yield return new WaitForSeconds(hitDelay);
		
		Attack();
		yield return new WaitForSeconds(Random.Range(.5f, 3));
		canAttack = true;
		GSM.speed = GSM.previousSpeed;
	}
	private void Attack()
	{	
		
		float distance = Vector3.Distance(transform.position,GSM.enemyGameObject.transform.position);
		float dot = Vector3.Dot(transform.forward, (GSM.enemyGameObject.transform.position - transform.position).normalized);
		if (dot > 0.7f)
		{
			GSM.speed = -1f;
			EnemyHit?.Invoke(attackPower, enemyHitSound);	
		}
	}
}
