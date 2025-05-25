using System.Collections;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Managers;

namespace Corridor {
    public class InteractableElevator : MonoBehaviour, IInteractable
    {
        private AudioManager _audioManager;

        public void Awake()
        {
            _audioManager = FindFirstObjectByType<AudioManager>();
        }
        public void OnInteract()
        {
            Debug.Log("Elevator interacted!");
            StartCoroutine(waitSound(_audioManager._audioClips[1]));
            
        }
        
        private IEnumerator waitSound(AudioClip clip)
        {
            _audioManager.PlayElevatorDing();
            yield return new WaitForSeconds(clip.length + 1);
        }
    }   
}