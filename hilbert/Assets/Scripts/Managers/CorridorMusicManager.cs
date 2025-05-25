using System.Collections;
using UnityEngine;

namespace Managers
{
    public class CorridorMusicManager : MonoBehaviour
    {
        public AudioClip[] musicClips;
        private AudioSource _audioSource;
        private readonly float _playChance = 1f;

        IEnumerator Start()
        {
            _audioSource = GetComponent<AudioSource>();
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            PlayMusicOccasionally();
        }

        private void PlayMusicOccasionally()
        {
            if (Random.value < _playChance)
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
}