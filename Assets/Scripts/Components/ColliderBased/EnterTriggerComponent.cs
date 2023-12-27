using Extensions;
using UnityEngine;

namespace Components.ColliderBased
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer))
                return;

            if(_tag != "" && !string.IsNullOrEmpty(_tag) && !other.gameObject.CompareTag(_tag))
                return;

            _action?.Invoke(other.gameObject);
        }
    }
}
