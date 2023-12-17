using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private Slider _slider;

        public static LoadingScreen Instance;

        private void Start()
        {
            if(Instance != null) 
            { 
                Destroy(Instance);
            }
            Instance = this;
        }

        public void Activate()
        {
            _slider.value = 0;
            _content.SetActive(true);
        }

        public void SetLoadingProgress(float loadingProgress)
        {
            _slider.value = loadingProgress;
        }
    }
}