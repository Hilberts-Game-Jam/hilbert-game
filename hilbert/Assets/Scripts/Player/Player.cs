using System.Collections.Generic;
using Scripts.Core;
using Scripts.Input;
using Scripts.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private Collider2D _collider;
        
        private IInteractable _currentInteractable;
        
        public Dictionary<ClassesAndEnums.KeyObject, int> PlayerKeyObjects = new Dictionary<ClassesAndEnums.KeyObject, int>();
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
            _collider = GetComponent<Collider2D>();
        }
        
        private void OnEnable()
        [SerializeField] private GameObject _doorInteractablePrefab;
        [SerializeField] private GameObject _elevatorInteractablePrefab;
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
            
            if (context.performed && _currentInteractable != null)
            {
                _currentInteractable.OnInteract();
                
                // todo: pass correct interaction type for done interaction -> Item, Door, Elevator, etc.
                Events.TriggerInteraction(ClassesAndEnums.InteractionType.ITEM);
            }
            else
            {
                // Event for NONE interaction type interaction
                Events.TriggerInteraction();
            }
        }
        
        private void Update()
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
            
            _rb.linearVelocity = _movement * _movementSpeed;
            
            if (_playerAnimator != null)
            {
                _playerAnimator.SetFloat(MoveX, _movement.x);
                _playerAnimator.SetFloat(MoveY, _movement.y);
                _playerAnimator.SetFloat(Speed, _movement.SqrMagnitude());
            }
            if (_movement != Vector2.zero)
            {
                float angle = Mathf.Atan2(_movement.y, _movement.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle + 90);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                _currentInteractable = interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactable == _currentInteractable)
            {
                _currentInteractable = null;
            }
        }
    }
}
