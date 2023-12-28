using Creatures.Enemy;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        private EnemyController _enemy;
        private EnemyAI _enemyAI;

        private void Start()
        {
            _enemy = GetComponent<EnemyController>();
        }

        public void OnDoAttack()
        {
            _enemy.OnDoAttack();
        }
    }
}