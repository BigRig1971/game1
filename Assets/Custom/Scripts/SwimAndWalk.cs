using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StupidHumanGames;

namespace StupidHumanGames
{
    public class SwimAndWalk : MonoBehaviour
    {
        float distanceToSurface = 0f;
        bool buttAboveWater = false;
        bool headAboveWater = false;
        bool feetInWater = false;
        bool swimming = false;
        float _animationBlend;
        Animator animator;
        public AudioSource footStep;
        public AudioSource stepInWater;
        public AudioSource roll;
        public AudioSource jumpLand;
        public AudioSource jumpStart;
        public float boyancy = 1f;
        Rigidbody rb;
        bool state = false;
        private float _rotationVelocity;
        public float _verticalVelocity;
        private GameObject _mainCamera;
        private float _targetRotation = 0.0f;
        Animator _animator;
        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;
        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;
        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;
        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        private const float _threshold = 0.01f;
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;
        public float RotationSmoothTime = 0.12f;
        public bool LockCameraPosition = false;
        Quaternion targetRot;
        [SerializeField] bool groundHugging = false;
        [SerializeField] LayerMask groundLayer;
        public float runSpeed = 5f;
        public float walkSpeed = 2.5f;
        public StupidHumanInputs _input;
        private void Awake()
        {
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
            _animator = GetComponent<Animator>();
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
        private void FixedUpdate()
        {
            if (groundHugging) OnYPosition();

            OnRootMotion();
            OnCamRot();
        }
        void Update()
        {
            if (_input.cursor)
            {
                _input.cursor = false;
                state = !state;
                if (state)
                {
                    OnCursorVisable();
                }
                else
                {
                    OnCursorHide();
                }
            }
        }
        void OnYPosition()
        {
            Vector3 position = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.up.y * 2, transform.position.z),
                -transform.up, out hit, 20, groundLayer))
            {
                if (groundHugging) targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / 0.15f);
                position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .01f;
                transform.position = position;
            }
        }
        private void OnCamRot()
        {
            if (state) return;
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
                _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
            }
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        void OnCursorVisable()
        {
            InventoryManager.OpenInventory();
        }
        void OnCursorHide()
        {
            InventoryManager.CloseInventory();
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
        public void FootStepOnGround()
        {
            feetInWater = false;
        }
        public void FootStepInWater()
        {
            feetInWater = true;
        }
        public void FootStep()
        {
            if (feetInWater)
            {
                if (stepInWater != null) stepInWater?.Play();
            }
            else
            {
                if (footStep != null) footStep?.Play();
            }
        }
        void OnApplyRootMotion()
        {
            if (!_animator.applyRootMotion)
                _animator.applyRootMotion = true;
        }
        void OnDisableRootMotion()
        {
            if (_animator.applyRootMotion)
                _animator.applyRootMotion = false;
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
      
        void TreadingWater()
        {
            boyancy = -.5f;
            swimming = true;
            StartCoroutine(OnSwimming());
        }
        void Swimming()
        {
            boyancy = .5f;
            swimming = true;
            StartCoroutine(OnSwimming());
        }
        void Walking()
        {
            swimming = false;
        }
        void OnRootMotion()
        {
            if (state || swimming) return;
            OnApplyRootMotion();
            float targetSpeed = _input.sprint ? runSpeed : walkSpeed;
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            _animator.SetFloat("Speed", targetSpeed);
            if (_input.move == Vector2.zero) return;
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rotation, 0.0f);
        }
        IEnumerator OnSwimming()
        {
            OnDisableRootMotion();
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            _animator.SetBool("Swim", true);
            while (swimming)
            {               
                float targetSpeed = _input.sprint ? runSpeed * .5f : walkSpeed * .5f;
                Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
                if (_input.move == Vector2.zero)
                {
                    rb.AddForce(0f, boyancy * -Physics.gravity.y, 0f);
                    targetSpeed = 0f;
                    _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * 5f);
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                }
                else
                {
                    _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * 5f);
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);
                    var qto = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
                    var rot = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * 5f);
                    transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rotationY, 0.0f);
                    transform.position += transform.forward * Time.deltaTime * targetSpeed;
                }
                _animator.SetFloat("SwimSpeed", _animationBlend);
                yield return null;
            }
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            _animator.SetBool("Swim", false);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}

