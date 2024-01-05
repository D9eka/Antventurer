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
        [SerializeField] private SkillDef _doubleJumpSkillData;
        [SerializeField] private SkillDef _wallJumpSkillData;
        [SerializeField] private SkillDef _p2SkillData;
        [SerializeField] private SkillDef _p3SkillData;
        [SerializeField] private SkillDef _flightSkillData;

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
                Skill.DoubleJump => _doubleJumpSkillData.Data,
                Skill.WallJump => _wallJumpSkillData.Data,
                Skill.P2 => _p2SkillData.Data,
                Skill.P3 => _p3SkillData.Data,
                Skill.Flight => _flightSkillData.Data,
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