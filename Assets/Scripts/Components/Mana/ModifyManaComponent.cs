using UnityEngine;

namespace Components.Mana
{
    public class ModifyManaComponent : MonoBehaviour
    {
        [SerializeField] private int _manaDelta;

        public void ModifyMana(GameObject target)
        {
            if(target.TryGetComponent<ManaComponent>(out var manaComponent))
                manaComponent.ModifyMana(_manaDelta);
        }
    }
}