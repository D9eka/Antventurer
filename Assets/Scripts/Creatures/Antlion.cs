using Creatures;
using Creatures.Enemy;
using Creatures.Player;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class Antlion : MonoBehaviour
    {
        [SerializeField] private Vector2 _creaturePos = Vector2.zero;
        [Space]
        [SerializeField] private Animator _groundAnimator;
        [SerializeField] private Animator _lionAnimator;

        private const string ATTACK_KEY = "attack";
        private const string CREATURE_DEATH_KEY = "death-by-antlion";

        public void OnAttack(GameObject creature)
        {
            if (!creature.TryGetComponent(out Animator creatureAnimator))
                return;

            creature.transform.parent = transform;
            _lionAnimator.transform.localScale = creature.transform.localScale;
            MoveCreature(creature.transform);

            _groundAnimator.SetTrigger(ATTACK_KEY);
            _lionAnimator.SetTrigger(ATTACK_KEY);
            creatureAnimator.SetTrigger(CREATURE_DEATH_KEY);
        }

        private void MoveCreature(Transform creature)
        {
            creature.position = _creaturePos;
        }
    }
}