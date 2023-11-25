using Components.ColliderBased;
using UnityEngine;

namespace Assets.Scripts.Creatures
{
    public class Creature : MonoBehaviour
    {
        [SerializeField] protected GameObject Visual;

        [SerializeField] private bool _invertScale;
        [SerializeField] protected float Speed;

        [SerializeField] protected float JumpForce;
        [SerializeField] protected float MaxJumpTime;

        [Header("Checkers")]
        [SerializeField] protected LayerCheck GroundCheckCenter;

        protected Rigidbody2D Rigidbody;
        private Animator Animator;

        protected Vector2 Direction;
        protected bool IsGrounded;
        protected float JumpTimeCounter;

        private const string IS_ON_GROUND_KEY = "is-on-ground";
        private const string IS_RUNNING_KEY = "is-running";
        private const string VERTICAL_VELOCITY_KEY = "vertical-velocity";

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = Visual.GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            IsGrounded = GroundCheckCenter.IsTouchingLayer;

            var xVelocity = Direction.x * Speed;
            Rigidbody.velocity = new Vector2(xVelocity, Rigidbody.velocity.y);

            Jump();

            Animator.SetFloat(VERTICAL_VELOCITY_KEY, Rigidbody.velocity.y);
            Animator.SetBool(IS_ON_GROUND_KEY, IsGrounded);
            Animator.SetBool(IS_RUNNING_KEY, Direction.x != 0);

            UpdateSpriteDirection(Direction);
        }

        protected virtual void Jump()
        {
            bool isJumpKeyPressed = Direction.y > 0;

            if (IsGrounded)
            {
                JumpTimeCounter = MaxJumpTime;
                if (isJumpKeyPressed)
                {
                    Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, JumpForce);
                }
                return;
            }
            if (!IsGrounded && !isJumpKeyPressed)
            {
                JumpTimeCounter = 0;
                return;
            }
            if (!IsGrounded && isJumpKeyPressed && JumpTimeCounter > 0)
            {
                Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, JumpForce);
                JumpTimeCounter -= Time.deltaTime;
                return;
            }
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
            Direction = new Vector2(direction.x, direction.y);
        }
    }
}