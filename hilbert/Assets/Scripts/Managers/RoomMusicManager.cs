using UnityEngine;

namespace Scripts.Managers
{
    public class RoomMusicManager : MonoBehaviour
    {
        public AudioClip[] musicClips;
        private AudioSource _audioSource;

        public void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            PlayMusic();
        }

        public void PlayMusic()
        {
            switch (WeirdnessManager.WeirdnessLevel)
            {
                case 1:
                    _audioSource.clip = musicClips[0];
                    break;
                case 2:
                    _audioSource.clip = musicClips[1];
                    break;
                case 3:
                    _audioSource.clip = musicClips[2];
                    break;
            }
            _audioSource.Play();
        }
    }
}