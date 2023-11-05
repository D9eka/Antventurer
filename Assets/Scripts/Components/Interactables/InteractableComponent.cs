using UnityEngine;
using UnityEngine.Events;

namespace XYZ.Components.Interactables
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;

        public void Interact()
        {
            _action?.Invoke();
        }
    }
}