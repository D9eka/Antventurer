using Assets.Scripts.Creatures.Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Creatures.Player.Player;

namespace Components.Mana
{
    public class ManaComponent : MonoBehaviour
    {
        [SerializeField] private int _maxMana;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;

        private int _mana;

        public EventHandler<OnValueChangeEventArgs> OnValueChange;
        public class OnValueChangeEventArgs : EventArgs
        {
            public int value;
            public int maxValue;
        }

        public int Mana => _mana;
        public int MaxMana => _maxMana;

        private void Start()
        {
            if(PlayerPrefsController.TryGetPlayerMana(out var mana)) 
            { 
                _mana = mana;
            }
            else
            {
                _mana = _maxMana;
            }

            OnValueChange?.Invoke(this, new OnValueChangeEventArgs
            {
                value = _mana,
                maxValue = _maxMana,
            });
        }

        public void ModifyMana(int changeValue)
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
    }
}