using Creatures;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Creatures.Enemy
{
    public class Patrol : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _threshold = 1f;

        private Enemy _enemy;
        private int _destinationPointIndex;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        public IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (IsOnPoint())
                {
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }

                var direction = _points[_destinationPointIndex].position - transform.position;
                direction.y = 0;
                _enemy.SetDirection(direction.normalized);
                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            return (_points[_destinationPointIndex].position - transform.position).magnitude < _threshold;
        }

        public float GetDistanceToNearestPoint()
        {
            return _points.Select(transform => Vector2.Distance(transform.position, _enemy.transform.position))
                          .Min();
        }
    }
}