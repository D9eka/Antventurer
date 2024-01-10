using Components.Audio;
using UnityEngine;

namespace Components.UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;

        private void Start()
        {
            if (_backgroundMusic != null)
                AudioHandler.Instance.PlayMusic(_backgroundMusic);
        }
    }
}