using System;
using Character;
using UnityEngine;

namespace Gameplay.Equipment
{
    public class Weapon : Equipment
    {
        [SerializeField] private BoxCollider2D hitBox;

        private void OnDestroy()
        {
            // Unbind animation event
            BindHitBoxAnimationEvent((GameCharacter)Owner, false);
        }

        public int GetDamage()
        {
            return Owner != null ? Owner.CombinedStats.Attack : 0;
        }

        public void EnableHitBox(bool isEnabled)
        {
            if (hitBox)
                hitBox.enabled = isEnabled;
        }

        protected override void OnOwnerSet(IEquipable newOwner)
        {
            base.OnOwnerSet(newOwner);
            
            var ownerCharacter = (GameCharacter)newOwner;
            if (ownerCharacter == null)
                return;
            
            transform.parent = ownerCharacter.CharacterAnimation.transform;
            BindHitBoxAnimationEvent(ownerCharacter, true);
        }

        protected override void OnOwnerUnset(IEquipable currentOwner)
        {
            base.OnOwnerUnset(currentOwner);
            
            var ownerCharacter = (GameCharacter)currentOwner;
            if (ownerCharacter == null)
                return;
            
            transform.parent = null;
            BindHitBoxAnimationEvent(ownerCharacter, false);
        }

        private void BindHitBoxAnimationEvent(GameCharacter owner, bool isBind)
        {
            if (owner == null || owner.CharacterAnimation == null)
                return;

            if (isBind)
                owner.CharacterAnimation.HitBoxAnimationEvent += OnHitBoxAnimationEvent;
            else
                owner.CharacterAnimation.HitBoxAnimationEvent -= OnHitBoxAnimationEvent;
        }

        private void OnHitBoxAnimationEvent(CharacterAnimation sender, bool isEnabled)
        {
            EnableHitBox(isEnabled);
        }
    }
}