using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI.Dialogs
{
    public class DialogContent : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public TextMeshProUGUI Text => _text;
    }
}