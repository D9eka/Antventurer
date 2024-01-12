using InstantGamesBridge;
using InstantGamesBridge.Common;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Creatures.Player
{
    public static class PlayerPrefsController
    {
        #region Player
        private const string LOCATION = "PlayerLocation";
        private const string POSITION_X = "PlayerPosX";
        private const string POSITION_Y = "PlayerPosY";
        private const string SCALE = "PlayerScale";

        private const string MANA = "PlayerMana";
        private const string MAX_MANA = "PlayerMaxMana";

        private const string PROGRESS = "PlayerProgress";
        private const string ALLOW_DOUBLE_JUMP = "PlayerAllowDoubleJump";
        private const string ALLOW_WALL_JUMP = "PlayerAllowWallJump";
        private const string ALLOW_P2_SKILL = "PlayerAllowP2Skill";
        private const string ALLOW_P3_SKILL = "PlayerAllowP3Skill";
        private const string ALLOW_FLIGHT = "PlayerAllowFlight";

        private const string FIRST_START = "PlayerFirstStart";
        private static Vector2 firstStartPos = new Vector2(3.319f, 0.8893859f);
        private const string TALK_WITH_WORKER = "PlayerTalkWithWorker";

        public static PlayerData GetPlayerData()
        {
            if (!PlayerPrefs.HasKey(FIRST_START))
                return new PlayerData(GetLocation(), GetPosition());
            else
                return new PlayerData(GetLocation(), GetPosition(), GetScale(),
                                      GetMana(), GetMaxMana(),
                                      GetProgress(), GetDoubleJumpState(), GetWallJumpState(),
                                      GetP2State(), GetP3State(), GetFlightState(),
                                      GetFirstStartState(), GetTalkWithWorkerState());
        }

        public static bool GetDataFromServer()
        {
            string json = LoadExtern();
            if (json == null || json == "")
                return false;

            PlayerData data = JsonUtility.FromJson<PlayerData>(json);
            SaveData(data);
            return true;
        }

        public static bool HaveData()
        {
            return PlayerPrefs.HasKey(FIRST_START);
        }

        public static void SavePlayerData()
        {
            if (PlayerController.Instance == null)
                return;

            PlayerData data = PlayerController.Instance.SaveData();
            SaveData(data);
        }

        public static void SaveData(PlayerData data)
        {
            SetPosition(data.Position.Value);
            SetScale(data.Scale);

            SetMana(data.Mana);
            SetMaxMana(data.MaxMana);

            SetProgress(data.Progress);
            SetDoubleJumpState(data.DoubleJumpState);
            SetWallJumpState(data.WallJumpState);
            SetP2State(data.P2State);
            SetP3State(data.P3State);
            SetFlightState(data.FlightState);

            SetFirstStartState(data.FirstStart);
            SetTalkWithWorkerState(data.TalkWithWorker);


            if (Bridge.platform.id == PlatformId.Yandex)
            {
                string jsonString = JsonUtility.ToJson(data);
                SaveExtern(jsonString);
            }
        }

        [DllImport("__Internal")]
        private static extern void SaveExtern(string data);

        [DllImport("__Internal")]
        private static extern string LoadExtern();

        #endregion

        #region Audio
        private const string MUSIC_VOLUME = "MusicVolume";


        public static float GetMusicVolume()
        {
            return GetFloat(MUSIC_VOLUME, 1f);
        }

        public static void SetMusicVolume(float volume)
        {
            SetFloat(MUSIC_VOLUME, volume);
        }
        #endregion

        #region Position
        public static Vector2 GetPosition()
        {
            return new Vector2(PlayerPrefs.GetFloat(POSITION_X, firstStartPos.x), PlayerPrefs.GetFloat(POSITION_Y, firstStartPos.y));
        }

        public static void SetPosition(Vector2 position)
        {
            PlayerPrefs.SetFloat(POSITION_X, position.x);
            PlayerPrefs.SetFloat(POSITION_Y, position.y);
        }
        #endregion

        #region Scale
        public static float GetScale()
        {
            return GetFloat(SCALE, 1f);
        }

        public static void SetScale(float scale)
        {
            SetFloat(SCALE, scale);
        }
        #endregion

        #region Location
        public static string GetLocation()
        {
            return GetString(LOCATION);
        }
        public static void SetPlayerLocation(string location)
        {
            SetString(LOCATION, location);
        }
        #endregion

        #region Mana
        public static float GetMana()
        {
            return GetFloat(MANA);
        }

        public static void SetMana(float mana)
        {
            SetFloat(MANA, mana);
        }
        #endregion

        #region MaxMana
        public static float GetMaxMana()
        {
            return GetFloat(MAX_MANA);
        }

        public static void SetMaxMana(float mana)
        {
            SetFloat(MAX_MANA, mana);
        }
        #endregion

        #region Progress
        public static float GetProgress()
        {
            return GetFloat(PROGRESS);
        }

        public static void SetProgress(float progress)
        {
            SetFloat(PROGRESS, progress);
        }
        #endregion

        #region DoubleJump
        public static bool GetDoubleJumpState()
        {
            return GetBool(ALLOW_DOUBLE_JUMP);
        }

        public static void SetDoubleJumpState(bool state)
        {
            SetBool(ALLOW_DOUBLE_JUMP, state);
        }
        #endregion

        #region WallJump
        public static bool GetWallJumpState()
        {
            return GetBool(ALLOW_WALL_JUMP);
        }

        public static void SetWallJumpState(bool state)
        {
            SetBool(ALLOW_WALL_JUMP, state);
        }
        #endregion

        #region P2
        public static bool GetP2State()
        {
            return GetBool(ALLOW_P2_SKILL);
        }

        public static void SetP2State(bool state)
        {
            SetBool(ALLOW_P2_SKILL, state);
        }
        #endregion

        #region P3
        public static bool GetP3State()
        {
            return GetBool(ALLOW_P3_SKILL);
        }

        public static void SetP3State(bool state)
        {
            SetBool(ALLOW_P3_SKILL, state);
        }
        #endregion

        #region Flight
        public static bool GetFlightState()
        {
            return GetBool(ALLOW_FLIGHT);
        }

        public static void SetFlightState(bool state)
        {
            SetBool(ALLOW_FLIGHT, state);
        }
        #endregion

        #region FirstStart
        public static bool GetFirstStartState()
        {
            return GetBool(FIRST_START, true);
        }

        public static void SetFirstStartState(bool state)
        {
            SetBool(FIRST_START, state);
        }
        #endregion

        #region TalkWithWorker
        public static bool GetTalkWithWorkerState()
        {
            return GetBool(TALK_WITH_WORKER);
        }

        public static void SetTalkWithWorkerState(bool state)
        {
            SetBool(TALK_WITH_WORKER, state);
        }
        #endregion

        #region Bool
        private static bool GetBool(string key, bool defaultValue = false)
        {
            if (PlayerPrefs.HasKey(key))
            {
                return PlayerPrefs.GetInt(key) == 1;
            }
            return defaultValue;
        }

        private static void SetBool(string key, bool state) 
        {
            PlayerPrefs.SetInt(key, state ? 1 : 0);
        }
        #endregion

        #region Float
        public static float GetFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetFloat(key) : defaultValue;
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
        #endregion

        #region String
        public static string GetString(string key)
        {
            return PlayerPrefs.HasKey(key) ? PlayerPrefs.GetString(key) : "";
        }
        public static void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }
        #endregion

        public static void CleanPlayerInfo()
        {
            PlayerPrefs.DeleteKey(POSITION_X);
            PlayerPrefs.DeleteKey(POSITION_Y);
            PlayerPrefs.DeleteKey(SCALE);
            PlayerPrefs.DeleteKey(LOCATION);

            PlayerPrefs.DeleteKey(MANA);
            PlayerPrefs.DeleteKey(MAX_MANA);

            PlayerPrefs.DeleteKey(ALLOW_DOUBLE_JUMP);
            PlayerPrefs.DeleteKey(ALLOW_WALL_JUMP);
            PlayerPrefs.DeleteKey(ALLOW_P2_SKILL);
            PlayerPrefs.DeleteKey(ALLOW_P3_SKILL);
            PlayerPrefs.DeleteKey(ALLOW_FLIGHT);

            PlayerPrefs.DeleteKey(FIRST_START);
            PlayerPrefs.DeleteKey(TALK_WITH_WORKER);
        }
    }
}