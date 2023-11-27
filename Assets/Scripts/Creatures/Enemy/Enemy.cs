using Components.ColliderBased;
using UnityEngine;

namespace Creatures.Enemy
{
    [RequireComponent(typeof(EnemyAI))]
    public class Enemy : Creature
    {
        [Header("Checkers")]
        [SerializeField] private CheckCircleOverlap _attackRange;

        private const string HIT_KEY = "hit";
        private const string ATTACK_KEY = "attack";

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update() 
        { 
            base.Update();
        }

        public virtual void TakeDamage()
        {
            _animator.SetTrigger(HIT_KEY);
        }

        public virtual void Attack()
        {
            Debug.Log("Attack");
            _animator.SetTrigger(ATTACK_KEY);
            OnDoAttack();
        }

        public void OnDoAttack()
        {
            _attackRange.Check();
        }
    }
}
