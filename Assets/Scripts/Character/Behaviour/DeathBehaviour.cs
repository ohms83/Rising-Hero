using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character.Behaviour
{
    public class DeathBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterAnimation characterAnimation;
        [SerializeField] private bool destroyOnDeath = true;

        public bool IsDeathSequenceStarted
        {
            get;
            private set;
        }
        private void Start()
        {
            IsDeathSequenceStarted = false;
            enabled = false;
            Assert.IsNotNull(characterAnimation);
        }

        public void BeginDeathSequence()
        {
            IsDeathSequenceStarted = true;
            characterAnimation.Death();
            if (destroyOnDeath)
                Destroy(gameObject, 1.0f);
        }
    }
}
