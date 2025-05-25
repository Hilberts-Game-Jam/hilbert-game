using UnityEngine;
using Scripts.Interfaces;

namespace Corridor {
    public class InteractableDoor : MonoBehaviour, IInteractable
    {
        public void OnInteract()
        {
            Debug.Log("Door interacted!");
            // Add your door logic here (open, close, etc.)
        }
    }
}
