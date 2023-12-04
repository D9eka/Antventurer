using Components.ColliderBased;
using Components.Mana;
using Components.UI;
using Creatures.Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Player
{
    [RequireComponent(typeof(ManaComponent))]
    public class PlayerController : Creature
    {
        [Header("Checkers")]
        [SerializeField] private LayerCheck _groundCheckLeft;
        [SerializeField] private LayerCheck _groundCheckRight;
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private CheckCircleOverlap _enemyCheck;

        [Header("Jump")]
        [SerializeField] private float _maxJumpTime;

        [Header("Double Jump")]
        [SerializeField] private bool _allowDoubleJump;
        [SerializeField] private float _doubleJumpForce;
        [SerializeField] private int _doubleJumpManaExpense;

        [Header("Wall Jump")]
        [SerializeField] private bool _allowWallJump;
        [SerializeField] private float _wallSlidingSpeed;
        [SerializeField] private Vector2 _wallJumpForce;
        [SerializeField] private float _wallJumpDuration;
        [SerializeField] private int _wallJumpManaExpense;

        [Header("Pheromone 2 Skill")]
        [SerializeField] private bool _allowP2Skill;
        [SerializeField] private float _p2SkillManaExprense;

        [Header("Pheromone 3 Skill")]
        [SerializeField] private bool _allowP3Skill;
        [SerializeField] private float _p3SkillManaExprensePerSecond;

        [Header("Flight")]
        [SerializeField] private bool _allowFlight;

        private ManaComponent mana;

        private float jumpTimeCounter;

        private bool haveDoubleJump;
        private bool madeDoubleJump;

        private bool isTouchingWall;
        private bool isWallSliding;
        private bool isWallJumping;
        private float defaultGravityScale;

        private List<EnemyController> recruitedEnemies = new();

        private bool p3SkillEnabled;

        public static PlayerController Instance { get; private set; }

        public EventHandler<OnPlayerGroundedEventArgs> OnPlayerGrounded;
        public class OnPlayerGroundedEventArgs : EventArgs
        {
            public Vector2 position;
        }

        public EventHandler<int> OnChangeProgress;
        public EventHandler OnUnlockDoubleJump;

        public EventHandler<GameObject> OnRecruitEnemy;
        public EventHandler OnOrderToAttack;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;
            mana = GetComponent<ManaComponent>();
            LoadData(PlayerPrefsController.GetPlayerData());

            defaultGravityScale = _rigidbody.gravityScale;
        }

        private void LoadData(PlayerData data)
        {
            if(data.Position != null)
                transform.position = data.Position.Value;

            _allowDoubleJump = data.DoubleJumpState;
            _allowWallJump = data.WallJumpState;
            _allowP2Skill = data.P2State;
            _allowP3Skill = data.P3State;
            _allowFlight = data.FlightState;
        }

        private void Start()
        {
            #region InputReader
            PlayerInputReader inputReader = GetComponent<PlayerInputReader>();
            inputReader.OnPlayerMove += PlayerController_OnPlayerMove;
            inputReader.OnPlayerInteract += PlayerController_OnPlayerInteract;
            inputReader.OnPlayerUseP2Skill += PlayerController_OnPlayerUseP2Skill;
            inputReader.OnPlayerUseP3Skill += PlayerController_OnPlayerUseP3Skill;

            inputReader.OnPlayerChangeDoubleJumpState = PlayerController_OnPlayerChangeDoubleJumpState;
            inputReader.OnPlayerChangeWallJumpState = PlayerController_OnPlayerChangeWallJumpState;
            #endregion
        }

        #region Events
        private void PlayerController_OnPlayerMove(object sender, Vector2 e)
        {
            SetDirection(e);
        }

        private void PlayerController_OnPlayerInteract(object sender, EventArgs e)
        {
            _interactionCheck.Check();
        }

        private void PlayerController_OnPlayerUseP2Skill(object sender, EventArgs e)
        {
            _enemyCheck.Check();
        }

        private void PlayerController_OnPlayerUseP3Skill(object sender, bool e)
        {
            if (!_allowP3Skill)
                return;
            p3SkillEnabled = e;
        }

        public void OrderToAttack()
        {
            OnOrderToAttack?.Invoke(this, EventArgs.Empty);
        }

        public void PlayerController_OnPlayerChangeDoubleJumpState(object sender, EventArgs e)
        {
            _allowDoubleJump = !_allowDoubleJump;
            Debug.Log($"Double Jump is {_allowDoubleJump}");
            if (_allowDoubleJump)
            {
                OnUnlockDoubleJump?.Invoke(this, EventArgs.Empty);
                OnChangeProgress?.Invoke(this, 20);
            }
        }

        public void PlayerController_OnPlayerChangeWallJumpState(object sender, EventArgs e)
        {
            _allowWallJump = !_allowWallJump;
            Debug.Log($"Wall Jump is {_allowWallJump}");

            if (_allowWallJump)
            {
                HUD.Instance.SendMessage("Вы разблокировали прыжок от стены!", 5f);
                OnChangeProgress?.Invoke(this, 40);
            }
        }
        #endregion

        protected override void Update()
        {
            _isGrounded = _groundCheckCenter.IsTouchingLayer;
            bool isFullyGrounded = _groundCheckLeft.IsTouchingLayer && _groundCheckCenter.IsTouchingLayer && _groundCheckRight.IsTouchingLayer;
            if (isFullyGrounded && !p3SkillEnabled)
                OnPlayerGrounded?.Invoke(this, new OnPlayerGroundedEventArgs
                {
                    position = transform.position
                });

            if (p3SkillEnabled)
                mana.ModifyMana(_p3SkillManaExprensePerSecond * Time.deltaTime);

            if (!isWallJumping)
            {
                Move();
                Jump();
            }

            if (_allowWallJump)
            {
                isTouchingWall = _wallCheck.IsTouchingLayer;
                WallSlide();
                WallJump();
            }

            UpdateAnimations();
            UpdateSpriteDirection();
        }

        protected override void Jump()
        {
            bool isJumpKeyPressed = _direction.y > 0;

            if (_isGrounded || isTouchingWall)
            {
                jumpTimeCounter = _maxJumpTime;
                madeDoubleJump = haveDoubleJump = false;

                if (isJumpKeyPressed)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                }
                return;
            }
            if (!_isGrounded && !isJumpKeyPressed)
            {
                if (!_allowFlight)
                    jumpTimeCounter = 0;
                haveDoubleJump = _allowDoubleJump && !madeDoubleJump;
                return;
            }
            if (!_isGrounded && isJumpKeyPressed && jumpTimeCounter > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                if(!_allowFlight)
                    jumpTimeCounter -= Time.deltaTime;
                return;
            }
            if (!_isGrounded && isJumpKeyPressed && haveDoubleJump)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _doubleJumpForce);
                haveDoubleJump = false;
                madeDoubleJump = true;
                mana.ModifyMana(_doubleJumpManaExpense);
                Debug.Log("Double");
                return;
            }
        }

        #region Wall Jump
        private void WallSlide()
        {
            if (isTouchingWall && !_isGrounded && _direction.x != 0)
            {
                isWallSliding = true;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, -_wallSlidingSpeed);
            }
            else
            {
                isWallSliding = false;
            }
        }

        private void WallJump()
        {
            if (isWallSliding)
            {
                isWallJumping = false;
                CancelInvoke(nameof(StopWallJumping));
            }

            bool isJumpKeyPressed = _direction.y > 0;
            if (isJumpKeyPressed && isWallSliding) 
            {
                isWallJumping = true;
                _rigidbody.velocity = new Vector2(_wallJumpForce.x * transform.localScale.x, _wallJumpForce.y);

                jumpTimeCounter = 0;
                Invoke(nameof(StopWallJumping), _wallJumpDuration);
            }
        }    

        private void StopWallJumping()
        {
            isWallJumping = false;
            mana.ModifyMana(_wallJumpManaExpense);
        }
        #endregion

        public void Recruit(GameObject go)
        {
            if (!go.transform.parent.TryGetComponent(out EnemyController enemy))
                return;
            if (recruitedEnemies.Contains(enemy))
                return;
            recruitedEnemies.Add(enemy);
            enemy.Recruit();
            mana.ModifyMana(_p2SkillManaExprense);
        }

        public PlayerData SaveData()
        {
            var manaData = mana.SaveData();
            return new PlayerData(transform.position, manaData.mana, manaData.maxMana, 
                                  _allowDoubleJump, _allowWallJump, 
                                  _allowP2Skill, _allowP3Skill, _allowFlight);
        }    
    }

    public struct PlayerData
    {
        public Vector2? Position { get; private set; }

        public float Mana { get; private set; }
        public float MaxMana { get; private set; }

        public bool DoubleJumpState { get; private set; }
        public bool WallJumpState { get; private set; }
        public bool P2State { get; private set; }
        public bool P3State { get; private set; }
        public bool FlightState { get; private set; }

        private const float DEFAULT_MANA = 100f;
        private const float DEFAULT_MAX_MANA = 100f;

        public PlayerData(Vector2? position, float mana = DEFAULT_MANA, float maxMana = DEFAULT_MAX_MANA, 
                          bool doubleJumpState = false, bool wallJumpState = false,
                          bool p2State = false, bool p3State = false, bool flightState = false)
        {
            Position = position;

            Mana = mana;
            MaxMana = maxMana;

            DoubleJumpState = doubleJumpState;
            WallJumpState = wallJumpState;
            P2State = p2State;
            P3State = p3State;
            FlightState = flightState;
        }
    }
}