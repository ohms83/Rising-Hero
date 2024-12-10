using System.Collections;
using UnityEngine;

namespace Character.Behaviour
{
    public class AttackBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public float CooldownTime { get; set; } = 1;
        
        private static readonly int AttackTrigger = Animator.StringToHash("Attack");
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (animator != null)
                StartCoroutine(Cooldown());
            else
                Debug.LogWarning("animator parameter isn't assigned!");
        }

        private IEnumerator Cooldown()
        {
            while (true)
            {
                yield return new WaitForSeconds(CooldownTime);
                animator.SetTrigger(AttackTrigger);
            }
        }
    }
}
