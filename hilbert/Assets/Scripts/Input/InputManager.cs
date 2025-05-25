using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Scripts.Input
{
    public class InputManager : MonoBehaviour
    {
        public static Vector2 Movement;
        public static event Action UsePressed;

        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _useAction;

        private float _lastXTime = -1f;
        private float _lastYTime = -1f;
        private float _xValue = 0f;
        private float _yValue = 0f;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _moveAction.performed += OnMoveChanged;
            _moveAction.canceled += OnMoveChanged;
            
            _useAction = _playerInput.actions["Use"];
            _useAction.performed += OnUsePerformed;
        }

        private void OnDestroy()
        {
            _moveAction.performed -= OnMoveChanged;
            _moveAction.canceled -= OnMoveChanged;
            
            if (_useAction != null)
                _useAction.performed -= OnUsePerformed;
        }
        
        private void OnUsePerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
                UsePressed?.Invoke();
        }

        private void OnMoveChanged(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            float now = Time.unscaledTime;

            // Detect changes and update last press time
            if (Mathf.Abs(input.x) > 0.01f && Mathf.Abs(_xValue) < 0.01f)
                _lastXTime = now;
            if (Mathf.Abs(input.y) > 0.01f && Mathf.Abs(_yValue) < 0.01f)
                _lastYTime = now;

            _xValue = input.x;
            _yValue = input.y;

            // Only one axis at a time, prioritize the most recently pressed
            if (Mathf.Abs(_xValue) > 0.01f && Mathf.Abs(_yValue) > 0.01f)
            {
                if (_lastXTime > _lastYTime)
                    Movement = new Vector2(_xValue, 0f);
                else
                    Movement = new Vector2(0f, _yValue);
            }
            else if (Mathf.Abs(_xValue) > 0.01f)
            {
                Movement = new Vector2(_xValue, 0f);
            }
            else if (Mathf.Abs(_yValue) > 0.01f)
            {
                Movement = new Vector2(0f, _yValue);
            }
            else
            {
                Movement = Vector2.zero;
            }
        }
    }
}