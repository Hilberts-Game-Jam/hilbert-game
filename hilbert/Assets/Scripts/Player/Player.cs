using System;
using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using Scripts.Interfaces;

namespace Scripts.Player
{
    public class Player : MonoBehaviour, PlayerControls.IPlayerActions
    {
        [Header("Movement")] 
        [SerializeField] private float _movementSpeed;
        [Header("Rigid Body")]
        [SerializeField] private Rigidbody2D _rb;
        [Header("Collider")]
        [SerializeField] private Collider2D _collider;

        [SerializeField] private Animator _playerAnimator;
        
        private PlayerControls _controls;
        private Vector2 _moveInput;
        private IInteractable _currentInteractable;
        
        // Called before everything else
        private void Awake()
        {
            _controls = new PlayerControls();
            _controls.Player.SetCallbacks(this);
        }

        private void OnEnable() => _controls.Player.Enable();
        private void OnDisable() => _controls.Player.Disable();

        private void Update()
        {
            _playerAnimator.SetFloat("Speed", _moveInput.magnitude);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _moveInput = context.ReadValue<Vector2>().normalized;
                float zRotation = transform.rotation.eulerAngles.z;
                if (_moveInput.y > 0 && Mathf.Abs(_moveInput.x) < 0.1f)
                    zRotation = 180;
                else if (_moveInput.y < 0 && Mathf.Abs(_moveInput.x) < 0.1f)
                    zRotation = 0;
                else if (_moveInput.x < 0 && Mathf.Abs(_moveInput.y) < 0.1f)
                    zRotation = -90;
                else if (_moveInput.x > 0 && Mathf.Abs(_moveInput.y) < 0.1f)
                    zRotation = 90;

                transform.rotation = Quaternion.Euler(0, 0, zRotation);
            }
            else if (context.canceled)
            {
                _moveInput = Vector2.zero;
            }
        }
        public void OnUse(InputAction.CallbackContext context)
        {
            Events.TriggerInteraction(); // todo: remove this here, just for testing purposes
            if (context.performed && _currentInteractable != null)
            {
                _currentInteractable.OnInteract();
                Events.TriggerInteraction(); // todo: may optionally pass a InteractionType if needed
            }
        }

        // Called in sync with physics engine, all physics related stuff in here!
        private void FixedUpdate()
        {
            if (_rb != null)
            {
                _rb.MovePosition(_rb.position + _moveInput * (_movementSpeed * Time.fixedDeltaTime));
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

