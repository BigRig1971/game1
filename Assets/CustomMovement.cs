using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using Cinemachine;


namespace StarterAssets
{
	public class CustomMovement : MonoBehaviour
	{
		//custom inputs

		//custom movement
		private int _animIDRoll;
		private int _swim;
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
		private bool inWater = false;

		void Start()
		{
			_tpc = GetComponent<ThirdPersonController>();
			_controller = GetComponent<CharacterController>();
			_animIDRoll = Animator.StringToHash("Roll");
			_swim = Animator.StringToHash("Swim");
			
			//  _animator.runtimeAnimatorController = OnLand;
			_mainCamera = Camera.main;


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
			if (_tpc._input.roll)
			{
				_tpc._animator.SetBool(_animIDRoll, true);
				
				_tpc._input.roll = false;
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
			_tpc._animator.applyRootMotion = true;
		}
		public void OnDisableRootMotion()
		{
			_tpc._animator.applyRootMotion = false;
		}
		

	}
}