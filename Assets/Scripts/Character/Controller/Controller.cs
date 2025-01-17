using UnityEngine;

namespace Character.Controller
{
    public class ControllerBase : MonoBehaviour
    {
        [SerializeField] private GameCharacter m_controlledCharacter;
        
        public GameCharacter ControlledCharacter
        {
            get => m_controlledCharacter;
            set
            {
                if (m_controlledCharacter != null)
                    m_controlledCharacter.onCharacterDeath.RemoveListener(OnCharacterDeath);
                
                value.onCharacterDeath.AddListener(OnCharacterDeath);
                m_controlledCharacter = value;
            }
        }

        protected virtual void Awake()
        {
            if (m_controlledCharacter == null)
                return;
            ControlledCharacter = m_controlledCharacter;
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void OnCharacterDeath(GameCharacter controlledCharacter)
        {
            enabled = false;
        }
    }
}
