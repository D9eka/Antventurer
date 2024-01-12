using InstantGamesBridge;
using InstantGamesBridge.Modules.Game;
using UnityEngine;

namespace Examples
{
    public class Example : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicAudioSource;

        private void Start()
        {
            _musicAudioSource.Play();
            Bridge.game.visibilityStateChanged += OnGameVisibilityStateChanged;
        }

        private void OnGameVisibilityStateChanged(VisibilityState visibilityState)
        {
            switch (visibilityState)
            {
                case VisibilityState.Visible:
                    _musicAudioSource.Play();
                    break;

                case VisibilityState.Hidden:
                    _musicAudioSource.Pause();
                    break;
            }
        }
    }
}