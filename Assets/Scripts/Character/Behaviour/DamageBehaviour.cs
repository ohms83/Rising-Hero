using System;
using DG.Tweening;
using Effect;
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
        }
    }
}
