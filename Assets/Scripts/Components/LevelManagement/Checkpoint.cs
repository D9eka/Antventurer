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
            Vector2 lastPlayerPosition = PlayerPrefsController.GetPlayerPosition();
            if (Vector2.Distance(PlayerController.Instance.transform.position, lastPlayerPosition) < 3f)
                return;

            PlayerPrefsController.SavePlayerData();

            HUD.Instance.SendMessage("Сохранение...", 3f);
        }
    }
}