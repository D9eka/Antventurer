using System;
using UnityEngine;

namespace Components.UI.Skills
{
    [Serializable]
    public class SkillData
    {
        public string Name;
        public Sprite Image;
        public string Action;
        public bool HaveKey;
        public string KeyText;
        public string Description;
    }
}