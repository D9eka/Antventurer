using System;
using Components.ColliderBased;
using UnityEngine;
using static Creatures.Player.Player;

public class Enemy : MonoBehaviour
{
    public float speedEnemy;
    public int positionOfPatrol;

    public Transform point;
    public Transform point2;
    private bool movingRight;
    Transform player;
    public float stoppingDistance;

    bool chill = false;
    bool angry = false;
    bool goBack = false;
    bool isReplay = false;

    public float time;
    public Animator _animator;
    private Rigidbody2D rb;

    public EventHandler<OnPlayerGroundedEventArgs> OnPlayerGrounded;
    [SerializeField] private LayerCheck GroundCheckLeft;
    [SerializeField] private LayerCheck GroundCheckCenter;
    [SerializeField] private LayerCheck GroundCheckRight;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, player.position) < 10 && Input.GetKey(KeyCode.G))
        {
            isReplay = true;
        }

        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && angry == false)
        {
            chill = true;
            angry = false;
            goBack = false;
        }

        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            chill = false;
            angry = true;
            goBack = false;
        }

        if (Vector2.Distance(transform.position, player.position) > stoppingDistance)
        {
            angry = false;
            goBack = true;
        }

        if (isReplay == true)
        {
            Replay();
        }
            else if (chill == true)
        {
            Chill();
        }
        else if (angry == true)
        {
            Angry();
        }
        else if (goBack == true)
        {
            GoBack();
        }

        if (movingRight && transform.localScale.x < 0)
        {
            Flip();
        }
        else if (!movingRight && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    void Chill()
    {
        speedEnemy = 1f;
        if (transform.position.x > point2.position.x)
        {
            movingRight = false;
        }
        else if (transform.position.x < point.position.x)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            rb.velocity = new Vector2(speedEnemy, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speedEnemy, rb.velocity.y);
        }

        //if (movingRight)
        //{
        //    transform.position = new Vector2(transform.position.x + speedEnemy * Time.deltaTime, transform.position.y);
        //}
        //else
        //{
        //    transform.position = new Vector2(transform.position.x - speedEnemy * Time.deltaTime, transform.position.y);
        //}
    }

    void Angry()
    {
        speedEnemy = 4f;
        rb.velocity = new Vector2(speedEnemy * Mathf.Sign(player.position.x - transform.position.x), rb.velocity.y);
        //transform.position = Vector2.MoveTowards(transform.position, player.position, speedEnemy * Time.deltaTime);
        
    }

    void GoBack()
    {
        //rb.velocity = new Vector2(-speedEnemy, rb.velocity.y);
        transform.position = Vector2.MoveTowards(transform.position, point.position, speedEnemy * Time.deltaTime);
    }

    void Flip()
    {
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void Replay()
    {
        float moveDirection = Mathf.Sign(player.position.x - transform.position.x);

        // Устанавливаем новую скорость объекта с плавным следованием
        rb.velocity = new Vector2(Mathf.MoveTowards(rb.velocity.x, speedEnemy * moveDirection, 5 * Time.deltaTime), rb.velocity.y);

        // Проверка, если персонаж прыгнул, применяем прыжок к объекту
        if (IsPlayerJumping() && IsGrounded())
        {
            Jump();
        }
    }

    void Jump()
    {
        var jumpForce = 5.1f;
        var maxTimeJump = 0.5f;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce * maxTimeJump);
    }

    bool IsPlayerJumping()
    {
        // Ваш код для проверки, прыгнул ли игрок (может потребоваться реализация)
        // Возвращайте true, если игрок прыгнул, иначе false
        return Input.GetKey(KeyCode.W); // Пример, используйте ваш метод для определения прыжка игрока
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);
        return hit.collider != null;
    }
}