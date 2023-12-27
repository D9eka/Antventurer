using Creatures.Player;
using UnityEngine;

namespace Creatures
{
    public class AntWorker : MonoBehaviour
    {
        private Animator _animator;

        private Transform _player;

        private bool attention;

        private const string ATTENTION_KEY = "attention";

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();

            _player = PlayerController.Instance.transform;
        }

        private void Update()
        {
            if (!attention)
                return;

            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(
                transform.position.x < _player.position.x ? scaleX : -scaleX,
                transform.localScale.y);
        }

        public void ChangeAttentionState()
        {
            attention = !attention;
            _animator.SetBool(ATTENTION_KEY, attention);
        }
    }
}