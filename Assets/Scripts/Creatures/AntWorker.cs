using Components.UI.Dialogs;
using Components.UI.Skills;
using Creatures.Player;
using System;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Creatures
{
    public class AntWorker : MonoBehaviour
    {
        [SerializeField] private DialogBoxController _dialogBoxController;
        [Space]
        [SerializeField] private DialogDef _startDialog;
        [SerializeField] private UnityEvent _onStartStartDialog;
        [SerializeField] private UnityEvent _onFinishStartDialog;
        [Space]
        [SerializeField] private DialogDef _exitDialog;
        [SerializeField] private UnityEvent _onStartExitDialog;
        [SerializeField] private UnityEvent _onFinishExitDialog;
        [Space]
        [SerializeField] private DialogDef _finalDialog;
        [SerializeField] private UnityEvent _onStartFinalDialog;
        [SerializeField] private UnityEvent _onFinishFinalDialog;

        private DialogData activeDialog;
        private UnityEvent activeOnStartEvent;
        private UnityEvent activeOnFinishEvent;
        
        private Animator animator;

        private Transform player;

        private bool attention;

        private const string ATTENTION_KEY = "attention";

        private void Start()
        {
            SetActiveDialog();

            animator = GetComponentInChildren<Animator>();
            player = PlayerController.Instance.transform;
            PlayerController.Instance.OnUnlockSkill += PlayerController_OnUnlockSkill;
        }

        private void PlayerController_OnUnlockSkill(object sender, Skills.Skill e)
        {
            SetActiveDialog();
        }

        private void SetActiveDialog()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            switch (sceneName)
            {
                case "Start":
                    activeDialog = _startDialog.Data;
                    activeOnStartEvent = _onStartStartDialog;
                    activeOnFinishEvent = _onFinishStartDialog;
                    gameObject.SetActive(!PlayerPrefsController.GetTalkWithWorkerState());
                    break;
                case "Exit":
                    if(PlayerPrefsController.GetFlightState())
                    {
                        activeDialog = _finalDialog.Data;
                        activeOnStartEvent = _onStartFinalDialog;
                        activeOnFinishEvent = _onFinishFinalDialog;
                    }
                    else
                    {
                        activeDialog = _exitDialog.Data;
                        activeOnStartEvent = _onStartExitDialog;
                        activeOnFinishEvent = _onFinishExitDialog;
                    }
                    break;
            }
        }

        private void Update()
        {
            if (!attention)
                return;

            float scaleX = Mathf.Abs(transform.localScale.x);
            transform.localScale = new Vector2(
                transform.position.x > player.position.x ? scaleX : -scaleX,
                transform.localScale.y);
        }

        public void ChangeAttentionState()
        {
            attention = !attention;
            animator.SetBool(ATTENTION_KEY, attention);
        }

        public void ShowDialog()
        {
            _dialogBoxController.ShowDialog(activeDialog, activeOnStartEvent, activeOnFinishEvent);
        }

        public void ChangeFirstTalkState()
        {
            PlayerPrefsController.SetTalkWithWorkerState(true);
        }
    }
}