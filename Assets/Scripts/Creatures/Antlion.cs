using Creatures;
using Creatures.Enemy;
using Creatures.Player;
using DragonBones;
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
        [Space]
        [SerializeField] private UnityArmatureComponent _lionArmature;

        private const string ATTACK_KEY = "Antlion";
        private const string CREATURE_DEATH_KEY = "death-by-antlion";

        public void OnAttack(GameObject creature)
        {
            if (!creature.TryGetComponent(out Animator creatureAnimator))
                return;

            creature.GetComponent<CapsuleCollider2D>().enabled = false;
            creature.transform.parent.GetComponent<Rigidbody2D>().simulated = false;
            creature.transform.parent.parent = transform;
            MoveCreature(creature.transform);

            _groundAnimator.SetTrigger(ATTACK_KEY);
            _lionArmature.animation.Play(ATTACK_KEY, 1);
            creatureAnimator.SetTrigger(CREATURE_DEATH_KEY);
        }

        private void MoveCreature(UnityEngine.Transform creature)
        {
            creature.transform.parent.localPosition = _creaturePos;
        }
    }
}