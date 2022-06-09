using UnityEngine;
using System.Collections;
using StupidHumanGames;


#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StupidHumanGames
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {

        bool cursorState = false;
        InventoryManager inventory;
        public enum State { MoveOnLand, Swimming, TreadingWater };
        [SerializeField] State _currentState;
        [SerializeField] Transform head;
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        public AudioClip rollAudioClip;
        [Range(0, 1)] public float rollVolume = 0.5f;
        public AudioClip JumpingAudioClip;
        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -50.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        public bool GroundedUnderWater = false;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.0f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;
        // state stuff
        private bool headAboveWater = true;
        private bool buttAboveWater = true;
        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        public float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animIDSwim;
        private int _animIDSwimSpeed;
        private int _animIDRoll;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        public StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }
        private void Awake()
        {

            _currentState = State.MoveOnLand;
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            StartCoroutine(CurrentState());
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif
            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }
        private void Update()
        {
            OnGravity();
            _hasAnimator = TryGetComponent(out _animator);

            if (_input._inventory)
            {
                _input._inventory = false;
                cursorState = !cursorState;
                if (cursorState)
                {
                    OnCursorVisable();
                }
                else
                {
                    OnCursorHide();
                }
            }
        }
        void OnCursorVisable()
        {
            InventoryManager.OpenInventory();
        }
        void OnCursorHide()
        {
            InventoryManager.CloseInventory();
        }
        private void FixedUpdate()
        {

        }

        private void LateUpdate()
        {

            if (!InventoryManager.IsOpen()) CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animIDSwimSpeed = Animator.StringToHash("SwimSpeed");
            _animIDSwim = Animator.StringToHash("Swim");
            _animIDRoll = Animator.StringToHash("Roll");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);

            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        private void SwimGroundCheck()
        {
            Vector3 spherePosition = new Vector3(head.position.x, head.position.y,
               head.position.z);
            GroundedUnderWater = (Physics.CheckSphere(spherePosition, .3f, GroundLayers,
                 QueryTriggerInteraction.Ignore));
            if (GroundedUnderWater)
            {
                if (!_animator.applyRootMotion) _animator.applyRootMotion = true;
            }
            else
            {
                if (_animator.applyRootMotion) _animator.applyRootMotion = false;
            }
        }
        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input._look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1f : Time.deltaTime;
                _cinemachineTargetYaw += _input._look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input._look.y * deltaTimeMultiplier;
            }
            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        IEnumerator MoveOnLand()
        {
            Gravity = -50f;
            if (_animator && !_animator.applyRootMotion) _animator.applyRootMotion = true;
            _animator.SetBool(_animIDSwim, false);
            while (OnIsOnLand())
            {
                GroundedCheck();
                JumpAndGravity();
                //Roll
                if (_input._roll)
                {
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDRoll, true);
                        _input._roll = false;
                    }
                }

                // set target speed based on move speed, sprint speed and if sprint is pressed
                float targetSpeed = _input._sprint ? SprintSpeed : MoveSpeed;

                // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

                // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is no input, set the target speed to 0
                if (_input._move == Vector2.zero) targetSpeed = 0.0f;

                // a reference to the players current horizontal velocity
                float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

                float speedOffset = 0.1f;
                float inputMagnitude = _input.analogMovement ? _input._move.magnitude : 1f;

                // accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    // creates curved result rather than a linear one giving a more organic speed change
                    // note T in Lerp is clamped, so we don't need to clamp our speed
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);

                    // round speed to 3 decimal places
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }

                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;

                // normalise input direction
                Vector3 inputDirection = new Vector3(_input._move.x, 0.0f, _input._move.y).normalized;

                // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
                // if there is a move input rotate player when the player is moving
                if (_input._move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);

                    // rotate to face input direction relative to camera position
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }


                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

                // move the player
                if (!_animator.applyRootMotion)
                {
                    _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
                }


                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetFloat(_animIDSpeed, _animationBlend);
                    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);

                }
                yield return null;

            }
            _currentState = State.TreadingWater;
        }
        IEnumerator TreadingWater()
        {

            if (_animator.applyRootMotion) _animator.applyRootMotion = false;
            _animator.SetBool(_animIDSwim, true);
            while (OnIsTreadingWater())
            {

                
                _controller.Move(new Vector3(0.0f, -.5f, 0.0f) * Time.deltaTime);
                if (_hasAnimator)
                {
                    _animator.SetFloat(_animIDSwimSpeed, _animationBlend);

                }
                yield return null;
            }
            _currentState = State.Swimming;
        }
        IEnumerator Swimming()
        {

            if (_animator.applyRootMotion) _animator.applyRootMotion = false;
            _animator.SetBool(_animIDSwim, true);
            while (OnIsSwimming())
            {
                SwimGroundCheck();
                
                float targetSpeed = _input._sprint ? SprintSpeed : MoveSpeed;
                if (_input._move == Vector2.zero) targetSpeed = 0.0f;
                float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
                float speedOffset = 0.1f;
                float inputMagnitude = _input.analogMovement ? _input._move.magnitude : 1f;                // accelerate or decelerate to target speed
                if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                    currentHorizontalSpeed > targetSpeed + speedOffset)
                {
                    _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                        Time.deltaTime * SpeedChangeRate);
                    _speed = Mathf.Round(_speed * 1000f) / 1000f;
                }
                else
                {
                    _speed = targetSpeed;
                }
                _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
                if (_animationBlend < 0.01f) _animationBlend = 0f;
                Vector3 inputDirection = new Vector3(_input._move.x, 0.0f, _input._move.y).normalized;
                if (_input._move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);
                    var qto = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
                    var rot = Quaternion.Slerp(transform.rotation, qto, Time.deltaTime * 5f);
                    transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rotation, 0.0f);
                   // Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
                    // if (Grounded)

                  //  _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                // new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);



                    transform.position += transform.forward * Time.deltaTime * targetSpeed;

                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                    _controller.Move(new Vector3(0.0f, 1f, 0.0f) * Time.deltaTime); //float up
                }
                if (_hasAnimator)
                {
                    _animator.SetFloat(_animIDSwimSpeed, _animationBlend);
                    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                }
                yield return null;
            }
            _currentState = State.MoveOnLand;
        }
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDSwim, false);
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                    if (!_animator.applyRootMotion) _animator.applyRootMotion = true;
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input._jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _animator.applyRootMotion = false;
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    StartCoroutine(JumpDelay());

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }


                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);

                    }
                }

                // if we are not grounded, do not jump
                _input._jump = false;
            }
        }

        private IEnumerator JumpDelay()
        {

            yield return new WaitForSeconds(.3f);

            _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);


        }
        void OnGravity()
        {
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);

            Gizmos.DrawSphere(
                new Vector3(head.position.x, head.position.y, head.position.z),
                .3f);
        }

        public void OnFootstep()
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void OnPlayerRoll()
        {
            AudioSource.PlayClipAtPoint(rollAudioClip, transform.TransformPoint(_controller.center), rollVolume);
        }
        public void OnPlayerLand()
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
        public void OnPlayerJump()
        {
            AudioSource.PlayClipAtPoint(JumpingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
        }
        public void OnHeadAboveWater()
        {
            headAboveWater = true;
        }
        public void OnHeadBelowWater()
        {
            headAboveWater = false;
        }
        public void OnButtAboveWater()
        {
            buttAboveWater = true;
        }
        public void OnButtBelowWater()
        {
            buttAboveWater = false;
        }
        bool OnIsSwimming()
        {
            if (!headAboveWater && !buttAboveWater) return true; else return false;
        }
        bool OnIsTreadingWater()
        {

            if (headAboveWater && !buttAboveWater) return true; else return false;
        }
        bool OnIsOnLand()
        {
            if (headAboveWater && buttAboveWater) return true; else return false;
        }
        IEnumerator CurrentState()
        {
            while (true)
            {
                yield return StartCoroutine(_currentState.ToString());
            }
        }

    }
}