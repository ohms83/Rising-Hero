using Skills;
using UnityEngine;

namespace Gameplay.Equipment
{
    public enum EquipmentType
    {
        Weapon,
        Armor,
        Accessory,
        Max
    }
    
    public interface IEquipable
    {
        public Stats CombinedStats { get; }
        public void Equip(EquipmentType type, Equipment equipment);
    }
    
    public class Equipment : MonoBehaviour
    {
        [SerializeField] private EquipmentType type;
        [SerializeField] private Stats stats;
        [SerializeField] private SkillType skill;

        public EquipmentType Type => type;
        public Stats Stats => stats;
        public SkillType Skill => skill;
        
        public IEquipable Equipper { get; set; }
    }
}
