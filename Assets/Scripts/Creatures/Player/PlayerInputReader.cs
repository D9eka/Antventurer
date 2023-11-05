using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures
{
    public class PlayerInputReader : MonoBehaviour
    {
        [SerializeField] private Player _player;

        public void OnMove(InputAction.CallbackContext context)
        { 
            var direction = context.ReadValue<Vector2>();
            _player.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if(context.canceled)
                _player.Interact();
        }

        public void OnChangeDoubleJumpState(InputAction.CallbackContext context)
        {
            _player.ChangeDoubleJumpState();
        }

        public void OnChangeWallJumpState(InputAction.CallbackContext context)
        {
            _player.ChangeWallJumpState();
        }
    }
}