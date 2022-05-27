using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.UI;



namespace StupidHumanGames
{
	public class CustomMovement : MonoBehaviour
	{

		//custom movement
		private int _animIDRoll;
		private int _swim;
		private int _rightHand;
		
		private CharacterController _controller;
		private Camera _mainCamera;
		private TPC _tpc;
		private bool headAboveWater = true;
		public bool buttAboveWater = true;
		public bool _verticalMovement = false;
		//audio stuff
		public AudioSource footStep;
		public AudioSource stepInWater;
		public AudioSource roll;
		public AudioSource jumpLand;
		public AudioSource jumpStart;
		private bool inWater = false;
		private float _previousSpeed;
		private bool fp = false;
		void Start()
		{
			_tpc = GetComponent<TPC>();
			_controller = GetComponent<CharacterController>();
			_animIDRoll = Animator.StringToHash("Roll");
			_swim = Animator.StringToHash("Swim");
			_rightHand = Animator.StringToHash("RightHand");
			_mainCamera = Camera.main;
			_previousSpeed = _tpc.MoveSpeed;
			
			
		}
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.V))
			{
				fp = !fp;
				if (fp)
				{
					_tpc._animator.SetBool("Dance", true);
				}
				if (!fp)
				{
					_tpc._animator.SetBool("Dance", false);
				}
			}
			if (Input.GetKeyDown(KeyCode.F))
			{
				fp = !fp;
				if (fp)
				{
					_tpc._animator.SetBool("FingerPussy", true);
				}
				if (!fp)
				{
					_tpc._animator.SetBool("FingerPussy", false);
				}
			}
			Movement();		
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
			if (_tpc._input.RightHand)
			{			
				_tpc._animator.SetBool(_rightHand, true);

				_tpc._input.RightHand = false;			
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
			OnDisableRootMotion();
			_tpc.GroundedOffset = -1;
			_verticalMovement = false;
			_tpc._animator.SetBool(_swim, true);
			_tpc._verticalVelocity = 0f;
			_tpc.Gravity = -10;
		}
		void Swimming()
		{
			OnDisableRootMotion();
			_tpc.GroundedOffset = -1;
			_verticalMovement = true;
			_tpc._animator.SetBool(_swim, true);
			_tpc.Gravity = 1;
			_tpc._verticalVelocity = 0f;
		}
		void Walking()
		{
			OnApplyRootMotion();
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
		 void OnApplyRootMotion()
		{
			if(!_tpc._animator.applyRootMotion)
			_tpc._animator.applyRootMotion = true;
		}
		 void OnDisableRootMotion()
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
		
	}
}