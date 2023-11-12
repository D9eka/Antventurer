using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Components.Mana
{
    public class ManaComponent : MonoBehaviour
    {
        [SerializeField] private int _maxMana;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Slider _slider;

        private int _mana;

        private void Start()
        {
            _mana = _maxMana;
            UpdateUI();
        }

        public void ModifyMana(int changeValue)
        {            
            _mana = Mathf.Min(_mana + changeValue, _maxMana);
            UpdateUI();
             
            if(changeValue < 0)
                _onDamage?.Invoke();
            if (_mana <= 0)
                _onDie?.Invoke();
            if(changeValue > 0)
                _onHeal?.Invoke();
        }

        private void UpdateUI()
        {
            _text.text = _mana.ToString();
            _slider.value = _mana / 100.0f; 
        }
    }
}