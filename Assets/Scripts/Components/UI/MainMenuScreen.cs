using Components.Audio;
using InstantGamesBridge.Modules.Platform;
using InstantGamesBridge;
using UnityEngine;
using Creatures.Player;
using InstantGamesBridge.Common;

namespace Components.UI
{
    public class MainMenuScreen : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;
        [Space]
        [SerializeField] private GameObject _contunueButton;

        private void Start()
        {
            if (_backgroundMusic != null)
                AudioHandler.Instance.PlayMusic(_backgroundMusic);

            Bridge.platform.SendMessage(PlatformMessage.GameReady);
            if (Bridge.platform.id == PlatformId.Yandex)
                _contunueButton.SetActive(PlayerPrefsController.GetDataFromServer());
            else
                _contunueButton.SetActive(PlayerPrefsController.HaveData());
            Debug.Log(PlayerPrefsController.HaveData());
        }
    }
}