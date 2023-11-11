using UnityEngine;

namespace XYZ.Components.Interactables
{
    public class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            if (go.TryGetComponent<InteractableComponent>(out var interactable))
                interactable.Interact();
        }
    }
}