using System.Collections;
using UnityEngine;

namespace Effect.Battle
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DamageFlash : MonoBehaviour
    {
        [SerializeField] private AnimationCurve tweenCurve;

        private SpriteRenderer m_renderer;

        private static readonly int TintPower = Shader.PropertyToID("_TintPower");

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            m_renderer = GetComponent<SpriteRenderer>();
        }

        private IEnumerator UpdateFlash()
        {
            float time = 0;
            while (true)
            {
                time += Time.deltaTime;
                if (tweenCurve[tweenCurve.length - 1].time <= time)
                    break;
                
                var flashPower = tweenCurve.Evaluate(time);
                m_renderer.material.SetFloat(TintPower, flashPower);
                yield return 0;
            }
            
            m_renderer.material.SetFloat(TintPower, 0);
            yield break;
        }

        public void FlashSprite()
        {
            if (tweenCurve == null || tweenCurve.length == 0)
            {
                Debug.LogWarning("tweenCurve is invalid.");
                return;
            }
            
            StopCoroutine(nameof(UpdateFlash));
            StartCoroutine(UpdateFlash());
        }
    }
}
