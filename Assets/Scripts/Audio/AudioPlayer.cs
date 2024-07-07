using UnityEngine;

namespace Audio
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _placementSound;
        [SerializeField] private AudioSource _audioSource;

        public static AudioPlayer instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this.gameObject);
        }

        public void PlayPlacementSound()
        {
            if (_placementSound != null)
                _audioSource.PlayOneShot(_placementSound);
        }
    }
}