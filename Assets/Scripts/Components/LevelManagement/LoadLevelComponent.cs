using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Components.UI;
using Creatures.Player;

namespace Components.LevelManagement
{
    public class LoadLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private LoadingScreen _loadingScreen;
        [Header("Position")]
        [SerializeField] private bool _havePosition;
        [SerializeField] private Vector2 _position;
        [Space]
        [SerializeField] private bool _cleanPlayerPrefs;

        public void Load()
        {
            if(_havePosition)
            {
                PlayerPrefsController.SetPlayerPosition(_position);
            }

            if (_cleanPlayerPrefs)
            {
                PlayerPrefsController.CleanPlayerInfo();
            }

            StartCoroutine(LoadAsync());
        }

        private IEnumerator LoadAsync()
        {
            AsyncOperation loadAsync = SceneManager.LoadSceneAsync(_sceneName);
            loadAsync.allowSceneActivation = false;
            _loadingScreen.gameObject.SetActive(true);

            while (!loadAsync.isDone) 
            {
                _loadingScreen.SetLoadingProgress(loadAsync.progress);

                if(loadAsync.progress >= 0.9f && !loadAsync.allowSceneActivation)
                {
                    yield return new WaitForSeconds(2.2f);
                    loadAsync.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}