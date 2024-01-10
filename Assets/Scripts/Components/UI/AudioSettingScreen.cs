using Components.Audio;
using Creatures.Player;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingScreen : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    private void Start()
    {
        _volumeSlider.value = PlayerPrefsController.GetMusicVolume();
        _volumeSlider.onValueChanged.AddListener(value => SaveVolumeValue(_volumeSlider.value));
    }

    private void SaveVolumeValue(float value)
    {
        PlayerPrefsController.SetMusicVolume(value);
        AudioHandler.Instance.LoadSettings();
    }
}
