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
            if (PlayerPrefsController.TryGetPlayerPosition(out Vector2 lastplayerPosition) &&
               Vector2.Distance(PlayerController.Instance.transform.position, lastplayerPosition) < 3f)
                return;

            PlayerPrefsController.SavePlayerData();

            HUD.Instance.SendMessage("Сохранение...", 3f);
        }
    }
}