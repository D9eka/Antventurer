using UnityEngine;

namespace Assets.Scripts.Creatures.Player
{
    public static class PlayerPrefsController
    {
        private const string PLAYER_POSITION_X = "PlayerPosX";
        private const string PLAYER_POSITION_Y = "PlayerPosY";

        private const string PLAYER_ALLOW_DOUBLE_JUMP = "PlayerAllowDoubleJump";
        private const string PLAYER_ALLOW_WALL_JUMP = "PlayerAllowWallJump";

        private const string PLAYER_MANA = "PlayerMana";

        public static bool TryGetPlayerPosition(out Vector2 position)
        {
            if (PlayerPrefs.HasKey(PLAYER_POSITION_X) && PlayerPrefs.HasKey(PLAYER_POSITION_Y))
            {
                position = new Vector2(PlayerPrefs.GetFloat(PLAYER_POSITION_X), PlayerPrefs.GetFloat(PLAYER_POSITION_Y));
                return true;
            }

            position = Vector2.zero;
            Debug.Log($"GET POS: 0 0");
            return false;
        }

        public static void SetPlayerPosition(Vector2 position)
        {
            PlayerPrefs.SetFloat(PLAYER_POSITION_X, position.x);
            PlayerPrefs.SetFloat(PLAYER_POSITION_Y, position.y);
        }

        public static bool GetPlayerDoubleJumpState()
        {
            if (PlayerPrefs.HasKey(PLAYER_ALLOW_DOUBLE_JUMP))
            {
                return PlayerPrefs.GetInt(PLAYER_ALLOW_DOUBLE_JUMP) == 1;
            }
            return false;
        }

        public static void SetPlayerDoubleJumpState(bool state)
        {
            PlayerPrefs.SetInt(PLAYER_ALLOW_DOUBLE_JUMP, state ? 1 : 0);
        }

        public static bool GetPlayerWallJumpState()
        {
            if (PlayerPrefs.HasKey(PLAYER_ALLOW_WALL_JUMP))
            {
                return PlayerPrefs.GetInt(PLAYER_ALLOW_WALL_JUMP) == 1;
            }
            return false;
        }

        public static void SetPlayerWallJumpState(bool state)
        {
            PlayerPrefs.SetInt(PLAYER_ALLOW_WALL_JUMP, state ? 1 : 0);
        }

        public static bool TryGetPlayerMana(out int mana)
        {
            if (PlayerPrefs.HasKey(PLAYER_MANA))
            {
                mana = PlayerPrefs.GetInt(PLAYER_MANA);
                return true;
            }
            mana = 0;
            return false;
        }

        public static void SetPlayerMana(int mana)
        {
            PlayerPrefs.SetInt(PLAYER_MANA, mana);
        }

        public static void CleanPlayerInfo()
        {
            PlayerPrefs.DeleteKey(PLAYER_POSITION_X);
            PlayerPrefs.DeleteKey(PLAYER_POSITION_Y);

            PlayerPrefs.DeleteKey(PLAYER_ALLOW_DOUBLE_JUMP);
            PlayerPrefs.DeleteKey(PLAYER_ALLOW_WALL_JUMP);

            PlayerPrefs.DeleteKey(PLAYER_MANA);
        }
    }
}