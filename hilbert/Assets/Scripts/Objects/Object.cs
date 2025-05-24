using Scripts.Core;
using Scripts.Interfaces;
using UnityEngine;


namespace Scripts.Objects
{
    public class Object : MonoBehaviour, IInteractable
    {
        private ClassesAndEnums.ObjectType objectType;
        private ClassesAndEnums.ObjectOverlayType objectOverlayType;
        
        private bool _isInteractable;
        private int _diceValue;
        void Awake()
        {
            _diceValue = ClassesAndEnums.D20.Roll();
            if (_diceValue > 10)
            {
                _isInteractable = true;
            }
            else
            {
                _isInteractable = false;
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
        public void OnInteract()
        {
            if (_isInteractable)
            {
                // Perform interaction logic here
                Debug.Log("Interacted with object: " + gameObject.name);
            }
        }

        private void BuildObject()
        {
            objectType = (ClassesAndEnums.ObjectType)Random.Range(0, System.Enum.GetValues(typeof(ClassesAndEnums.ObjectType)).Length);
            objectOverlayType = (ClassesAndEnums.ObjectOverlayType)Random.Range(0, System.Enum.GetValues(typeof(ClassesAndEnums.ObjectOverlayType)).Length);
            // TODO: Add logic to build the object based on its type, overlay type and _isInteractable status
        }
    }
}