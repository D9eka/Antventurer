using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Components.UI.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;
        [Space]
        [SerializeField] private float _textSpeed = 0.05f;
        [Space]
        [SerializeField] protected DialogContent _content;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentence];
        protected virtual DialogContent CurrentContent => _content;

        private const string IS_OPEN_KEY = "is-open";

        private DialogData _data;
        private int _currentSentence;
        private Coroutine _typingRoutine;

        private UnityEvent _onFinishDialog;

        public void ShowDialog(DialogData data, UnityEvent onStart, UnityEvent onFinish)
        {
            onStart?.Invoke();
            _onFinishDialog = onFinish;

            _data = data;
            _currentSentence = 0;
            _content.Text.text = "";

            _container.SetActive(true);
            _animator.SetBool(IS_OPEN_KEY, true);
        }

        private IEnumerator TypeDialogText()
        {
            _content.Text.text = string.Empty;
            /* Voice
            if (CurrentSentence.Voice != null)
                AudioHandler.Instance.PlaySound(CurrentSentence.Voice);
            */

            var text = CurrentSentence.Value;
            foreach (var letter in text)
            {
                _content.Text.text += letter;
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }

        public void OnSkip()
        {
            if (_typingRoutine == null)
                return;

            StopTypeAnimation();
            _content.Text.text = _data.Sentences[_currentSentence].Value;
        }

        public void OnContinue()
        {
            if (_typingRoutine != null)
            {
                OnSkip();
                return;
            }

            StopTypeAnimation();
            _currentSentence++;

            var isDialogComplete = _currentSentence >= _data.Sentences.Length;
            if (isDialogComplete)
                HideDialogBox();
            else
                OnStartDialogAnimationComplete();
        }

        private void HideDialogBox()
        {
            _animator.SetBool(IS_OPEN_KEY, false);
        }

        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }

        protected virtual void OnStartDialogAnimationBegin()
        {
            _container.SetActive(true);
        }

        protected virtual void OnStartDialogAnimationComplete()
        {
            //Cursor.visible = true;
            _typingRoutine = StartCoroutine(TypeDialogText());
        }

        protected virtual void OnCloseAnimationComplete()
        {
            _onFinishDialog?.Invoke();
            //Cursor.visible = false;
            _container.SetActive(false);
        }
    }
}