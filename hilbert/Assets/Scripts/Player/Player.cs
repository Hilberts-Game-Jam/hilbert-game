using System.Collections;
using System.Collections.Generic;
using Scripts.Core;
using Scripts.Input;
using Scripts.Interfaces;
using Scripts.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
        [SerializeField][Range(0f,1f)] float _footStepVolume = 0.2f;

        [SerializeField] private static int _doorCount;
        [SerializeField] private static float _roomCount;
        [SerializeField] private static readonly int _maxRoomCount = 20;
        [SerializeField] private static int _resets = 0;
        
        private IInteractable _currentInteractable;

        public static Dictionary<ClassesAndEnums.KeyObject, int> PlayerKeyObjects =
            new Dictionary<ClassesAndEnums.KeyObject, int>()
            {
                { ClassesAndEnums.KeyObject.MIRROR_SHARD , 0}
            };
        
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
            if (_currentInteractable != null)
            {
                Debug.Log("Interacted with: " + _currentInteractable);
                _doorCount++;
                _roomCount = _doorCount / 2f;
            }
            else
            {
                Debug.Log("No interactable object in range.");
            }
        }
        
        private IEnumerator FadeInFootstepVolume()
        {
            float timeElapsed = 0f;
            float duration = 1f; // Duration for the fade-in

            while (timeElapsed < duration)
            {
                _footStepSource.volume = Mathf.Lerp(0f, _footStepVolume, (float) (timeElapsed / duration));
                timeElapsed += Time.deltaTime;
                yield return null; // Wait for the next frame
            }

            _footStepSource.volume = _footStepVolume; // Ensure the volume is set to max at the end
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

            if (_roomCount == _maxRoomCount - 1)
            {
                Debug.Log("Bald wirds hell oder so :).");
            }

            if (_roomCount >= _maxRoomCount)
            {
                Debug.Log("Reset to start Scene");
                WeirdnessManager.ResetWeirdness();
                _roomCount = 0f;
                _doorCount = 0;
                _resets++;
                SceneManager.LoadScene("StartRoom");
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
