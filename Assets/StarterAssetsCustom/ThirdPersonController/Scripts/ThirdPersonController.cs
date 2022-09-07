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
        public bool _canMove = true;
        public float _surfaceDist = 0f;
        Quaternion currentRotation;
       public bool groundHugging = false; 
        Quaternion targetRot;
        public bool isMounted = false;
        public AudioSource _audioSource;
        float lerpG = 0f;
        bool animEnable = false;
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
        public Animator _animator;
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
           transform.localRotation = Quaternion.identity;
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
            StartCoroutine(CurrentState());
        }
        private void Update()
        {
            OnGravity();
            
           

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
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }
        private void SwimGroundCheck()
        {
            Vector3 spherePosition = new Vector3(head.position.x, head.position.y,
               head.position.z);
            GroundedUnderWater = (Physics.CheckSphere(spherePosition, 1f, GroundLayers,
                 QueryTriggerInteraction.Ignore));
            if (GroundedUnderWater && _hasAnimator)
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
            if (_input._look.sqrMagnitude >= _threshold && !LockCameraPosition && !InventoryManager.IsOpen())
            {
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1f : Time.deltaTime;
                _cinemachineTargetYaw += _input._look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input._look.y * deltaTimeMultiplier;
            }
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }
        IEnumerator MoveOnLand()
        {

            if (_hasAnimator && !_animator.applyRootMotion) _animator.applyRootMotion = true;
           if(_hasAnimator) _animator.SetBool(_animIDSwim, false);
            while (OnIsOnLand())
            {
                GroundedCheck();
                JumpAndGravity();

                if (_input._roll && !InventoryManager.IsOpen() && _canMove)
                {
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDRoll, true);
                        _input._roll = false;
                    }
                }
                if (Input.GetKeyDown(KeyCode.J)) _animator.SetTrigger("RandomIdle");
              
                if(groundHugging) OnYPosition();
                float targetSpeed = _input._sprint ? SprintSpeed : MoveSpeed;
                if (_input._move == Vector2.zero) targetSpeed = 0.0f;
                float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
                float speedOffset = 0.1f;
                float inputMagnitude = _input.analogMovement ? _input._move.magnitude : 1f;
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
                if (_input._move != Vector2.zero && !InventoryManager.IsOpen() && _canMove)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);
                    if (!isMounted)
                    {
                        if (groundHugging)
                        {
                            OnYPosition();
                            transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, rotation, currentRotation.eulerAngles.z);
                        }
                        else
                        {
                            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                        }
                        
                    }
                }
                Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
                if (!_animator.applyRootMotion)
                {
                    _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                                 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
                }
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
                SwimGroundCheck();
                OnSwim(-2);

                yield return null;
            }
            _currentState = State.Swimming;
        }
        IEnumerator Swimming()
        {
            if (_animator.applyRootMotion) _animator.applyRootMotion = false;
           if(_hasAnimator) _animator.SetBool(_animIDSwim, true);
            while (OnIsSwimming())
            {
                SwimGroundCheck();
                OnSwim(.5f);

                yield return null;
            }
            _currentState = State.MoveOnLand;
        }
        void OnSwim(float gravity)
        {
            float targetSpeed = _input._sprint ? SprintSpeed : MoveSpeed;
            if (_input._move == Vector2.zero) targetSpeed = 0.0f;
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            Vector3 inputDirection = new Vector3(_input._move.x, 0.0f, _input._move.y).normalized;
            if (_input._move != Vector2.zero && _canMove && !InventoryManager.IsOpen())
            {

                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, .2f);
               if(!isMounted) transform.rotation = Quaternion.Euler(_mainCamera.transform.rotation.eulerAngles.x, rotation, 0f);
                transform.position += transform.forward * Time.deltaTime * targetSpeed;
            }
            else
            {


               if(!isMounted) transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
                _controller.Move(new Vector3(0.0f, GravityLerp(gravity) * Time.deltaTime));
                

            }


            float inputMagnitude = _input.analogMovement ? _input._move.magnitude : 1f;
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }
        float GravityLerp(float goal)
        {

            float delta = goal - lerpG;
            delta *= Time.deltaTime;
            lerpG += delta;
            return lerpG;
        }
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDSwim, false);
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                    if (!_animator.applyRootMotion) _animator.applyRootMotion = true;
                }
                if (_verticalVelocity < 0.0f)  // stop our velocity dropping infinitely when grounded
                {
                    _verticalVelocity = -2f;
                }
                if (_input._jump && _jumpTimeoutDelta <= 0.0f) // Jump
                {
                    
                    StartCoroutine(JumpDelay());  // the square root of H * -2 * G = how much velocity needed to reach desired height
                    if (_hasAnimator)  // update animator if using character
                    {
                        _animator.applyRootMotion = false;
                        _animator.SetBool(_animIDJump, true);
                    }
                }
                if (_jumpTimeoutDelta >= 0.0f)  // jump timeout
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;   // reset the jump timeout timer
                if (_fallTimeoutDelta >= 0.0f)  // fall timeout
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_hasAnimator) // update animator if using character
                    {
                        _animator.SetBool(_animIDFreeFall, true);

                    }
                }
                _input._jump = false;  // if we are not grounded, do not jump
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
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
            Gizmos.DrawSphere(
                new Vector3(head.position.x, head.position.y, head.position.z),
                .3f);
        }

        private void OnFootstep(AnimationEvent OnfootStep)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                if (OnfootStep.animatorClipInfo.weight > .8f) _audioSource.PlayOneShot(FootstepAudioClips[index], FootstepAudioVolume);

            }
        }
        public void OnPlayerRoll()
        {
            _audioSource.PlayOneShot(rollAudioClip, rollVolume);
        }
        public void OnPlayerLand(AnimationEvent OnPlayerLand)
        {
            if (OnPlayerLand.animatorClipInfo.weight > .5f) _audioSource.PlayOneShot(LandingAudioClip, FootstepAudioVolume);
        }
        public void OnPlayerJump()
        {
            _audioSource.PlayOneShot(JumpingAudioClip, FootstepAudioVolume);
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
        public void OnEnableMount()
        {
            isMounted = true;
        }
        public void OnDisableMount()
        {
            isMounted = false;
        }
        public void OnYPosition()
        {
            Vector3 position = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.up.y, transform.position.z),
                -transform.up, out hit, 20, GroundLayers))
            {
                targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                currentRotation = transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / 0.15f);
               // position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .01f;
                //transform.position = position;
            }
        }

       

    }
}