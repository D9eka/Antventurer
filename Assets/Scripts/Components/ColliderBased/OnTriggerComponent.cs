using Extensions;
using UnityEngine;

namespace Components.ColliderBased
{
    public class OnTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _onEnterEvent;
        [SerializeField] private EnterEvent _onExitEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer))
                return;

            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag))
                return;

            _onEnterEvent.Invoke(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer))
                return;

            if (!string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag))
                return;

            _onExitEvent.Invoke(other.gameObject);
        }
    }
}