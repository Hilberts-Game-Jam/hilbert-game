using UnityEngine;
using Scripts.Interfaces;

namespace Corridor {
    public class InteractableElevator : MonoBehaviour, IInteractable
    {
        public void OnInteract()
        {
            Debug.Log("Elevator interacted!");
            // Add your elevator logic here
        }
    }   
}