using Character;
using ScriptableObjects.Character;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Gameplay.Equipment
{
    public class MeleeWeapon : Equipment
    {
        [SerializeField] private BoxCollider2D hitBox;

        private GameCharacterData m_ownerCharacterData;
        private UnityAction<CharacterAnimation, AnimationEvent> m_hitBoxAnimationEvent;

        private void OnDestroy()
        {
            // Unbind animation event
            BindHitBoxAnimationEvent((GameCharacter)Owner, false);
        }

        private void EnableHitBox(bool isEnabled)
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

            // Attach to the owner
            transform.parent = ownerCharacter.CharacterAnimation.transform;
            
            m_ownerCharacterData = ownerCharacter.SharedData;
            m_hitBoxAnimationEvent = ownerCharacter.CharacterAnimation.hitBoxAnimationEvent;
            BindHitBoxAnimationEvent(ownerCharacter, true);
        }

        protected override void OnOwnerUnset(IEquipable currentOwner)
        {
            base.OnOwnerUnset(currentOwner);
            
            var ownerCharacter = (GameCharacter)currentOwner;
            if (ownerCharacter == null)
                return;
            
            transform.parent = null;
            
            m_ownerCharacterData = null;
            m_hitBoxAnimationEvent = null;
            BindHitBoxAnimationEvent(ownerCharacter, false);
        }

        private void BindHitBoxAnimationEvent(GameCharacter owner, bool isBind)
        {
            if (isBind)
                owner.CharacterAnimation.hitBoxAnimationEvent += OnHitBoxAnimationEvent;
            else
                owner.CharacterAnimation.hitBoxAnimationEvent -= OnHitBoxAnimationEvent;
        }

        private void OnHitBoxAnimationEvent(CharacterAnimation sender, AnimationEvent eventArg)
        {
            var isEnabled = eventArg.intParameter > 0;

            if (isEnabled)
            {
                var attackData =
                    m_ownerCharacterData.
                        attackData.Find(data => data.animationClip == eventArg.animatorClipInfo.clip);
                hitBox.offset = new Vector2(attackData.hitBox.x, attackData.hitBox.y);
                hitBox.size = new Vector2(attackData.hitBox.width, attackData.hitBox.height);
            }

            hitBox.enabled = isEnabled;
        }
    }
}