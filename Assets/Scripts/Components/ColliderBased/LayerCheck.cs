using UnityEngine;

namespace Components.ColliderBased
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        private bool _isTouchingLayer;
        private Collider2D _collider;
        private GameObject _other;

        public bool IsTouchingLayer => _isTouchingLayer;
        public GameObject Other => _other;


        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
            _other = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
            _other = other.gameObject;
        }
    }
}