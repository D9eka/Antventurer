using System;
using UnityEngine;
using UnityEngine.Events;

namespace Components.UI.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private DialogBoxController _dialogBox;
        [SerializeField] private UnityEvent _onStart;
        [SerializeField] private UnityEvent _onFinish;
        [Space]
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;

        private DialogData _data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public void Show()
        {
            _dialogBox.ShowDialog(_data, _onStart, _onFinish);
        }
    }
}