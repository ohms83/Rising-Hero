using DG.Tweening;
using Effect.Battle;
using Gameplay.Battle;
using Gameplay.Equipment;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character.Behaviour
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DamageBehaviour : MonoBehaviour
    {
        [SerializeField] private GameCharacter ownerCharacter;
        [SerializeField] private DamageFlash damageFlash;
        [SerializeField] private Vector3 shakeStrength;
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private int vibrato = 20;
        [SerializeField] private Transform damageNumberRoot;

        private CharacterAnimation m_characterAnimation;
        private BoxCollider2D m_hurtBox;

        private void Start()
        {
            Assert.IsNotNull(ownerCharacter);
            m_characterAnimation = ownerCharacter.CharacterAnimation;

            m_hurtBox = GetComponent<BoxCollider2D>();
            Assert.IsNotNull(m_hurtBox);
            m_hurtBox.excludeLayers |= 1 << ownerCharacter.gameObject.layer;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (damageFlash != null)
                damageFlash.FlashSprite();

            if (m_characterAnimation != null)
            {
                DOTween.Shake(
                    () => m_characterAnimation.transform.localPosition,
                    value => m_characterAnimation.transform.localPosition = value,
                    shakeDuration,
                    shakeStrength,
                    vibrato);
            }
            
            TakeDamage(other.GetComponent<Equipment>());
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log($"Contact count = {other.contactCount}");
        }

        private void TakeDamage(Equipment weapon)
        { 
            if (weapon == null || damageNumberRoot == null)
                return;
            
            var damage = DamageCalculator.CalculateDamage(weapon.CombinedStats, ownerCharacter.CombinedStats);
            var damageNumber = DamageNumberPool.Instance.ObjectPool.Get();
            var damageNumberTransform = damageNumber.transform;
            damageNumberTransform.parent = damageNumberRoot;
            damageNumberTransform.localPosition = Vector3.zero;
            damageNumber.SetDamage(damage);
            
            ownerCharacter.TakeDamage(damage);
            m_hurtBox.enabled = !ownerCharacter.Stats.IsDeath;
        }
    }
}
