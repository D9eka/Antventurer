using Creatures.Enemy;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        private EnemyController _enemy;

        public void OnDoAttack()
        {
            _enemy.OnDoAttack();
        }
    }
}