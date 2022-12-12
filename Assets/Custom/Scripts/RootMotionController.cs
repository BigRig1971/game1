using StupidHumanGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class RootMotionController : MonoBehaviour
{
	private float _animationBlend;
	private float _speed;
	public float MoveSpeed = 2.0f;
	public float SprintSpeed = 5.335f;
	public float SpeedChangeRate = 10.0f;
	bool _sprint = false;
	private float _rotationVelocity;
	Quaternion currentRotation;
	public float RotationSmoothTime = 0.12f;
	private GameObject _mainCamera;
	private float _targetRotation = 0.0f;
	public Animator _animator;
	private CharacterController _controller;
	private bool _hasAnimator;
	public GameObject CinemachineCameraTarget;
	public bool LockCameraPosition = false;
	private const float _threshold = 0.01f;
	// cinemachine
	private float _cinemachineTargetYaw;
	private float _cinemachineTargetPitch;
	[Tooltip("How far in degrees can you move the camera up")]
	public float TopClamp = 70.0f;

	[Tooltip("How far in degrees can you move the camera down")]
	public float BottomClamp = -30.0f;

	[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
	public float CameraAngleOverride = 0.0f;

	Vector2 _move;
	Vector2 _look;
	Animator anim;
	private void Awake()
	{
		if (_mainCamera == null)
		{
			_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
	}
	private void Start()
	{
		anim = GetComponent<Animator>();
		_cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
		_hasAnimator = TryGetComponent(out _animator);
		_controller = GetComponent<CharacterController>();
	}
	public void OnSprint(InputValue value)
	{
		_sprint = (value.isPressed);
	}
	private void FixedUpdate()
	{
		MoveOnGround();
	}
	private void LateUpdate()
	{
		LookAround();
	}
	public void OnMove(InputValue value)
	{
		_move = (value.Get<Vector2>());

	}
	public void OnLook(InputValue value)
	{
		_look = (value.Get<Vector2>());
		
	}

	private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
	{
		if (lfAngle < -360f) lfAngle += 360f;
		if (lfAngle > 360f) lfAngle -= 360f;
		return Mathf.Clamp(lfAngle, lfMin, lfMax);
	}
	private void MoveOnGround()
	{
		float targetSpeed = _sprint ? SprintSpeed : MoveSpeed;
		if (_move == Vector2.zero) targetSpeed = 0.0f;
		_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
		if (_animationBlend < 0.01f) _animationBlend = 0f;
		Vector3 inputDirection = new Vector3(_move.x, 0.0f, _move.y).normalized;
		if (_move != Vector2.zero && !InventoryManager.IsOpen())
		{
			_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
							  _mainCamera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
				RotationSmoothTime);
			transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}
		Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
		
		if (_hasAnimator)
		{
			_animator.SetFloat("Speed", _animationBlend);
			_animator.SetFloat("MotionSpeed" , 1);
		}
	}
	private void LookAround()
	{
		if (_look.sqrMagnitude >= _threshold && !LockCameraPosition && !InventoryManager.IsOpen())
		{
			float deltaTimeMultiplier = true ? 1f : Time.deltaTime;
			_cinemachineTargetYaw += _look.x * deltaTimeMultiplier;
			_cinemachineTargetPitch += _look.y * deltaTimeMultiplier;
		}
		_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
		CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
			_cinemachineTargetYaw, 0.0f);
	}
}
