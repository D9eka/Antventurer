using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures.Player
{
    public static class PlayerPrefsController
    {
        private const string PLAYER_POSITION_X = "PlayerPosX";
        private const string PLAYER_POSITION_Y = "PlayerPosY";

        private const string PLAYER_MANA = "PlayerMana";
        private const string PLAYER_MAX_MANA = "PlayerMaxMana";

        private const string PLAYER_ALLOW_DOUBLE_JUMP = "PlayerAllowDoubleJump";
        private const string PLAYER_ALLOW_WALL_JUMP = "PlayerAllowWallJump";
        private const string PLAYER_ALLOW_P2_SKILL = "PlayerAllowP2Skill";
        private const string PLAYER_ALLOW_P3_SKILL = "PlayerAllowP3Skill";
        private const string PLAYER_ALLOW_FLIGHT = "PlayerAllowFlight";

        public static PlayerData GetPlayerData()
        {
            Vector2? position = null;
            if (!TryGetPlayerPosition(out Vector2 pos) || GetPlayerMaxMana() == 0)
                return new PlayerData(position);
            else
                return new PlayerData(pos, GetPlayerMana(), GetPlayerMaxMana(), 
                                      GetPlayerDoubleJumpState(), GetPlayerWallJumpState(), GetPlayerP2State(), GetPlayerP3State(), GetPlayerFlightState());
        }

        public static void SavePlayerData()
        {
            PlayerData data = PlayerController.Instance.SaveData();

            SetPlayerPosition(data.Position.Value);

            SetPlayerMana(data.Mana);
            SetPlayerMaxMana(data.MaxMana);

            SetPlayerDoubleJumpState(data.DoubleJumpState);
            SetPlayerWallJumpState(data.WallJumpState);
            SetPlayerP2State(data.P2State);
            SetPlayerP3State(data.P3State);
            SetPlayerFlightState(data.FlightState);
        }

        #region Position
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
        #endregion

        #region Mana
        public static float GetPlayerMana()
        {
            return GetFloat(PLAYER_MANA);
        }

        public static void SetPlayerMana(float mana)
        {
            SetFloat(PLAYER_MANA, mana);
        }
        #endregion

        #region MaxMana
        public static float GetPlayerMaxMana()
        {
            return GetFloat(PLAYER_MAX_MANA);
        }

        public static void SetPlayerMaxMana(float mana)
        {
            SetFloat(PLAYER_MAX_MANA, mana);
        }
        #endregion

        #region DoubleJump
        public static bool GetPlayerDoubleJumpState()
        {
            return GetBool(PLAYER_ALLOW_DOUBLE_JUMP);
        }

        public static void SetPlayerDoubleJumpState(bool state)
        {
            SetBool(PLAYER_ALLOW_DOUBLE_JUMP, state);
        }
        #endregion

        #region WallJump
        public static bool GetPlayerWallJumpState()
        {
            return GetBool(PLAYER_ALLOW_WALL_JUMP);
        }

        public static void SetPlayerWallJumpState(bool state)
        {
            SetBool(PLAYER_ALLOW_WALL_JUMP, state);
        }
        #endregion

        #region P2
        public static bool GetPlayerP2State()
        {
            return GetBool(PLAYER_ALLOW_P2_SKILL);
        }

        public static void SetPlayerP2State(bool state)
        {
            SetBool(PLAYER_ALLOW_P2_SKILL, state);
        }
        #endregion

        #region P3
        public static bool GetPlayerP3State()
        {
            return GetBool(PLAYER_ALLOW_P3_SKILL);
        }

        public static void SetPlayerP3State(bool state)
        {
            SetBool(PLAYER_ALLOW_P3_SKILL, state);
        }
        #endregion

        #region Flight
        public static bool GetPlayerFlightState()
        {
            return GetBool(PLAYER_ALLOW_FLIGHT);
        }

        public static void SetPlayerFlightState(bool state)
        {
            SetBool(PLAYER_ALLOW_FLIGHT, state);
        }
        #endregion

        private static bool GetBool(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key) == 1;
            }
            return false;
        }

        private static void SetBool(string key, bool state) 
        {
            PlayerPrefs.SetInt(key, state ? 1 : 0);
        }

        public static float GetFloat(string key)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : 0;
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static void CleanPlayerInfo()
        {
            PlayerPrefs.DeleteKey(PLAYER_POSITION_X);
            PlayerPrefs.DeleteKey(PLAYER_POSITION_Y);

            PlayerPrefs.DeleteKey(PLAYER_MANA);
            PlayerPrefs.DeleteKey(PLAYER_MAX_MANA);

            PlayerPrefs.DeleteKey(PLAYER_ALLOW_DOUBLE_JUMP);
            PlayerPrefs.DeleteKey(PLAYER_ALLOW_WALL_JUMP);
            PlayerPrefs.DeleteKey(PLAYER_ALLOW_P2_SKILL);
            PlayerPrefs.DeleteKey(PLAYER_ALLOW_P3_SKILL);
            PlayerPrefs.DeleteKey(PLAYER_ALLOW_FLIGHT);
        }
    }
}