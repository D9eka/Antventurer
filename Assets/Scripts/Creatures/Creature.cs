using Components.ColliderBased;
using UnityEngine;

namespace Creatures
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Creature : MonoBehaviour
    {
        [SerializeField] protected GameObject _visual;

        [SerializeField] private bool _invertScale;
        [SerializeField] protected float _speed;

        [SerializeField] protected float _jumpForce;

        [Header("Checkers")]
        [SerializeField] protected LayerCheck _groundCheckCenter;

        protected Rigidbody2D _rigidbody;
        protected Animator _animator;

        protected Vector2 _direction;
        protected bool _isGrounded;

        protected const string IS_ON_GROUND_KEY = "is-on-ground";
        protected const string IS_RUNNING_KEY = "is-running";
        protected const string VERTICAL_VELOCITY_KEY = "vertical-velocity";

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = _visual.GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            _isGrounded = _groundCheckCenter.IsTouchingLayer;

            Move();
            Jump();

            UpdateAnimations();
            UpdateSpriteDirection();
        }

        protected virtual void Move()
        {
            var xVelocity = _direction.x * _speed;
            _rigidbody.velocity = new Vector2(xVelocity, _rigidbody.velocity.y);
        }

        protected virtual void Jump()
        {
            bool isJumpKeyPressed = _direction.y > 0;

            if (_isGrounded && isJumpKeyPressed)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
                return;
            }
        }

        protected virtual void UpdateAnimations()
        {
            _animator.SetFloat(VERTICAL_VELOCITY_KEY, _rigidbody.velocity.y);
            _animator.SetBool(IS_ON_GROUND_KEY, _isGrounded);
            _animator.SetBool(IS_RUNNING_KEY, _direction.x != 0);
        }

        public void UpdateSpriteDirection()
        {
            var multiplier = _invertScale ? -1 : 1;
            if (_direction.x > 0)
                transform.localScale = new Vector3(multiplier, 1, 1);
            else if (_direction.x < 0)
                transform.localScale = new Vector3(-multiplier, 1, 1);
        }

        public virtual void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
    }
}