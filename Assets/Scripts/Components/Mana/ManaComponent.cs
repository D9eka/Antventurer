using Creatures.Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Components.Mana
{
    public class ManaComponent : MonoBehaviour
    {
        [SerializeField] private float _maxMana;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;

        private float _mana;

        public EventHandler<OnValueChangeEventArgs> OnValueChange;
        public class OnValueChangeEventArgs : EventArgs
        {
            public float value;
            public float maxValue;
        }

        private void Start()
        {
            _mana = _maxMana;
            LoadData(PlayerPrefsController.GetPlayerData());

            OnValueChange?.Invoke(this, new OnValueChangeEventArgs
            {
                value = _mana,
                maxValue = _maxMana,
            });
        }

        private void LoadData(PlayerData data)
        {
            _maxMana = data.MaxMana;
            _mana = data.Mana;
        }

        public void ModifyMana(float changeValue)
        {            
            _mana = Mathf.Min(_mana + changeValue, _maxMana);
            OnValueChange?.Invoke(this, new OnValueChangeEventArgs
            {
                value = _mana,
                maxValue = _maxMana,
            });

            if (changeValue < 0)
                _onDamage?.Invoke();
            if (_mana <= 0)
                _onDie?.Invoke();
            if(changeValue > 0)
                _onHeal?.Invoke();
        }

        public (float mana, float maxMana) SaveData()
        {
            return (_mana, _maxMana);
        }
    }
}