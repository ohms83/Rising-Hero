using System;
using JetBrains.Annotations;
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
        public void Equip(Equipment equipment);
    }
    
    public class Equipment : MonoBehaviour
    {
        [SerializeField] private EquipmentType type;
        [SerializeField] private Stats stats;
        [SerializeField] private SkillType skill;

        public EquipmentType Type => type;
        public Stats Stats => stats;
        public SkillType Skill => skill;

        private IEquipable m_owner;
        public IEquipable Owner
        {
            get => m_owner; set => SetOwner(value);
        }

        private void SetOwner(IEquipable owner)
        {
            if (owner != null)
                OnOwnerSet(owner);
            else if (m_owner != null)
                OnOwnerUnset(m_owner);
            
            m_owner = owner;
        }

        /// <summary>
        /// Call before the owner being assigned
        /// </summary>
        /// <param name="newOwner">The new owner that's going to equip this equipment</param>
        protected virtual void OnOwnerSet([NotNull] IEquipable newOwner)
        {
        }

        /// <summary>
        /// Call before the owner being removed
        /// </summary>
        /// <param name="currentOwner">The current owner that's equipping this equipment</param>
        protected virtual void OnOwnerUnset([NotNull] IEquipable currentOwner)
        {
            var ownerObjcet = (MonoBehaviour)currentOwner;
            if (ownerObjcet != null)
                transform.parent = null;
        }

        public Stats CombinedStats => Owner?.CombinedStats ?? Stats;
    }
}
