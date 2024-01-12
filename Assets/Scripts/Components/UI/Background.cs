using Creatures.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public class Background : MonoBehaviour
    {
        [Header("Backgrounds")]
        [SerializeField] private Sprite _start;
        [SerializeField] private Sprite _cemetery;
        [SerializeField] private Sprite _hibernationChamber;
        [SerializeField] private Sprite _cowshed;
        [SerializeField] private Sprite _exit;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            _image.color = Color.white;
            _image.sprite = PlayerPrefsController.GetLocation() switch
            {
                "Start" => _start,
                "Cemetery" => _cemetery,
                "HibernationÑhamber" => _hibernationChamber,
                "Cowshed" => _cowshed,
                "Exit" => _exit,
                _ => _start,
            };
        }
    }
}
