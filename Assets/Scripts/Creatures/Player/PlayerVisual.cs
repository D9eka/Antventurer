using Components.UI.Skills;
using Creatures.Player;
using DragonBones;
using System;
using UnityEngine;
using static Components.UI.Skills.Skills;

namespace Creatures.Player
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private UnityArmatureComponent armature;
        [Space]
        [Header("Visuals")]
        [SerializeField] private GameObject firstVisual;
        [SerializeField] private GameObject secondVisual;
        [SerializeField] private GameObject thirdVisual;
        [SerializeField] private GameObject fourthVisual;
        [SerializeField] private GameObject fifthVisual;
        [Space]
        #region Animations
        [Header("Animations")]
        [SerializeField] private Animation _birthAnimation;
        [SerializeField] private Animation _idleAnimation;
        [SerializeField] private Animation _walkAnimation;
        [SerializeField] private Animation _jumpAnimation;
        [SerializeField] private Animation _fallAnimation;
        [SerializeField] private Animation _landAnimation;
        [SerializeField] private Animation _doubleJumpAnimation;
        [SerializeField] private Animation _deathAnimation;
        [SerializeField] private Animation _deathAntlionAnimation;
        [SerializeField] private Animation _slipAnimation;
        [SerializeField] private Animation _JumpToWallAnimation;
        [SerializeField] private Animation _wallToJumpAnimation;
        [SerializeField] private Animation _flyAnimation;

        public void PlayBirthAnimation() => _birthAnimation.Play();
        public void PlayIdleAnimation() => _idleAnimation.Play();
        public void PlayWalkAnimation() => _walkAnimation.Play();
        public void PlayJumpAnimation() => _jumpAnimation.Play();
        public void PlayFallAnimation() => _fallAnimation.Play();
        public void PlayLandAnimation() => _landAnimation.Play();
        public void PlayDoubleJumpAnimation() => _doubleJumpAnimation.Play();
        public void PlayDeathAnimation() => _deathAnimation.Play();
        public void PlayDeathAntlionAnimation() => _deathAntlionAnimation.Play();
        public void PlaySlipAnimation() => _slipAnimation.Play();
        public void PlayJumpToWallAnimation() => _JumpToWallAnimation.Play();
        public void PlayWallToJumpAnimation() => _wallToJumpAnimation.Play();
        public void PlayFlyAnimation() => _flyAnimation.Play();
        #endregion

        public static PlayerVisual Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            PlayerController.Instance.OnChangeVisual += PlayerController_OnChangeVisual;

            SetActualVisual();
        }

        private void SetActualVisual()
        {
            if (PlayerPrefsController.GetP3State())
                ChangeVisual(fifthVisual);
            else if (PlayerPrefsController.GetP2State())
                ChangeVisual(fourthVisual);
            else if (PlayerPrefsController.GetWallJumpState())
                ChangeVisual(thirdVisual);
            else if (PlayerPrefsController.GetDoubleJumpState())
                ChangeVisual(secondVisual);
            else
                ChangeVisual(firstVisual);
        }

        private void ChangeVisual(GameObject visual)
        {
            DisableAllVisuals();
            visual.SetActive(true);
            armature = visual.GetComponent<UnityArmatureComponent>();
        }

        private void DisableAllVisuals()
        {
            firstVisual.SetActive(false);
            secondVisual.SetActive(false);
            thirdVisual.SetActive(false);
            fourthVisual.SetActive(false);
            fifthVisual.SetActive(false);
        }    

        private void PlayerController_OnChangeVisual(object sender, Skill e)
        {
            ChangeVisual(e switch
            {
                Skill.DoubleJump => secondVisual,
                Skill.WallJump => thirdVisual,
                Skill.P2 => fourthVisual,
                Skill.P3 => fifthVisual,
                Skill.Flight => fifthVisual,
                _ => firstVisual
            });
        }

        public void PlayAnimation(string name, bool loopTime)
        {
            armature.animation.Play(name, loopTime ? 0 : 1);
        }

        public void Deactivate()
        {
            PlayerController.Instance.Deactivate();
            armature.sortingOrder = -1;
        }
    }
}

[Serializable]
public class Animation
{
    public string Name;
    public bool LoopTime;

    public void Play()
    {
        PlayerVisual.Instance.PlayAnimation(Name, LoopTime);
    }
}