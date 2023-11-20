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

        private void Start()
        {
            Player.Instance.OnUnlockDoubleJump += Skills_OnUnlockDoubleJump;
        }

        public void Skills_OnUnlockDoubleJump(object sender, EventArgs e)
        {
            StartCoroutine(ViewWindow(_unlockDoubleJumpWindow, 5f));
        }

        private IEnumerator ViewWindow(GameObject window, float viewTime)
        {
            window.SetActive(true);
            yield return new WaitForSeconds(viewTime);
            window.SetActive(false);
        }
    }
}