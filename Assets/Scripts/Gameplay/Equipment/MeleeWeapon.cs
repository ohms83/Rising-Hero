using Character;
using Character.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Equipment
{
    public class MeleeWeapon : Equipment
    {
        [SerializeField] private BoxCollider2D m_hitBox;

        private GameCharacterData m_ownerCharacterData;
        private UnityAction<CharacterAnimation, AnimationEvent> m_hitBoxAnimationEvent;

        protected void Start()
        {
            EnableHitBox(false);
        }

        private void OnEnable()
        {
            BindHitBoxAnimationEvent((GameCharacter)Owner, true);
        }

        private void OnDisable()
        {
            BindHitBoxAnimationEvent((GameCharacter)Owner, false);
        }

        private void EnableHitBox(bool isEnabled)
        {
            if (m_hitBox)
                m_hitBox.enabled = isEnabled;
        }

        protected override void OnOwnerSet(IEquipable newOwner)
        {
            base.OnOwnerSet(newOwner);
            
            var ownerCharacter = (GameCharacter)newOwner;
            if (ownerCharacter == null)
                return;

            // Attach to the owner
            var tmpTransform = transform;
            tmpTransform.parent = ownerCharacter.CharacterAnimation.transform;
            tmpTransform.localPosition = Vector3.zero;

            // Setup hit box
            m_hitBox.excludeLayers |= 1 << ownerCharacter.gameObject.layer;
            m_hitBox.enabled = false;
            
            m_ownerCharacterData = ownerCharacter.SharedData;
            BindHitBoxAnimationEvent(ownerCharacter, true);
        }

        protected override void OnOwnerUnset(IEquipable currentOwner)
        {
            base.OnOwnerUnset(currentOwner);
            
            var ownerCharacter = (GameCharacter)currentOwner;
            if (ownerCharacter == null)
                return;
            
            transform.parent = null;
            
            m_hitBox.excludeLayers ^= 1 << ownerCharacter.gameObject.layer;
            m_hitBox.enabled = false;
            
            m_ownerCharacterData = null;
            BindHitBoxAnimationEvent(ownerCharacter, false);
        }

        private void BindHitBoxAnimationEvent(GameCharacter owner, bool isBind)
        {
            if (owner == null)
                return;
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
                m_hitBox.transform.localPosition = new Vector2(attackData.hitBox.x, attackData.hitBox.y);
                m_hitBox.size = new Vector2(attackData.hitBox.width, attackData.hitBox.height);
            }

            m_hitBox.enabled = isEnabled;
        }
    }
}