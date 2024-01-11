using Creatures.Player;
using DragonBones;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace Creatures.Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private UnityArmatureComponent armature;
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

        public static PlayerAnimator Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayAnimation(string name, bool loopTime)
        {
            armature.animation.Play(name, loopTime ? 0 : 1);
        }

        public void Deactivate()
        {
            PlayerController.Instance.Deactivate();
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
        PlayerAnimator.Instance.PlayAnimation(Name, LoopTime);
    }
}