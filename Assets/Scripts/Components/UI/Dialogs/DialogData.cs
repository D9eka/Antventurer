using System;
using UnityEngine;

namespace Components.UI.Dialogs
{
    [Serializable]
    public class DialogData
    {
        [SerializeField] private Sentence[] _sentences;
        public Sentence[] Sentences => _sentences;
    }

    [Serializable]
    public struct Sentence
    {
        [SerializeField] private string _value;
        [SerializeField] private AudioClip _voice;

        public string Value => _value;
        public AudioClip Voice => _voice;
    }
}