using Creatures;
using Creatures.Enemy;
using Pathfinding;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Creatures.AI
{
    [RequireComponent(typeof(Seeker))]
    public class AINavigation : MonoBehaviour
    {
        [Header("Pathfinder")]
        [SerializeField] private GameObject _targetPrefab;
        [SerializeField] private GameObject _target;
        [SerializeField] private bool _needJump;
        [SerializeField] private float _pathUpdateSeconds = 0.5f;
        [SerializeField] private float _threshold = 0.25f;

        [Header("Patrolling")]
        [SerializeField] private GameObject[] _points;

        private Seeker _seeker;
        private Path _path;
        private int _currentWaypoint = 0;

        private bool _isTargetSpawned;

        private Creature _creature;

        private int _patrollingPointIndex;

        [HideInInspector] public bool followEnabled = true;

        public GameObject GetTarget()
        { 
            return _target; 
        }

        public void SetTarget(Vector2 target, bool needJump = false)
        {
            GameObject gameObject = Instantiate(_targetPrefab, target, Quaternion.identity);
            SetTarget(gameObject, needJump);
            _isTargetSpawned = true;
        }

        public void SetTarget(GameObject target, bool needJump = false)
        {
            if(_isTargetSpawned) 
            {
                Destroy(_target);
                _isTargetSpawned = false;
            }
            _target = target;
            _needJump = needJump;
        }

        private void Start()
        {
            _seeker = GetComponent<Seeker>();
            _creature = GetComponent<Creature>();

            InvokeRepeating(nameof(UpdatePath), 0, _pathUpdateSeconds);
        }

        private void Update()
        {
            if (!followEnabled || _path == null || _currentWaypoint >= _path.vectorPath.Count)
            {
                _creature.SetDirection(Vector2.zero);
                return;
            }

            Vector2 direction = (_path.vectorPath[_currentWaypoint] - transform.position).normalized;
            if(_needJump)
                _creature.SetDirection(direction);
            else
                _creature.SetDirection(new Vector2(direction.x, 0f));

            if (Vector2.Distance(transform.position, _path.vectorPath[_currentWaypoint]) < _threshold)
            {
                _currentWaypoint++;
            }
        }

        private void UpdatePath()
        {
            if (TargetInDistance() && _seeker.IsDone())
            {
                _seeker.StartPath(transform.position, _target.transform.position, OnPathComplete);
            }
        }

        private bool TargetInDistance()
        {
            return _target != null;
        }

        private void OnPathComplete(Path p)
        {
            if (!p.error)
            {
                _path = p;
                _currentWaypoint = 0;
            }
        }

        public IEnumerator DoPatrol()
        {
            _target = _points[_patrollingPointIndex];
            while (enabled)
            {
                if (Vector2.Distance(transform.position, _points[_patrollingPointIndex].transform.position) < _threshold)
                {
                    _patrollingPointIndex = (int)Mathf.Repeat(_patrollingPointIndex + 1, _points.Length);
                    _target = _points[_patrollingPointIndex];
                }
                yield return null;
            }
        }

        public float GetDistanceToNearestPoint()
        {
            return _points.Select(gameObject => Vector2.Distance(gameObject.transform.position, _creature.transform.position))
                          .Min();
        }
    }
}