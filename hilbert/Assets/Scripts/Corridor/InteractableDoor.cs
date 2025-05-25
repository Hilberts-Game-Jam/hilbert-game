using System;
using System.Collections;
using Scripts.Core;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Managers;
using UnityEngine.SceneManagement;

namespace Scripts.Corridor {
    public class InteractableDoor : MonoBehaviour, IInteractable
    {
        public String leadsTo;
        private AudioManager _audioManager;
        public ClassesAndEnums.InteractionType InteractionType { get; private set; }

        public void Awake()
        {
            _audioManager = FindFirstObjectByType<AudioManager>();
            InteractionType = ClassesAndEnums.InteractionType.DOOR;
        }

        public void OnInteract()
        {
            Events.TriggerInteraction(InteractionType);
            StartCoroutine(waitSound(_audioManager._audioClips[0]));
            SceneManager.LoadScene(leadsTo);
        }

        private IEnumerator waitSound(AudioClip clip)
        {
            _audioManager.PlayDoorOpen();
            yield return new WaitForSeconds(clip.length + 1);
        }
    }
}
