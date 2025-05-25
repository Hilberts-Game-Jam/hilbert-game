using System;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Managers;
using UnityEngine.SceneManagement;

namespace Corridor {
    public class InteractableDoor : MonoBehaviour, IInteractable
    {
        public String leadsTo;
        public void OnInteract()
        {
            Debug.Log("Door interacted!");
            SceneManager.LoadScene(leadsTo);
        }
    }
}
