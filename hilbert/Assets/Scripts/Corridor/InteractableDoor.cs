using System;
using System.Collections;
using UnityEngine;
using Scripts.Interfaces;
using Scripts.Managers;
using UnityEngine.SceneManagement;

namespace Corridor {
    public class InteractableDoor : MonoBehaviour, IInteractable
    {
        public String leadsTo;
        private AudioManager _audioManager;

        public void Awake()
        {
            _audioManager = FindFirstObjectByType<AudioManager>();
        }

        public void OnInteract()
        {
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
