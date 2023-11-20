using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void SetLoadingProgress(float loadingProgress)
        {
            _slider.value = loadingProgress;
        }
    }
}