using DG.Tweening;
using Effect.Battle;
using UnityEngine;

namespace Character.Behaviour
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class DamageBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer characterSprite;
        [SerializeField] private DamageFlash damageFlash;
        [SerializeField] private Vector3 shakeStrength;
        [SerializeField] private float shakeDuration = 0.5f;
        [SerializeField] private int vibrato = 20;
        [SerializeField] private DamageNumberPool damageNumberPool;
        [SerializeField] private Transform damageNumberRoot;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (damageFlash != null)
                damageFlash.FlashSprite();

            if (characterSprite != null)
            {
                DOTween.Shake(
                    () => characterSprite.transform.localPosition,
                    value => characterSprite.transform.localPosition = value,
                    shakeDuration,
                    shakeStrength,
                    vibrato);
            }

            if (damageNumberPool != null)
            {
                var damageNumber = damageNumberPool.ObjectPool.Get();
                
                if (damageNumberRoot)
                    damageNumber.transform.parent = damageNumberRoot;
                damageNumber.transform.localPosition = Vector3.zero;
            }
        }
    }
}
