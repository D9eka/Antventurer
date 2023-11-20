using Assets.Scripts.Creatures.Player;
using Components.Mana;
using Components.UI;
using Creatures.Player;
using UnityEngine;

namespace Components.LevelManagement
{
    public class Checkpoint : MonoBehaviour
    {
        public void SavePosition()
        {
            PlayerPrefsController.SetPlayerPosition(Player.Instance.transform.position);
            PlayerPrefsController.SetPlayerDoubleJumpState(Player.Instance.AllowDoubleJump);
            PlayerPrefsController.SetPlayerWallJumpState(Player.Instance.AllowWallJump);
            PlayerPrefsController.SetPlayerMana(Player.Instance.GetComponent<ManaComponent>().Mana);

            HUD.Instance.SendMessage("Сохранение...", 3f);
        }
    }
}