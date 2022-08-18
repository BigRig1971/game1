using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using TMPro;
using System;

namespace StupidHumanGames
{
    public class CreatureMount : MonoBehaviour
    {
        AudioSource _audioSource;
        [SerializeField] AudioClip[] _randomAttackSound;
        [SerializeField] float _randomAttackSoundVolume = 1f;
        [SerializeField] AudioClip[] _randomSound;
        [SerializeField] float _randomSoundVolume = 1f;
        [SerializeField] AudioClip[] FootstepAudioClips;
        [SerializeField] float FootstepAudioVolume = 1f;
        [SerializeField] Transform _camRootPlayer;
        [SerializeField] Transform _camRootMount;
        Vector3 _prevCamRoot;
        [SerializeField] string _animation;
        [SerializeField] bool rootMotion = true;
        [SerializeField] GameObject _mountUI;
        [SerializeField] float walkSpeed = .5f;
        [SerializeField] float runSpeed = 1f;
        Quaternion currentRotation;
        public float RotationSmoothTime = 0.12f;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private GameObject _mainCamera;
        [SerializeField] bool groundHugging = true;
        Quaternion targetRot;
        [SerializeField] Transform _saddlePoint;
        [SerializeField] Transform _dismountPoint;
        Rigidbody rb;
        Animator _animator;
        Animator _thisAnimator;
        bool _isMounted = false;
        bool _canMount = false;
        StarterAssetsInputs _input;
        Transform _player;
        ThirdPersonController _tpc;      
        [SerializeField] LayerMask groundLayer;
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            rb = GetComponent<Rigidbody>();
            _prevCamRoot = _camRootPlayer.localPosition;
            _mountUI = Instantiate(_mountUI);
            _mountUI.SetActive(false);
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
            _thisAnimator = GetComponent<Animator>();
            OnYPosition();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _tpc = other.GetComponent<ThirdPersonController>();
                _player = other.GetComponent<Transform>();
                _animator = other.GetComponent<Animator>();
                _input = other.GetComponent<StarterAssetsInputs>();

                if (!_tpc.isMounted)
                {
                   
                    _canMount = true;
                    if (!_mountUI.activeSelf)
                    {
                        _mountUI.SetActive(true);
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                
                if (_mountUI.activeSelf)
                {
                    _canMount = false;
                    _mountUI.SetActive(false);
                    _player.transform.SetParent(null);
                }
            }
        }
        private void Update()
        {
            OnMount();
            if (RandomBool(100)) OnRandomSound();
        }
        void OnMount()
        {
           
            if (Input.GetKeyDown(KeyCode.M) && _canMount)
            {
                _isMounted = !_isMounted;

                if (_isMounted)
                {
                    _camRootPlayer.SetParent(_camRootMount);

                    _camRootPlayer.localPosition = Vector3.zero;
                    if (_mountUI.activeSelf) _mountUI.SetActive(false);
                    if (_animator != null) _animator.SetBool(_animation, true);
                    _player.transform.SetParent(_saddlePoint);
                    _player.transform.localPosition = _saddlePoint.localPosition;
                    _player.transform.localRotation = _saddlePoint.localRotation;
                    _tpc.OnEnableMount();
                }
                else
                {
                    if (!_mountUI.activeSelf) _mountUI.SetActive(true);
                    _camRootPlayer.SetParent(_player);
                    _camRootPlayer.localPosition = _prevCamRoot;
                    if (_animator != null) _animator.SetBool(_animation, false);
                    _player.transform.position = _dismountPoint.position;
                    _tpc.OnDisableMount();
                    _player.transform.rotation = Quaternion.Euler(0f, _player.rotation.eulerAngles.y, 0f);
                }
            }
            if (_isMounted) OnMove();
        }
        private void OnMove()
        {
            float speed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                speed = runSpeed;
            }
            else
            {
                speed = walkSpeed;
            }
            if (_player != null)
            {
                _player.transform.localPosition = _saddlePoint.localPosition;
                _player.transform.localRotation = _saddlePoint.localRotation;
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {

                    _thisAnimator.SetTrigger("Attack");
                }
                Vector3 inputDirection = new Vector3(_input._move.x, 0.0f, _input._move.y).normalized;
                if (_input._move != Vector2.zero)
                {
                    _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                      _mainCamera.transform.eulerAngles.y;
                    float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                        RotationSmoothTime);
                    transform.rotation = Quaternion.Euler(currentRotation.eulerAngles.x, rotation, currentRotation.eulerAngles.z);
                }
                if (_input != null && _input._move != Vector2.zero) _thisAnimator.SetFloat("Blend", speed); else _thisAnimator.SetFloat("Blend", 0f);
                OnYPosition();
            }
        }
        void OnYPosition()
        {
            Vector3 position = transform.position;
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + transform.up.y * 2f, transform.position.z),
                -transform.up, out hit, 20, groundLayer))
            {
                if (groundHugging)
                {
                    targetRot = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                    currentRotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime / 0.15f);
                    position.y = Terrain.activeTerrain.SampleHeight(transform.position) + .01f;
                    transform.position = position;
                }
            }
        }
        private void OnFootstep(AnimationEvent OnfootStep)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
                if (OnfootStep.animatorClipInfo.weight > .8f) _audioSource.PlayOneShot(FootstepAudioClips[index], FootstepAudioVolume);
            }
        }
        private void OnRandomSound()
        {
            if (_randomSound.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, _randomSound.Length);
                _audioSource.PlayOneShot(_randomSound[index], _randomSoundVolume);
            }
        }
        private void OnRandomAttackSound(AnimationEvent OnAttack)
        {
            if (_randomSound.Length > 0)
            {
                var index = UnityEngine.Random.Range(0, _randomAttackSound.Length);
                if (OnAttack.animatorClipInfo.weight > .2f) _audioSource.PlayOneShot(_randomAttackSound[index], _randomAttackSoundVolume);
            }
        }
        bool RandomBool(int rn)
        {
            int rnd = UnityEngine.Random.Range(0, rn);
            int rnd2 = UnityEngine.Random.Range(0, rn);
            if (rnd == rnd2) return true; else return false;
        }

       

    }
}

