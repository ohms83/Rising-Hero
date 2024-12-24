using System;
using UnityEngine;

namespace Test.Scripts
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TestTrigger : MonoBehaviour
    {
        [SerializeField] private Color activeColor = Color.yellow;
        [SerializeField] private Color inactiveColor = Color.white;
        private SpriteRenderer m_spriteRenderer;

        private void Start()
        {
            m_spriteRenderer = GetComponent<SpriteRenderer>();
            m_spriteRenderer.color = inactiveColor;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            m_spriteRenderer.color = activeColor;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            m_spriteRenderer.color = inactiveColor;
        }
    }
}
