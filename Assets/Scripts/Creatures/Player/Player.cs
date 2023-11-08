using System;
using Components;
using Components.ColliderBased;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Creatures
{
    public class Player : MonoBehaviour
    {
        [Header("Params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;

        [SerializeField] private float _jumpForce;
        [SerializeField] private float _maxJumpTime;

        [Header("Checkers")]
        [SerializeField] private LayerMask GroundLayer;
        [SerializeField] private LayerCheck GroundCheck;
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        [Header("Double Jump")]
        [SerializeField] private bool _allowDoubleJump;
        [SerializeField] private float _doubleJumpForce;

        [Header("Wall Jump")]
        [SerializeField] private float _slideSpeed;
        [SerializeField] private bool _allowWallJump;
        [SerializeField] private Vector2 _jumpWallAngle;
        [SerializeField] private float _jumpWallTime;

        private Rigidbody2D _rigidbody;
        private Vector2 _direction;
        private Animator _animator;
        private bool _isGrounded;
        private bool _isJumping;
        private float _jumpTimeCounter;

        private bool _isOnWall;
        private bool _haveDoubleJump;

        private bool _blockXMovement;

        private float _defaultGravityScale = 1f;
        private float _jumpWallTimeCounter;

        private const string IS_ON_GROUND_KEY = "is-on-ground";
        private const string IS_RUNNING_KEY = "is-running";
        private const string VERTICAL_VELOCITY_KEY = "vertical-velocity";
        private const string IS_ON_WALL_KEY = "is-on-wall";
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            _jumpTimeCounter = _maxJumpTime;
            _defaultGravityScale = _rigidbody.gravityScale;
            _haveDoubleJump = _allowDoubleJump;
            _jumpWallTimeCounter = _jumpWallTime;
        }

        private void FixedUpdate()
        {
            _isGrounded = GroundCheck.IsTouchingLayer;

            var xVelocity = _blockXMovement ? _rigidbody.velocity.x : _direction.x * _speed;
            _rigidbody.velocity = new Vector2(xVelocity, _rigidbody.velocity.y);

            Jump();

            if (_allowWallJump)
            {
                MoveOnWall();
                WallJump();
            }

            _animator.SetFloat(VERTICAL_VELOCITY_KEY, _rigidbody.velocity.y);
            _animator.SetBool(IS_ON_GROUND_KEY, _isGrounded);
            _animator.SetBool(IS_RUNNING_KEY, _direction.x != 0);

            UpdateSpriteDirection(_direction);

            _isOnWall = _wallCheck.IsTouchingLayer;
        }

        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (direction.x > 0)
                transform.localScale = new Vector3(multiplier, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-multiplier, 1, 1);
        }

        public void SetDirection(Vector2 direction)
        {
            _direction = new Vector2(direction.x, direction.y);
        }

        private void Jump()
        {
            bool isJumpKeyPressed = _direction.y > 0;

            if (_isGrounded && isJumpKeyPressed)
            {
                _isJumping = true;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                _jumpTimeCounter -= Time.fixedDeltaTime;
            }

            else if (_isJumping && isJumpKeyPressed && _jumpTimeCounter > 0)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                _jumpTimeCounter -= Time.fixedDeltaTime;
            }

            else if (_isJumping && !isJumpKeyPressed && _rigidbody.velocity.y < 0)
            {
                _jumpTimeCounter = 0;
            }

            else if (_isJumping && isJumpKeyPressed && _haveDoubleJump)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _doubleJumpForce);
                _haveDoubleJump = false;
            }

            else if(_isGrounded)
            {
                _isJumping = false;
                _jumpTimeCounter = _maxJumpTime;
                _haveDoubleJump = _allowDoubleJump;
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
                _isJumping = true;
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
        }

        public void ChangeWallJumpState()
        {
            _allowWallJump = !_allowWallJump;
            Debug.Log($"Wall Jump is {_allowWallJump}");
        }
    }
}