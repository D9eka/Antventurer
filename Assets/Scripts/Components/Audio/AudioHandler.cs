using Creatures.Player;
using UnityEngine;

namespace Components.Audio
{
    public class AudioHandler : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicAudioSource;

        public static AudioHandler Instance {  get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            if(Instance != null && Instance != this)
            {
                Destroy(this);
            }

            LoadSettings();
        }

        public void LoadSettings()
        {
            _musicAudioSource.volume = PlayerPrefsController.GetMusicVolume();
        }

        public void PlayMusic(AudioClip clip)
        {
            if (_musicAudioSource.clip == clip)
                return;
            _musicAudioSource.Stop();
            _musicAudioSource.clip = clip;
            _musicAudioSource.Play();
        }
    }
}
