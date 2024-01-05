using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Creatures.Player
{
    public class PlayerInputReader : MonoBehaviour
    {
        public EventHandler<Vector2> OnPlayerMove;
        public EventHandler<bool> OnPlayerJump;
        public EventHandler OnPlayerInteract;
        public EventHandler OnPlayerUseP2Skill;
        public EventHandler<bool> OnPlayerUseP3Skill;


        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            OnPlayerMove?.Invoke(this, direction);
        }

        public void OnJump(InputAction.CallbackContext context) 
        {
            if (context.started)
                OnPlayerJump?.Invoke(this, true);
            if (context.canceled)
                OnPlayerJump?.Invoke(this, false);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.canceled)
                OnPlayerInteract?.Invoke(this, EventArgs.Empty);
        }

        public void OnUseP2Skill(InputAction.CallbackContext context)
        {
            OnPlayerUseP2Skill?.Invoke(this, EventArgs.Empty);
        }

        public void OnActivateP3Skill(InputAction.CallbackContext context)
        {
            if (context.started)
                OnPlayerUseP3Skill?.Invoke(this, true);
            if(context.canceled)
                OnPlayerUseP3Skill?.Invoke(this, false);
        }
    }
}