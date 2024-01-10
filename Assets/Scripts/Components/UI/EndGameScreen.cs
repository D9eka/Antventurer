using Components.Audio;
using Creatures.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundMusic;
    [Space]
    [SerializeField] private TextMeshProUGUI _playerProgress;
    [SerializeField] private TextMeshProUGUI _endGameText;
    [Space]
    [SerializeField] private string _zeroPercentText;
    [SerializeField] private string _fourtyPercentText;
    [SerializeField] private string _seventyPercentText;
    [SerializeField] private string _hundredPercentText;

    private void Start()
    {
        if (_backgroundMusic != null)
            AudioHandler.Instance.PlayMusic(_backgroundMusic);

        float playerProgress = PlayerPrefsController.GetPlayerProgress();
        _playerProgress.text = $"{playerProgress}%";
        LayoutRebuilder.ForceRebuildLayoutImmediate(_playerProgress.transform.parent.GetComponent<RectTransform>());

        if (playerProgress <= 42f)
            _endGameText.text = _zeroPercentText;
        else if (playerProgress <= 70f)
            _endGameText.text = _fourtyPercentText;
        else if (playerProgress <= 99f)
            _endGameText.text = _seventyPercentText;
        else
            _endGameText.text = _hundredPercentText;
    }
}
