using Creatures.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Components.UI.Skills
{
    public class Skills : MonoBehaviour
    {
        [SerializeField] private SkillsContent _content;
        [Space]
        [Header("Skills Def")]
        [SerializeField] private SkillDef _doubleJumpSkillDef;
        [SerializeField] private SkillDef _wallJumpSkillDef;
        [SerializeField] private SkillDef _p2SkillDef;
        [SerializeField] private SkillDef _p3SkillDef;
        [SerializeField] private SkillDef _flightSkillDef;

        private const string SCREEN_ENABLED = "enabled";

        public static Skills Instance;

        public enum Skill
        {
            DoubleJump,
            WallJump,
            P2,
            P3,
            Flight,
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            PlayerController.Instance.OnUnlockSkill += PlayerController_OnUnlockSkill;
        }

        public void PlayerController_OnUnlockSkill(object sender, Skill e)
        {
            SkillData activeData = e switch
            {
                Skill.DoubleJump => _doubleJumpSkillDef.Data,
                Skill.WallJump => _wallJumpSkillDef.Data,
                Skill.P2 => _p2SkillDef.Data,
                Skill.P3 => _p3SkillDef.Data,
                Skill.Flight => _flightSkillDef.Data,
                _ => throw new NotImplementedException()
            };
            _content.SetData(activeData);

            StartCoroutine(ViewWindow(5f));
        }

        private IEnumerator ViewWindow(float viewTime)
        {
            GetComponent<Animator>().SetBool(SCREEN_ENABLED, true);
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForSeconds(viewTime);
            GetComponent<Animator>().SetBool(SCREEN_ENABLED, false);
            yield return new WaitForSeconds(0.3f);
        }
    }
}