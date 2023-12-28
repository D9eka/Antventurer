using Creatures.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Components.UI
{
    public class Skills : MonoBehaviour
    {
        [Header("UnlockWindows")]
        [SerializeField] private GameObject _unlockDoubleJumpWindow;
        [SerializeField] private GameObject _unlockWallJumpWindow;
        [SerializeField] private GameObject _unlockP2Window;
        [SerializeField] private GameObject _unlockP3Window;

        private void Start()
        {
            PlayerController.Instance.OnUnlockDoubleJump += PlayerController_OnUnlockDoubleJump;
            PlayerController.Instance.OnUnlockWallJump += PlayerController_OnUnlockWallJump;
            PlayerController.Instance.OnUnlockP2 += PlayerController_OnUnlockP2;
            PlayerController.Instance.OnUnlockP3 += PlayerController_OnUnlockP3;
        }

        public void PlayerController_OnUnlockDoubleJump(object sender, EventArgs e)
        {
            StartCoroutine(ViewWindow(_unlockDoubleJumpWindow, 5f));
        }

        private void PlayerController_OnUnlockWallJump(object sender, EventArgs e)
        {
            StartCoroutine(ViewWindow(_unlockWallJumpWindow, 5f));
        }

        private void PlayerController_OnUnlockP2(object sender, EventArgs e)
        {
            StartCoroutine(ViewWindow(_unlockP2Window, 5f));
        }

        private void PlayerController_OnUnlockP3(object sender, EventArgs e)
        {
            StartCoroutine(ViewWindow(_unlockP3Window, 5f));
        }

        private IEnumerator ViewWindow(GameObject window, float viewTime)
        {
            window.SetActive(true);
            yield return new WaitForSeconds(viewTime);
            window.SetActive(false);
        }
    }
}