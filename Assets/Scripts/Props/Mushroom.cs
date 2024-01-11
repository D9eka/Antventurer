using Components.UI.Skills;
using Creatures.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Components.UI.Skills.Skills;

namespace Props
{
    public class Mushroom : MonoBehaviour
    {
        [SerializeField] private Skill _unlocksSkill;

        private void Start()
        {
            bool isSkillUnlocked = _unlocksSkill switch
            {
                Skill.DoubleJump => PlayerPrefsController.GetDoubleJumpState(),
                Skill.WallJump => PlayerPrefsController.GetWallJumpState(),
                Skill.P2 => PlayerPrefsController.GetP2State(),
                Skill.P3 => PlayerPrefsController.GetP3State(),
                Skill.Flight => PlayerPrefsController.GetFlightState(),
                _ => true
            };
            gameObject.SetActive(!isSkillUnlocked);
        }

        public void UnlockSkill()
        {
            PlayerController player = PlayerController.Instance;
            switch (_unlocksSkill)
            {
                case Skill.DoubleJump:
                    player.UnlockDoubleJump();
                    break;
                case Skill.WallJump: 
                    player.UnlockWallJump(); 
                    break;
                case Skill.P2:
                    player.UnlockP2();
                    break;
                case Skill.P3:
                    player.UnlockP3();
                    break;
                case Skill.Flight:
                    player.UnlockFlight();
                    break;
                default:
                    break;
            }
        }
    }
}
