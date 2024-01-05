using UnityEngine;

namespace Components.UI.Skills
{
    [CreateAssetMenu(fileName = "Skill")]
    public class SkillDef : ScriptableObject
    {
        [SerializeField] private SkillData _data;
        public SkillData Data => _data;
    }
}