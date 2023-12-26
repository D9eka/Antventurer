using System.Collections;
using UnityEngine;

namespace Creatures.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        public void Deactivate()
        {
            PlayerController.Instance.Deactivate();
        }
    }
}