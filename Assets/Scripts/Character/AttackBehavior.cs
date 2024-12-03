using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character
{
    [RequireComponent(typeof(Animator))]
    public class AttackBehavior : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D hitBox;

        public float CooldownTime { get; set; } = 1;

        private Animator m_animator;
        private static readonly int Attack = Animator.StringToHash("Attack");
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (hitBox != null)
                hitBox.enabled = false;

            m_animator = GetComponent<Animator>();
            Assert.IsNotNull(m_animator);

            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            while (true)
            {
                yield return new WaitForSeconds(CooldownTime);
                m_animator.SetTrigger(Attack);
            }
        }

        private void OnHitBoxEnable()
        {
            if (hitBox != null)
                hitBox.enabled = true;
        }

        private void OnHitBoxDisable()
        {
            if (hitBox != null)
                hitBox.enabled = false;
        }
    }
}
