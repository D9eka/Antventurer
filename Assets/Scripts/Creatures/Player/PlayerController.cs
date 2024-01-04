using Components.ColliderBased;
using Components.Mana;
using Components.UI;
using Creatures.Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        private const string DOUBLE_JUMP_KEY = "double-jump";
        private const string IS_ON_WALL_KEY = "is-on-wall";

        public bool Active { get; private set; }

        public static PlayerController Instance { get; private set; }

        public EventHandler<OnPlayerGroundedEventArgs> OnPlayerGrounded;
        public class OnPlayerGroundedEventArgs : EventArgs
        {
            public Vector2 position;
        }

        public EventHandler<int> OnChangeProgress;
        public EventHandler OnUnlockDoubleJump;
        public EventHandler OnUnlockWallJump;
        public EventHandler OnUnlockP2;
        public EventHandler OnUnlockP3;

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
            if(PlayerPrefsController.TryGetPlayerData(out PlayerData data))
                LoadData(data);

            defaultGravityScale = _rigidbody.gravityScale;
            Active = true;
        }

        private void LoadData(PlayerData data)
        {
            transform.position = data.Position.Value;
            transform.localScale = new Vector2(data.Scale, transform.localScale.y);
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
            _isGrounded = _groundCheckLeft.IsTouchingLayer || _groundCheckCenter.IsTouchingLayer || _groundCheckRight.IsTouchingLayer;
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

        public override void UpdateSpriteDirection()
        {
            base.UpdateSpriteDirection();
            _animator.SetBool(IS_ON_WALL_KEY, isTouchingWall);
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
                _animator.SetTrigger(DOUBLE_JUMP_KEY);
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

        #region Unlock Skills
        public void UnlockDoubleJump()
        {
            _allowDoubleJump = true;
            SaveData();
            OnUnlockDoubleJump?.Invoke(this, EventArgs.Empty);
        }
        public void UnlockWallJump()
        {
            _allowWallJump = true;
            SaveData();
            OnUnlockWallJump?.Invoke(this, EventArgs.Empty);
        }
        public void UnlockP2()
        {
            _allowP2Skill = true;
            SaveData();
            OnUnlockP2?.Invoke(this, EventArgs.Empty);
        }
        public void UnlockP3()
        {
            _allowP3Skill = true;
            SaveData();
            OnUnlockP3?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        public void Deactivate()
        {
            Active = false;
        }

        public PlayerData SaveData()
        {
            var manaData = mana.SaveData();
            return new PlayerData(SceneManager.GetActiveScene().name, transform.position, transform.localScale.x,
                                  manaData.mana, manaData.maxMana, 
                                  _allowDoubleJump, _allowWallJump, 
                                  _allowP2Skill, _allowP3Skill, _allowFlight);
        } 
    }

    [System.Serializable]
    public struct PlayerData
    {
        public string Location { get; private set; }
        public Vector2? Position { get; private set; }
        public float Scale { get; private set; }

        public float Mana { get; private set; }
        public float MaxMana { get; private set; }

        public bool DoubleJumpState { get; private set; }
        public bool WallJumpState { get; private set; }
        public bool P2State { get; private set; }
        public bool P3State { get; private set; }
        public bool FlightState { get; private set; }

        private const float DEFAULT_MANA = 100f;
        private const float DEFAULT_MAX_MANA = 100f;

        public PlayerData(string location, Vector2 position, float scale = 1f, float mana = DEFAULT_MANA, float maxMana = DEFAULT_MAX_MANA, 
                          bool doubleJumpState = true, bool wallJumpState = true,
                          bool p2State = true, bool p3State = true, bool flightState = false)
        {
            Location = location;
            Position = position;
            Scale = scale;

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