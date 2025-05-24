using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Player
{
    public class Player : MonoBehaviour, PlayerControls.IPlayerActions
    {
        [Header("Movement")] 
        [SerializeField] private float _movementSpeed;
        [Header("Rigid Body")]
        [SerializeField] private Rigidbody2D _rb;
        
        private PlayerControls _controls;
        private Vector2 _moveInput;
        
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
            throw new System.NotImplementedException();
        }

        // Called in sync with physics engine, all physics related stuff in here!
        void FixedUpdate()
        {
            if (_rb != null)
            {
                _rb.MovePosition(_rb.position + _moveInput * (_movementSpeed * Time.fixedDeltaTime));
            }
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}

