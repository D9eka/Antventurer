using Components.Mana;
using Components.UI;
using Creatures.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour
{
    [Header("Progress")]
    [SerializeField] private TextMeshProUGUI _progressText;
    [SerializeField] private Slider _progressSlider;
    [Space]
    [SerializeField] private Button _skillsButton;

    private void OnEnable()
    {
        SetSkillButtonState();
        UpdateSlider();
    }

    private void SetSkillButtonState()
    {
        Debug.Log(PlayerPrefsController.GetPlayerDoubleJumpState());
        _skillsButton.interactable = PlayerPrefsController.GetPlayerDoubleJumpState();
    }

    public void UpdateSlider()
    {
        float value = PlayerPrefsController.GetPlayerProgress();
        _progressText.text = value.ToString() + '%';
        _progressSlider.value = value;
    }
}
