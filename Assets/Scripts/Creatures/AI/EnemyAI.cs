using System;
using System.Collections;
using Assets.Scripts.Creatures.AI;
using Components.ColliderBased;
using Components.Mana;
using Creatures.Player;
using UnityEngine;

namespace Creatures.Enemy
{
    [RequireComponent(typeof (AINavigation))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private Animator _signAnimator;
        [SerializeField] private LayerCheck _canAttack;

        [SerializeField] private float _maxPatrolDistance = 10f;

        [Header("State: CheckTrace")]
        [SerializeField] private float _traceCheckingDelay = 2f;
        [Header("State: AgroToPlayer")]
        [SerializeField] private float _alarmDelay = 0.5f;
        [Header("State: GoToPlayer")]
        [SerializeField] private float _missPlayerDelay = 5f;
        [SerializeField] private float _missPlayerCoolDown = 0.5f;
        [Header("State: Attack(Player)")]
        [SerializeField] private float _attackCoolDown = 1f;
        [Header("State: FollowThePlayer")]
        [SerializeField] private float _minDistanceToPlayer = 1.5f;
        [SerializeField] private float _maxDistanceToPlayer = 3f;

        private Coroutine _current;
        private float _missPlayerCounter;

        private EnemyController _enemy;
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

        private const string SUSPICION_SIGN_KEY = "suspicion-sign";
        private const string DETECTION_SIGN_KEY = "detection-sign";
        private const string RECRUITMENT_SIGN_KEY = "recruitment-sign";

        private void Start()
        {
            _enemy = GetComponent<EnemyController>();
            _navigation = GetComponent<AINavigation>();

            _enemy.OnRecruited += EnemyAi_OnRecruited;

            StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        private void EnemyAi_OnRecruited(object sender, EventArgs e)
        {
            if (_state == State.Patrolling)
            {
                PlayerController.Instance.OnOrderToAttack += EnemyAi_OnOrderToAttack;
                _signAnimator.SetTrigger(RECRUITMENT_SIGN_KEY);
                StartState(FollowThePlayer(), State.FollowThePlayer);
            }
        }

        private void EnemyAi_OnOrderToAttack(object sender, EventArgs e)
        {
            StartState(AttackEnemy(), State.AttackEnemy);
        }

        public void OnPlayerInVision(GameObject gameObject)
        {
            if (_enemy.IsRecruited || _state == State.GoToPlayer || _state == State.AttackPlayer)
                return;
            _navigation.SetTarget(gameObject);
            _navigation.SetTarget(gameObject);
            _signAnimator.SetTrigger(DETECTION_SIGN_KEY);
            StartState(AgroToPlayer(), State.AgroToPlayer);
        }

        public void OnTraceInVision(GameObject gameObject)
        {
            if(_state == State.Patrolling || (_state == State.GoToTrace && gameObject != _navigation.GetTarget()))
            {
                _navigation.SetTarget(gameObject);
                _signAnimator.SetTrigger(SUSPICION_SIGN_KEY);
                StartState(GoToTrace(), State.GoToTrace);
            }
        }

        public void OnEnemyInVision(GameObject gameObject)
        {
            if (_state == State.AttackEnemy)
            {
                return;
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
            _enemy.CheckTrace();
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
                    StartState(AttackPlayer(), State.AttackPlayer);
                }
                yield return null;
            }
            _enemy.SetDirection(Vector2.zero);
            yield return new WaitForSeconds(_missPlayerCoolDown);
            StartState(_navigation.DoPatrol(), State.Patrolling);
        }

        private IEnumerator AttackPlayer()
        {
            _navigation.followEnabled = false;
            while (_canAttack.IsTouchingLayer)
            {
                _enemy.Attack();
                yield return new WaitForSeconds(_attackCoolDown);
            }
            StartState(GoToPlayer(), State.GoToPlayer);
        }

        private IEnumerator FollowThePlayer()
        { 
            while (enabled)
            {
                if(Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > _maxDistanceToPlayer)
                {
                    _navigation.SetTarget(new Vector2(
                        PlayerController.Instance.transform.position.x + PlayerController.Instance.transform.localScale.x * _minDistanceToPlayer,
                        PlayerController.Instance.transform.position.y), true);
                }
                yield return null;
            }
        }

        private IEnumerator AttackEnemy()
        {
            yield return null;
        }

        private void StartState(IEnumerator coroutine, State state)
        {
            _state = state;
            _navigation.followEnabled = true;
            Debug.Log($"Change state to {state}");
            _enemy.Stop();

            if (_current != null)
                StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }
    }
}