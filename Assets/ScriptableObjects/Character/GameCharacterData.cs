using System;
using System.Collections.Generic;
using Character;
using Gameplay;
using Gameplay.Equipment;
using Skills;
using UnityEditor.Animations;
using UnityEngine;

namespace ScriptableObjects.Character
{
    [Serializable]
    public struct AttackData
    {
        public AnimationClip animationClip;
        public Rect hitBox;

    }
    [CreateAssetMenu(fileName = "GameCharacterData", menuName = "Scriptable Objects/Character/GameCharacterData")]
    public class GameCharacterData : ScriptableObject
    {
        public GameCharacter prefab;
        [Tooltip("Character's default stats")]
        public Stats defaultStats;
        public List<SkillType> defaultSkills;
        public List<Equipment> defaultEquipments;
        public List<Rect> hurtBoxes;
        public List<AttackData> attackData;
    }
}
