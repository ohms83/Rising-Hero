using System;
using Character;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "MeleeAttackSkill", menuName = "Scriptable Objects/Skills/Effects")]
    public class MeleeAttackSkill : SkillEffect
    {
        public override void Activate(GameCharacter owner)
        {
            owner.CharacterAnimation.Attack();
        }

        public override void Deactivate(GameCharacter owner)
        {
            
        }
    }
}