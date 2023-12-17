using System;
using UnityEngine;

namespace Components.UI
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private Action _fadeInCallback;
        private Action _fadeOutCallback;

        private static readonly int Fade = Animator.StringToHash("Fade");
        public bool IsFading { get; private set; }

        public static Fader Instance;

        private void Start()
        {
            if(Instance != null)
                Destroy(Instance);
            Instance = this;
        }

        public void FadeIn(Action fadeInCallback)
        {
            if (IsFading)
                return;

            IsFading = true;
            _fadeInCallback = fadeInCallback;
            _animator.SetBool(Fade, true);
        }

        public void FadeOut(Action fadedOutCallBack)
        {
            if (IsFading)
                return;

            IsFading = true;
            _fadeInCallback = fadedOutCallBack;
            _animator.SetBool(Fade, false);
        }

        private void FadeInAnimationOver()
        {
            _fadeInCallback?.Invoke();
            _fadeInCallback = null;
            IsFading = false;
        }
        private void FadeOutAnimationOver()
        {
            _fadeOutCallback?.Invoke();
            _fadeOutCallback = null;
            IsFading = false;
        }
    }
}