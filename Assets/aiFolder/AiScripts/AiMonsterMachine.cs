using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class AiMonsterMachine : MonoBehaviour
{
	[HideInInspector]
	public Rigidbody rb;
	public bool canFly = false;
	public int numberOfAttacks = 5;
	private string myHerd;
	public static event System.Action<float> Scale;
	public bool canSwim = false;
	public GenericState currentState;
	public GameObject enemyGameObject;
	public GameObject MainGo;
	public GameObject Go;
	public float scale = 1, followDistance = 10f, attackDistance = 3f, speed = 2.5f, turnSpeed = 3f, maxAltitude = 400f, minAltitude = 280f, navWidth = 1.5f, navLength = 3f, maxRadius = 20f, rayDelay = 1f;
	[HideInInspector]
	public Animator anim;
	[HideInInspector]
	public RaycastHit hit;
	[HideInInspector]
	public Transform navDown, navFwd;
	[HideInInspector]
	public float Pitch, Yaw, Roll, previousSpeed, enemyDistance, homeDistance, previousTurnSpeed, velocity;
	[HideInInspector]
	public Vector3 PreviousFramePosition = Vector3.zero, homePosition;
	[HideInInspector]
	public bool _obstacle = false;
	[SerializeField]
	private UnityEvent turnRight, turnLeft, turnUp, turnDown, goHome;
	
	private void OnEnable()
	{
		HealthBar.PlayerDead += EnemyDeath;
	}
	void Awake()
	{
		gameObject.tag = "Player";
		previousTurnSpeed = turnSpeed;
		homePosition = transform.position;
		scale = scale * Random.Range(.7f, 1.3f);
		speed = speed * scale;
		attackDistance = attackDistance * scale;
		enemyGameObject = GameObject.FindGameObjectWithTag("Player");
		anim = MainGo.GetComponent<Animator>();
		previousSpeed = speed;
		MainGo.transform.localScale = new Vector3(scale, scale, scale);
	}
	private void Start()
	{
		
		
		rb = MainGo.GetComponent<Rigidbody>();
		rb.useGravity = false;
		if (turnRight == null)
			turnRight = new UnityEvent();
		if (turnLeft == null)
			turnLeft = new UnityEvent();
		if (turnDown == null)
			turnDown = new UnityEvent();
		if (turnUp == null)
			turnUp = new UnityEvent();
		if (goHome == null)
			goHome = new UnityEvent();
		
	}
	
	private void FixedUpdate()
	{
		
		//speed = Mathf.Clamp(speed, .5f * previousSpeed, 1f * previousSpeed);
		// speed = speed += Random.Range(-.01f, .01f);
		Vector3 pos = rb.position;
		pos.y = Mathf.Clamp(pos.y, minAltitude, maxAltitude);
		rb.position = pos;
		CheckForObstacles();

		if (velocity > 0.1f)
		{
			anim.SetFloat("Blend", velocity);//,.1f, Time.deltaTime);
		}
		else
		{
			anim.SetFloat("Blend", 0f);
		}
		
	}
	private void Update()
	{
		
		float movementPerFrame = Vector3.Distance(PreviousFramePosition, transform.position);
		velocity = movementPerFrame / Time.deltaTime / previousSpeed;
		PreviousFramePosition = transform.position;
		enemyDistance = Vector3.Distance(navFwd.transform.position, enemyGameObject.transform.position);
		homeDistance = Vector3.Distance(transform.position, homePosition);
		RunGenericStateMachine();
		
		
	}
	private void RunGenericStateMachine()
	{
		GenericState nextState = currentState?.RunCurrentState();
		if (nextState != null)
		{
			SwitchToTheNextState(nextState);
		}
	}
	private void SwitchToTheNextState(GenericState nextState)
	{
		currentState = nextState;
	}
	public void Damage()
	{

	}
	public void CheckForObstacles()
	{
		float rnd = Random.Range(-navWidth, navWidth);
		Quaternion q = transform.rotation;
		Pitch = Mathf.Rad2Deg * Mathf.Atan2(2 * q.x * q.w - 2 * q.y * q.z, 1 - 2 * q.x * q.x - 2 * q.z * q.z);
		Yaw = Mathf.Rad2Deg * Mathf.Atan2(2 * q.y * q.w - 2 * q.x * q.z, 1 - 2 * q.y * q.y - 2 * q.z * q.z);
		Roll = Mathf.Rad2Deg * Mathf.Asin(2 * q.x * q.y + 2 * q.z * q.w);
		if (Pitch > 25) turnUp.Invoke();
		if (Pitch < -25) turnDown.Invoke();
		Ray avoid = new Ray(navFwd.position + (navFwd.transform.right * rnd), navFwd.transform.forward.normalized);
		Debug.DrawRay(avoid.origin, avoid.direction * navLength * scale, Color.red);
		if (Physics.Raycast(avoid, out hit, navLength * scale) && gameObject
			!= MainGo)
		{
			float hDist = valueChange(value: hit.distance, newMin: 0.0f, newMax: 1f);
			turnSpeed = previousTurnSpeed - (hDist / previousTurnSpeed);



			if (rnd < -.2f)
			{
				turnRight.Invoke();
			}
			if (rnd > .2f)

				turnLeft.Invoke();
		}
		if (transform.position.y < minAltitude)
		{

			if (canSwim)
			{
				goHome.Invoke();
			}
			else
			{
				if (Roll > .5f)
				{
					turnRight.Invoke();
				}
				if (Roll < -.5f)
				{
					turnLeft.Invoke();
				}
			}
		}
		if (transform.position.y > maxAltitude)
		{
			if (canSwim)
			{
				goHome.Invoke();
			}
			else
			{
				if (Roll < -.5f)
				{
					turnRight.Invoke();
				}
				if (Roll > .5f)
				{
					turnLeft.Invoke();
				}
			}
		}
		if (homeDistance > maxRadius)
		{
			goHome.Invoke();
		}

	}

	float valueChange(float value, float newMin, float newMax)
	{
		return newMin + value * (newMax - newMin);
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(navDown.transform.position, attackDistance);
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, followDistance);

	}
	private void OnDisable()
	{
		HealthBar.PlayerDead -= EnemyDeath;
	}
	void EnemyDeath()
	{

	}
	private void GetScale(float value)
	{
		Scale?.Invoke(scale);
	}
	
}
