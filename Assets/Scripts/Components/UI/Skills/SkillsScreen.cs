using Creatures.Player;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Components.UI.Skills
{
    public class SkillsScreen : MonoBehaviour
    {
        [SerializeField] private SkillsContent _content;
        [Space]
        [SerializeField] private SkillVisual _doubleJumpSkillVisual;
        [Space]
        [SerializeField] private SkillVisual _wallJumpSkillVisual;
        [Space]
        [SerializeField] private SkillVisual _p2SkillVisual;
        [Space]
        [SerializeField] private SkillVisual _p3SkillVisual;
        [Space]
        [SerializeField] private SkillVisual _flightSkillVisual;
        [Space]
        [SerializeField] private Sprite _statusSelectedSprite;
        [SerializeField] private Sprite _statusActiveSprite;
        [SerializeField] private Sprite _statusDisabledSprite;

        private SkillVisual _activeSkill;

        public SkillsContent Content => _content;

        private void Start()
        {
            AddListeners();

            _activeSkill = _doubleJumpSkillVisual;
            UpdateVisual();
        }

        private void OnEnable()
        {
            UpdateVisual();
        }

        private void AddListeners()
        {
            _doubleJumpSkillVisual.Button.onClick.AddListener(() => ChangeActiveSkill(_doubleJumpSkillVisual));
            _wallJumpSkillVisual.Button.onClick.AddListener(() => ChangeActiveSkill(_wallJumpSkillVisual));
            _p2SkillVisual.Button.onClick.AddListener(() => ChangeActiveSkill(_p2SkillVisual));
            _p3SkillVisual.Button.onClick.AddListener(() => ChangeActiveSkill(_p3SkillVisual));
            _flightSkillVisual.Button.onClick.AddListener(() => ChangeActiveSkill(_flightSkillVisual));
        }

        public void ChangeActiveSkill(SkillVisual activeSkill) 
        {
            _activeSkill = activeSkill;
            UpdateVisual();
        }

        private void UpdateVisual()
        {
            _content.SetData(_activeSkill.Def.Data);
            UpdateSkillVisuals();
        }

        private void UpdateSkillVisuals()
        {
            bool haveDoubleJumpSkill = PlayerPrefsController.GetPlayerDoubleJumpState();
            ChangeStatus(_doubleJumpSkillVisual, haveDoubleJumpSkill);

            bool haveJumpSkill = PlayerPrefsController.GetPlayerWallJumpState();
            ChangeStatus(_wallJumpSkillVisual, haveJumpSkill);

            bool haveP2Skill = PlayerPrefsController.GetPlayerP2State();
            ChangeStatus(_p2SkillVisual, haveP2Skill);

            bool haveP3Skill = PlayerPrefsController.GetPlayerP3State();
            ChangeStatus(_p3SkillVisual, haveP3Skill);

            bool haveFlightSkill = PlayerPrefsController.GetPlayerFlightState();
            ChangeStatus(_flightSkillVisual, haveFlightSkill);
        }

        private void ChangeStatus(SkillVisual skill, bool active)
        {
            skill.Button.interactable = active;
            Sprite sprite;
            if(active)
            {
                sprite = skill == _activeSkill ? _statusSelectedSprite : _statusActiveSprite;
            }
            else
                sprite = _statusDisabledSprite;

            skill.Status.sprite = sprite;
        }
    }

    [Serializable]
    public class SkillVisual
    {
        public SkillDef Def;
        public Button Button;
        public Image Status;
    }
}
