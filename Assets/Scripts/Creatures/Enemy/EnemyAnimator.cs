using Creatures.Enemy;
using Creatures.Player;
using DragonBones;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Creatures.Enemy
{
    public class EnemyAnimator : MonoBehaviour
    {
        [SerializeField] private UnityArmatureComponent armature;
        [Space]
        #region Animations
        [Header("Animations")]
        [SerializeField] private Animation _idleAnimation;
        [SerializeField] private Animation _walkAnimation;
        [SerializeField] private Animation _jumpAnimation;
        [SerializeField] private Animation _fallAnimation;
        [SerializeField] private Animation _landAnimation;
        [SerializeField] private Animation _attentionAnimation;
        [SerializeField] private Animation _findAnimation;
        [SerializeField] private Animation _attackAnimation;
        [SerializeField] private Animation _deathAnimation;
        [SerializeField] private Animation _deathAntlionAnimation;

        public void PlayIdleAnimation() => _idleAnimation.Play(this);
        public void PlayWalkAnimation() => _walkAnimation.Play(this);
        public void PlayJumpAnimation() => _jumpAnimation.Play(this);
        public void PlayFallAnimation() => _fallAnimation.Play(this);
        public void PlayLandAnimation() => _landAnimation.Play(this);
        public void PlayAttentionAnimation() => _attentionAnimation.Play(this);
        public void PlayFindAnimation() => _findAnimation.Play(this);
        public void PlayAttackAnimation() => _attackAnimation.Play(this);
        public void PlayDeathAnimation() => _deathAnimation.Play(this);
        public void PlayDeathAntlionAnimation() => _deathAntlionAnimation.Play(this);
        #endregion

        public void PlayAnimation(string name, bool loopTime)
        {
            armature.animation.Play(name, loopTime ? 0 : 1);
        }

        public void Stop()
        {
            GetComponentInParent<EnemyController>().Stop();
        }
    }

    [Serializable]
    public class Animation
    {
        public string Name;
        public bool LoopTime;

        public void Play(EnemyAnimator animator)
        {
            animator.PlayAnimation(Name, LoopTime);
        }
    }
}