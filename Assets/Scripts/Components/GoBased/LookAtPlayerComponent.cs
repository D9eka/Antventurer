using Creatures.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayerController : MonoBehaviour
{
    [SerializeField] private bool invertScale;

    private Transform _player;

    private void Start()
    {
        _player = PlayerController.Instance.transform;
    }

    private void Update()
    {
        float scaleX = invertScale ? - Mathf.Abs(transform.localScale.x) : Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector2(
            transform.position.x < _player.position.x ? scaleX : -scaleX,
            transform.localScale.y);
    }
}
