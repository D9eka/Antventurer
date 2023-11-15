using Components.ColliderBased;
using UnityEngine;

public class CreateSpriteOnCollision : MonoBehaviour
{
    public LayerMask GroundLayer;
    public Sprite spriteToCreate;
    public LayerCheck GroundCheck;
    public GameObject footStep;
    public string sortingLayerName;
    public int sortingOrder = 0;
    public float footStepTimer = 0.1f;

    private float timer;
    private bool _isGrounded;
    

    private void Start()
    {
        CreateSprite(GetGroundPosition(GroundLayer));
    }

    private void Update()
    {
        _isGrounded = GroundCheck.IsTouchingLayer;

        if (_isGrounded)
        {
            Vector2 groundPosition = GetGroundPosition(GroundLayer);
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                timer += Time.deltaTime;
                if (timer > footStepTimer)
                {
                    CreateSprite(groundPosition);
                    timer = 0;
                }
            } else
            {
                CreateSprite(groundPosition);
                timer += Time.deltaTime;
                if (timer > 5)
                {
                    CreateSprite(groundPosition);
                    timer = 0;
                }
            }
        } else
        {
            timer = 0;
        }
    }

    private void CreateSprite(Vector2 groundPosition)
    {
        if (spriteToCreate != null)
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.y = Mathf.RoundToInt(groundPosition.y);
            GameObject newSprite = GameObject.Instantiate(footStep, spawnPosition, Quaternion.identity);
            newSprite.SetActive(true);  
        }
    }

    private Vector2 GetGroundPosition(LayerMask groundLayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, groundLayer);

        if (hit.collider != null)
        {
            return hit.point;
        }

        return Vector2.zero;
    }
}