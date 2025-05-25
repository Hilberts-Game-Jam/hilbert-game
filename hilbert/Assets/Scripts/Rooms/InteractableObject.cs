using System;
using Scripts.Core;
using Scripts.Interfaces;
using UnityEngine;

namespace Scripts.Rooms
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        public ClassesAndEnums.InteractionType InteractionType { get; private set; }
        [SerializeField] private ClassesAndEnums.InteractionType interactionType;

        public void Awake()
        {
            InteractionType = interactionType;
        }

        public void OnInteract()
        {
            Debug.Log("InteractableObject interacted with: " + InteractionType);
            Events.TriggerInteraction(InteractionType);
        }
    }
}