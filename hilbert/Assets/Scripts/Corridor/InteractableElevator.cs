using System.Collections;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Managers;
using Scripts.Core;

namespace Scripts.Corridor {
    public class InteractableElevator : MonoBehaviour, IInteractable
    {
        private AudioManager _audioManager;
        
        public ClassesAndEnums.InteractionType InteractionType { get; private set; }

        public void Awake()
        {
            _audioManager = FindFirstObjectByType<AudioManager>();
            InteractionType = ClassesAndEnums.InteractionType.ELEVATOR;
        }

        public void OnInteract()
        {
            Events.TriggerInteraction(InteractionType);
            Debug.Log("Elevator interacted!");
            StartCoroutine(waitSound(_audioManager._audioClips[1]));
            WeirdnessManager.IncrementWeirdness();
            Debug.Log("Corridor regenerated");
            CorridorManager.Instance.DestroyEveryTile(); //This fails
            CorridorManager.Instance.GenerateInitialCorridor();
        }
        
        private IEnumerator waitSound(AudioClip clip)
        {
            _audioManager.PlayElevatorDing();
            yield return new WaitForSeconds(clip.length + 1);
        }
    }   
}