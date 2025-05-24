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
        
        private PlayerControls _controls;
        private Vector2 _moveInput;
        private IInteractable _currentInteractable;
        
        // Called before everything else
        void Awake()
        {
            _controls = new PlayerControls();
            _controls.Player.SetCallbacks(this);
        }

        void OnEnable() => _controls.Player.Enable();
        void OnDisable() => _controls.Player.Disable();

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _moveInput = context.ReadValue<Vector2>().normalized;
            }
            else if (context.canceled)
            {
                _moveInput = Vector2.zero;
            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.performed && _currentInteractable != null)
            {
                _currentInteractable.OnInteract();
            }
        }

        // Called in sync with physics engine, all physics related stuff in here!
        void FixedUpdate()
        {
            if (_rb != null)
            {
                _rb.MovePosition(_rb.position + _moveInput * (_movementSpeed * Time.fixedDeltaTime));
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                _currentInteractable = interactable;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != null && interactable == _currentInteractable)
            {
                _currentInteractable = null;
            }
        }
    }
}

