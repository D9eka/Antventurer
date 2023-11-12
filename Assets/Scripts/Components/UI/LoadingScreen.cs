using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        /*
        private const string SCREEN_PATH = "UI/LoadingScreen";
        private static LoadingScreen _instance;

        public static LoadingScreen Instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<LoadingScreen>(SCREEN_PATH);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        */

        public void SetLoadingProgress(float loadingProgress)
        {
            _slider.value = loadingProgress;
        }
    }
}