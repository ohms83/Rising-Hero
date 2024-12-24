using Character.Controller;
using Skills;
using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerCharacter : GameCharacter
    {
        protected override void EquipSkill(SkillBase skill)
        {
            base.EquipSkill(skill);
            skill.IsAutoCast = true;
        }
    }
}
