using Assets.Scripts.Creatures;
using Assets.Scripts.Creatures.Player;
using Components.ColliderBased;
using Components.Mana;
using Components.UI;
using System;
using UnityEngine;

namespace Creatures.Player
{
    [RequireComponent(typeof(ManaComponent))]
    public class Player : Creature
    {
        [Header("Checkers")]
        [SerializeField] private LayerCheck _groundCheckLeft;
        [SerializeField] private LayerCheck _groundCheckRight;
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Header("Jump")]
        [SerializeField] private float _maxJumpTime;

        [Header("Double Jump")]
        [SerializeField] private bool _allowDoubleJump;
        [SerializeField] private float _doubleJumpForce;
        [SerializeField] private int _doubleJumpManaExpense;

        [Header("Wall Jump")]
        [SerializeField] private float _slideSpeed;
        [SerializeField] private bool _allowWallJump;
        [SerializeField] private Vector2 _jumpWallAngle;
        [SerializeField] private float _jumpWallTime;
        [SerializeField] private int _wallJumpManaExpense;

        private ManaComponent _mana;

        private float _jumpTimeCounter;

        private bool _isOnWall;
        private bool _haveDoubleJump;
        private bool _madeDoubleJump;

        private bool _blockXMovement;

        private float _defaultGravityScale;
        private float _jumpWallTimeCounter;

        public static Player Instance { get; private set; }

        public bool AllowDoubleJump => _allowDoubleJump;
        public bool AllowWallJump => _allowWallJump;

        public EventHandler<OnPlayerGroundedEventArgs> OnPlayerGrounded;
        public class OnPlayerGroundedEventArgs : EventArgs
        {
            public Vector2 position;
        }

        public EventHandler<int> OnChangeProgress;
        public EventHandler OnUnlockDoubleJump;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null)
            {
                Destroy(Instance);
            }
            Instance = this;

            if(PlayerPrefsController.TryGetPlayerPosition(out var position))
            {
                transform.position = position;
            }

            _allowDoubleJump = PlayerPrefsController.GetPlayerDoubleJumpState();
            _allowWallJump = PlayerPrefsController.GetPlayerWallJumpState();
            _mana = GetComponent<ManaComponent>();

            _defaultGravityScale = _rigidbody.gravityScale;
            _jumpWallTimeCounter = _jumpWallTime;
        }

        protected override void Update()
        {
            bool isFullyGrounded = _groundCheckLeft.IsTouchingLayer && _groundCheckCenter.IsTouchingLayer && _groundCheckRight.IsTouchingLayer;
            if (isFullyGrounded)
                OnPlayerGrounded?.Invoke(this, new OnPlayerGroundedEventArgs
                {
                    position = transform.position
                });

            _isOnWall = _wallCheck.IsTouchingLayer;
            if (_allowWallJump)
            {
                MoveOnWall();
                WallJump();
            }

            base.Update();
        }

        protected override void Jump()
        {
            bool isJumpKeyPressed = _direction.y > 0;

            if (_isGrounded)
            {
                _jumpTimeCounter = _maxJumpTime;
                _madeDoubleJump = _haveDoubleJump = false;
                if(isJumpKeyPressed)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                }
                return;
            }
            if (!_isGrounded && !isJumpKeyPressed)
            {
                _jumpTimeCounter = 0;
                _haveDoubleJump = _allowDoubleJump && !_madeDoubleJump;
                return;
            }
            if (!_isGrounded && isJumpKeyPressed && _jumpTimeCounter > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                _jumpTimeCounter -= Time.deltaTime;
                return;
            }
            if (!_isGrounded && isJumpKeyPressed && _haveDoubleJump)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _doubleJumpForce);
                _haveDoubleJump = false;
                _madeDoubleJump = true;
                _mana.ModifyMana(_doubleJumpManaExpense);
                Debug.Log("Double");
                return;
            }
        }

        private void MoveOnWall()
        {
            if (_isOnWall && !_isGrounded)
            {
                if (!_blockXMovement && _direction.y == 0)
                {
                    _rigidbody.gravityScale = 0f;
                    _rigidbody.velocity = new Vector2(0, _slideSpeed);
                }
            }
            else if (!_isOnWall && !_isGrounded)
            {
                _rigidbody.gravityScale = _defaultGravityScale;
            }
        }

        private void WallJump()
        {
            if (_isOnWall && !_isGrounded && _direction.y > 0)
            {
                _blockXMovement = true;
                _direction.x = 0;

                _rigidbody.gravityScale = _defaultGravityScale;
                _rigidbody.velocity = new Vector2(0, 0);
                _rigidbody.velocity = new Vector2(transform.localScale.x * _jumpWallAngle.x, _jumpWallAngle.y);
                //_mana.ModifyMana(_wallJumpManaExpense);
            }
            if (_blockXMovement && (_jumpWallTimeCounter -= Time.fixedDeltaTime) <= 0)
            {
                if (_isOnWall || _isGrounded || _direction.x != 0)
                {
                    _blockXMovement = false;
                    _jumpWallTimeCounter = _jumpWallTime;
                }
            }
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void ChangeDoubleJumpState()
        {
            _allowDoubleJump = !_allowDoubleJump;
            Debug.Log($"Double Jump is {_allowDoubleJump}");
            if(_allowDoubleJump) 
            {
                OnUnlockDoubleJump?.Invoke(this, EventArgs.Empty);
                OnChangeProgress?.Invoke(this, 20);
            }
        }

        public void ChangeWallJumpState()
        {
            _allowWallJump = !_allowWallJump;
            Debug.Log($"Wall Jump is {_allowWallJump}");

            if (_allowWallJump)
            {
                HUD.Instance.SendMessage("Вы разблокировали прыжок от стены!", 5f);
                OnChangeProgress?.Invoke(this, 40);
            }
        }
    }
}