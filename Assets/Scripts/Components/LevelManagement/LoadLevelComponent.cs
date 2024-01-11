using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Components.UI;
using Creatures.Player;

namespace Components.LevelManagement
{
    public enum LoadingMode
    {
        Manually,
        FromSave,
        ToMainMenu
    }

    public enum LoadingWindow
    {
        Screen,
        Fading
    }

    public class LoadLevelComponent : MonoBehaviour
    {
        [SerializeField] private LoadingWindow _window;
        [SerializeField] private LoadingMode _mode;

        [SerializeField] private string _sceneName;
        [SerializeField] private Vector2 _position;
        [SerializeField] private bool _invertScale;

        [SerializeField] private bool _cleanPlayerPrefs;

        public void Load()
        {
            PlayerPrefsController.SavePlayerData();
            switch(_mode)
            {
                case LoadingMode.Manually:
                    PlayerPrefsController.SetPosition(_position);
                    PlayerPrefsController.SetScale(_invertScale ? -1 : 1);
                    PlayerPrefsController.SetPlayerLocation(_sceneName);
                    break;
                case LoadingMode.FromSave:
                    _sceneName = PlayerPrefsController.GetLocation();
                    break;
                case LoadingMode.ToMainMenu:
                    _sceneName = "MainMenu";
                    break;
            }

            if (_cleanPlayerPrefs)
                PlayerPrefsController.CleanPlayerInfo();

            StartCoroutine(LoadAsync());
        }

        private IEnumerator LoadAsync()
        {
            switch(_window)
            {
                case LoadingWindow.Fading:
                    bool waitFading = true;
                    Fader.Instance.FadeIn(() => waitFading = false);

                    yield return new WaitUntil(() => waitFading == false);

                    SceneManager.LoadScene(_sceneName);

                    waitFading = true;
                    Fader.Instance.FadeOut(() => waitFading = false);

                    yield return new WaitUntil(() => waitFading == false);
                    break;
                case LoadingWindow.Screen:
                    AsyncOperation loadAsync = SceneManager.LoadSceneAsync(_sceneName);
                    loadAsync.allowSceneActivation = false;
                    LoadingScreen.Instance.Activate();

                    while (!loadAsync.isDone)
                    {
                        LoadingScreen.Instance.SetLoadingProgress(loadAsync.progress);

                        if (loadAsync.progress >= 0.9f && !loadAsync.allowSceneActivation)
                        {
                            yield return new WaitForSeconds(2.2f);
                            loadAsync.allowSceneActivation = true;
                        }
                        yield return null;
                    }
                    break;
            }
        }
    }
}