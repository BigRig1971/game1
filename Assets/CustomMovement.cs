using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;



namespace StarterAssets
{
	public class CustomMovement : MonoBehaviour
	{
		//custom inputs

		//custom movement
		private int _animIDRoll;
		private int _swim;
		private int _punch;
		private CharacterController _controller;
		private Camera _mainCamera;
		private ThirdPersonController _tpc;
	
		//   public RuntimeAnimatorController OnLand;
		//   public RuntimeAnimatorController InWater;
		private bool headAboveWater = true;
		private bool buttAboveWater = true;
		private bool headBelowWater = false;
		private bool buttBelowWater = false;
		public bool _verticalMovement = false;
		//audio stuff
		public AudioSource footStep;
		public AudioSource stepInWater;
		public AudioSource roll;
		public AudioSource jumpLand;
		public AudioSource jumpStart;
		private bool inWater = false;
		private float _previousSpeed;
		void Start()
		{
			_tpc = GetComponent<ThirdPersonController>();
			_controller = GetComponent<CharacterController>();
			_animIDRoll = Animator.StringToHash("Roll");
			_swim = Animator.StringToHash("Swim");
			_punch = Animator.StringToHash("Punch");

			//  _animator.runtimeAnimatorController = OnLand;
			_mainCamera = Camera.main;
			_previousSpeed = _tpc.MoveSpeed;

		}

		// Update is called once per frame
		void Update()
		{
			Movement();
			
		}
		private void FixedUpdate()
		{

		}
		private IEnumerator Delay()
		{
			yield return new WaitForSeconds(.3f);
		}
		private void Movement()
		{
			if ( _tpc._input.cursorInputForLook && _tpc._input.roll)
			{
				_tpc._animator.SetBool(_animIDRoll, true);
				
				_tpc._input.roll = false;
			}
			if (_tpc._input.punch)
			{
				
				_tpc.MoveSpeed = 0;
				_tpc._animator.SetBool(_punch, true);

				_tpc._input.punch = false;
			
			}

		}
		public void ButtAboveWater()
		{
			buttAboveWater = true;

			Walking();
		}
		public void ButtBelowWater()
		{
			buttAboveWater = false;
			if (headAboveWater)
			{
				TreadingWater();
			}
		}
		public void HeadAboveWater()
		{
			headAboveWater = true;
			
			if (!buttAboveWater)
			{
				TreadingWater();
			}
		}
		public void HeadBelowWater()
		{
			headAboveWater = false;
			Swimming();
		}
		void TreadingWater()
		{
			_tpc.GroundedOffset = -1;
			_verticalMovement = false;
			_tpc._animator.SetBool(_swim, true);
			_tpc._verticalVelocity = 0f;
			_tpc.Gravity = -10;
		}
		void Swimming()
		{
			_tpc.GroundedOffset = -1;
			_verticalMovement = true;
			_tpc._animator.SetBool(_swim, true);
			_tpc.Gravity = 1;
			_tpc._verticalVelocity = 0f;
		}
		void Walking()
		{
			_tpc.GroundedOffset = 0;
			_verticalMovement = false;
			_tpc._animator.SetBool(_swim, false);
			_tpc.Gravity = -15f;
		}
	
		public void FootStepOnGround()
		{
			inWater = false;
		}
		public void FootStepInWater()
		{
			inWater = true;
		}
		public void FootStep()
		{
			if (inWater)
			{
				stepInWater?.Play();
			}
			else
			{
				footStep?.Play();
			}
		}
		public void OnApplyRootMotion()
		{
			if(!_tpc._animator.applyRootMotion)
			_tpc._animator.applyRootMotion = true;
		}
		public void OnDisableRootMotion()
		{
			if(_tpc._animator.applyRootMotion)
			_tpc._animator.applyRootMotion = false;
		}
		public void OnRollSound()
		{
			roll?.Play();
		}
		public void OnJumpLandSound()
		{
			jumpLand?.Play();
		}
		public void OnJumpStartSound()
		{
			jumpStart?.Play();
		}
		private void ChangeSkinnedMesh(GameObject bodyPart, Mesh pMesh)
		{
			Mesh meshInstance = Instantiate(pMesh) as Mesh;

			bodyPart.GetComponent<SkinnedMeshRenderer>().sharedMesh = meshInstance;
		}
	}

}