using Components.ColliderBased;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Creatures.Enemy
{
    public class EnemyController : Creature
    {
        [Header("Checkers")]
        [SerializeField] private CheckCircleOverlap _attackRange;

        private const string CHECK_TRACE_KEY = "check-trace";
        private const string HIT_KEY = "hit";
        private const string ATTACK_KEY = "attack";

        public EventHandler OnRecruited;
        public EventHandler OnDie;

        public bool IsRecruited { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            Enemies.AddEnemy(transform);
            if (TryGetComponent(out EnemyAI ai))
                ai.OnChangeSpeed += EnemyAI_OnChangeSpeed;
        }

        private void EnemyAI_OnChangeSpeed(object sender, float e)
        {
            _speed = e;
        }

        protected override void Update() 
        { 
            base.Update();
        }

        public void Recruit()
        {
            OnRecruited?.Invoke(this, EventArgs.Empty);
            IsRecruited = true;
        }

        public void Stop()
        {
            _direction = Vector2.zero;
            _rigidbody.velocity = Vector2.zero;
        }

        public void CheckTrace()
        {
            _animator.SetTrigger(CHECK_TRACE_KEY);
        }

        public void TakeDamage()
        {
            _animator.SetTrigger(HIT_KEY);
        }

        public void Attack()
        {
            _animator.SetTrigger(ATTACK_KEY);
            OnDoAttack();
        }

        public void OnDoAttack()
        {
            _attackRange.Check();
        }

        public override void Die()
        {
            OnDie?.Invoke(this, EventArgs.Empty);
            _rigidbody.velocity = Vector2.zero;
            base.Die();
        }
    }
}
