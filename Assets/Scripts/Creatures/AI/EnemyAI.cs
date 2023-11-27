using System.Collections;
using Assets.Scripts.Creatures.AI;
using Components.ColliderBased;
using UnityEngine;

namespace Creatures.Enemy
{
    [RequireComponent(typeof (AINavigation))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _maxPatrolDistance = 10f;

        [SerializeField] private float _traceCheckingDelay = 2f;
        [SerializeField] private float _alarmDelay = 0.5f;
        [SerializeField] private float _missPlayerDelay = 5f;
        [SerializeField] private float _attackCoolDown = 1f;
        [SerializeField] private float _missPlayerCoolDown = 0.5f;

        private Coroutine _current;
        private float _missPlayerCounter;

        private Enemy _enemy;
        private AINavigation _navigation;
        private enum State
        {
            Patrolling,
            GoToTrace,
            CheckTrace,
            FollowTheTrace,
            AgroToPlayer,
            GoToPlayer,
            AttackPlayer,
            FollowThePlayer,
            AttackEnemy,
            Attacked
        }

        private State _state;

        private bool isRecruited;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
            _navigation = GetComponent<AINavigation>();
        }

        private void Start()
        {
            StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        public void OnPlayerInVision(GameObject gameObject)
        {
            if (isRecruited || _state == State.GoToPlayer || _state == State.AttackPlayer)
                return;
            _navigation.SetTarget(gameObject);
            StartState(AgroToPlayer(), State.AgroToPlayer);
        }

        public void OnTraceInVision(GameObject gameObject)
        {
            if(_state == State.Patrolling || (_state == State.GoToTrace && gameObject != _navigation.GetTarget()))
            {
                _navigation.SetTarget(gameObject);
                StartState(GoToTrace(), State.GoToTrace);
            }
        }

        private IEnumerator GoToTrace()
        { 
            while (_navigation.GetTarget() != null)
            {
                if (TraceController.Instance.IsOnTrace(transform.position))
                {
                    StartState(CheckTrace(), State.CheckTrace);
                }
                yield return null;
            }
            StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        private IEnumerator CheckTrace()
        {
            _navigation.followEnabled = false;
            yield return new WaitForSeconds(_traceCheckingDelay);
            if (TraceController.Instance.IsTraceActual(_enemy.transform.position))
                StartState(FollowTheTrace(), State.FollowTheTrace);
            else
                StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        private IEnumerator FollowTheTrace()
        {
            _navigation.followEnabled = false;
            while (TraceController.Instance.IsOnTrace(_enemy.transform.position) && 
                  TraceController.Instance.IsTraceActual(_enemy.transform.position) &&
                  _navigation.GetDistanceToNearestPoint() < _maxPatrolDistance)
            {
                _enemy.SetDirection(TraceController.Instance.GetTraceDirection(_enemy.transform.position));
                yield return null;
            }

            StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        private IEnumerator AgroToPlayer()
        {
            _navigation.followEnabled = false;
            yield return new WaitForSeconds(_alarmDelay);
            if (_vision.IsTouchingLayer)
                StartState(GoToPlayer(), State.GoToPlayer);
            else
                StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        private IEnumerator GoToPlayer()
        {
            _missPlayerCounter = _missPlayerDelay;
            while (_vision.IsTouchingLayer || _missPlayerCounter > 0)
            {
                if (!_vision.IsTouchingLayer)
                    _missPlayerCounter -= Time.deltaTime;
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack(), State.AttackPlayer);
                }
                yield return null;
            }
            _enemy.SetDirection(Vector2.zero);
            yield return new WaitForSeconds(_missPlayerCoolDown);
            StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        private IEnumerator Attack()
        {
            _navigation.followEnabled = false;
            while (_canAttack.IsTouchingLayer)
            {
                _enemy.Attack();
                yield return new WaitForSeconds(_attackCoolDown);
            }
            StartState(GoToPlayer(), State.GoToPlayer);
        }

        private void StartState(IEnumerator coroutine, State state)
        {
            _state = state;
            _navigation.followEnabled = true;
            Debug.Log($"Change state to {state}");
            _enemy.SetDirection(Vector2.zero);

            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }
    }
}