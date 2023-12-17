using Components.Mana;
using Creatures.Player;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public class HUD : MonoBehaviour
    {
        [Header("Message")]
        [SerializeField] private GameObject _message;
        [SerializeField] private TextMeshProUGUI _messageHandler;
        [Header("Mana")]
        [SerializeField] private TextMeshProUGUI _manaValue;
        [SerializeField] private Slider _manaSlider;
        [Header("Location")]
        [SerializeField] private GameObject _location;
        [SerializeField] private TextMeshProUGUI _locationName;

        public static HUD Instance { get; private set; }

        private void Start()
        {
            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;

            PlayerController.Instance.GetComponent<ManaComponent>().OnValueChange += HUD_OnValueChange;

            if (PlayerPrefsController.TryGetPlayerData(out PlayerData data))
                LoadData(data);



            StartCoroutine(ViewMessage(_location, _locationName, Location.Instance.Name, 5f));
        }

        private void LoadData(PlayerData data)
        {
            UpdateMana(data.Mana, data.MaxMana);
        }    

        public void SendMessage(string message, float viewTime)
        {
            StartCoroutine(ViewMessage(_message, _messageHandler, message, viewTime));
        }

        private IEnumerator ViewMessage(GameObject go, TextMeshProUGUI textHandler, string message, float viewTime)
        {
            textHandler.text = message;
            go.SetActive(true);
            yield return new WaitForSeconds(viewTime);
            textHandler.text = string.Empty;
            go.SetActive(false);
        }

        private void HUD_OnValueChange(object sender, ManaComponent.OnValueChangeEventArgs e)
        {
            UpdateMana(e.value, e.maxValue);
        }

        private void UpdateMana(float value, float maxValue)
        {
            _manaValue.text = ((int)value).ToString();
            _manaSlider.maxValue = maxValue;
            _manaSlider.value = value;
        }
    }
}