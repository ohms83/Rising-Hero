using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Effect.Battle
{
    [RequireComponent(typeof(Animation))]
    public class DamageNumber : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text; 
        public delegate void DamageNumberDelegate(DamageNumber damageNumber);
        public DamageNumberDelegate OnFinishedAnimate;
        
        private Animation m_animation;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (m_animation.isPlaying)
                return;
            
            OnFinishedAnimate?.Invoke(this);
            Stop();
        }

        public void Init()
        {
            m_animation = GetComponent<Animation>();
            Assert.IsNotNull(m_animation);
            m_animation.wrapMode = WrapMode.Once;
        }

        public void Animate()
        {
            m_animation.Stop();
            m_animation.Play();
            enabled = true;
        }

        public void Stop()
        {
            m_animation.Stop();
            enabled = false;
        }

        public void SetDamage(int damage)
        {
            text.SetText($"{damage}");
        }
    }
}
