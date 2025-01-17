using System;
using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Skills
{
    public abstract class SkillEffect : ScriptableObject
    {
        public SkillData data;

        public abstract void Activate(GameCharacter owner);
        public  abstract void Deactivate(GameCharacter owner);
    }
}
