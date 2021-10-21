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
		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private Camera _mainCamera;
		private ThirdPersonController _tpc;
		private Rigidbody _rb;
		//   public RuntimeAnimatorController OnLand;
		//   public RuntimeAnimatorController InWater;
		private bool headAboveWater = true;
		private bool buttAboveWater = true;
		public bool _verticalMovement = false;

		void Start()
		{
			_tpc = GetComponent<ThirdPersonController>();
			_animator = GetComponent<Animator>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
			_animIDRoll = Animator.StringToHash("Roll");
			_swim = Animator.StringToHash("Swim");
			_rb = GetComponent<Rigidbody>();
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
			if (_input.roll)
			{
				StartCoroutine(Roll());
			}

		}
	
		public void HeadAboveWater()
		{
			if (_animator.GetBool("Grounded"))
			{
				Walking();
			}
			if (!_animator.GetBool("Grounded"))
			{
				TreadingWater();
			}

		}
		public void HeadBelowWater()
		{
			Swimming();
		}

		void TreadingWater()
		{
			_verticalMovement = false;
			_animator.SetBool("Swim", true);
		
			_tpc._verticalVelocity = 0f;
			_tpc.Gravity = -10;
			
		}
		void Swimming()
		{
			_verticalMovement = true;
			_animator.SetBool("Swim", true);
			_tpc.Gravity = 1;
			_tpc._verticalVelocity = 0f;
			
		}
		void Walking()
		{
			
			_verticalMovement = false;
			_animator.SetBool("Swim", false);
			_tpc.Gravity = -15;
		
			// _animator.runtimeAnimatorController = OnLand;



		}
		private IEnumerator Roll()
		{
			_animator.applyRootMotion = true;
			_animator.SetTrigger(_animIDRoll);
			_input.roll = false;
		yield return new WaitForSeconds(.7f);
			_animator.applyRootMotion = false;
		}

	}
}