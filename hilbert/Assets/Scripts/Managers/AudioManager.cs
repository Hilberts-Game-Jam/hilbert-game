using System;
using UnityEngine;

namespace Scripts.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] public AudioClip[] _audioClips;
        [SerializeField] private AudioSource _audioSource;

 
        public void PlayDoorOpen()
        {
            _audioSource.clip = _audioClips[0];
            _audioSource.Play();
        }

        public void PlayElevatorDing()
        {
            _audioSource.clip = _audioClips[1];
            _audioSource.Play();

        }

        public void PlayKeyItemFound()
        {
            _audioSource.clip = _audioClips[2];
            _audioSource.Play();
        }
    }
}
