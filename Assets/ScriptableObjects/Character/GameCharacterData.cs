using System.Collections.Generic;
using Character;
using Gameplay;
using Gameplay.Equipment;
using Skills;
using UnityEditor.Animations;
using UnityEngine;

namespace ScriptableObjects.Character
{
    [CreateAssetMenu(fileName = "GameCharacterData", menuName = "Scriptable Objects/GameCharacterData")]
    public class GameCharacterData : ScriptableObject
    {
        public GameCharacter prefab;
        public Sprite sprite;
        public AnimatorController animController;
        public Stats stats;
        
        [Tooltip("Camera's tracking target (only used by player characters)")]
        public Vector2 cameraTarget;
        
        public List<SkillBase> defaultSkills;
        public List<Equipment> defaultEquipments;
        public List<Rect> hitBoxes;
        public List<Rect> hurtBoxes;
    }
}
