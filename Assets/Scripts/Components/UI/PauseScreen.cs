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

    private void Start()
    {
        PlayerController.Instance.OnChangeProgress += PauseScreen_OnChangeProgress;
    }

    public void PauseScreen_OnChangeProgress(object sender, int e)
    {
        _progressText.text = e.ToString();
        _progressSlider.value = e;
    }
}
