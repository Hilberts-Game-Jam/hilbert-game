using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Core;
using Scripts.Input;
using Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Scripts.Player
{
    public class Player : MonoBehaviour
    {
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveY = Animator.StringToHash("MoveY");
        private static readonly int Speed = Animator.StringToHash("Speed");
        [SerializeField] private float _movementSpeed = 5f;
        
        private Vector2 _movement;
        
        private Rigidbody2D _rb;
        private Animator _playerAnimator;
        private AudioSource _footStepSource;
        
        private IInteractable _currentInteractable;
        
        public Dictionary<ClassesAndEnums.KeyObject, int> PlayerKeyObjects = new Dictionary<ClassesAndEnums.KeyObject, int>();
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
            _footStepSource = GetComponent<AudioSource>();
            _footStepSource.volume = 0f;
        }
        
        private void OnEnable()
        {
            InputManager.UsePressed += HandleUse;
        }

        private void OnDisable()
        {
            InputManager.UsePressed -= HandleUse;
        }

        private void HandleUse()
        {
            OnUse(new InputAction.CallbackContext());
        }
        
        public void OnUse(InputAction.CallbackContext context)
        {
            _currentInteractable?.OnInteract();
        }
        
        private IEnumerator FadeInFootstepVolume()
        {
            float timeElapsed = 0f;
            float duration = 2f; // Duration for the fade-in

            while (timeElapsed < duration)
            {
                _footStepSource.volume = Mathf.Lerp(0f, 1f, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null; // Wait for the next frame
            }

            _footStepSource.volume = 1f; // Ensure the volume is set to 1 at the end
        }
        
        private void Update()
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
            
            _rb.linearVelocity = _movement.normalized * _movementSpeed;
            
            if (_playerAnimator != null)
            {
                _playerAnimator.SetFloat(MoveX, _movement.x);
                _playerAnimator.SetFloat(MoveY, _movement.y);
                _playerAnimator.SetFloat(Speed, _movement.SqrMagnitude());
            }
            if (_movement != Vector2.zero)
            {
                if (!_footStepSource.isPlaying)
                {
                    _footStepSource.pitch = Random.Range(0.8f, 1.2f);
                    _footStepSource.Play();
                    StartCoroutine(FadeInFootstepVolume());
                }
                float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            } else
            {
                if (_footStepSource.isPlaying)
                {
                    _footStepSource.Stop();
                    _footStepSource.volume = 0f;
                    StopAllCoroutines();
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                _currentInteractable = interactable;
                Debug.Log(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactable == _currentInteractable)
            {
                _currentInteractable = null;
                Debug.Log("Exited interactable: " + interactable);
            }
        }
    }
}
