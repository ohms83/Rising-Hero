using System.Collections.Generic;
using Pattern;
using ScriptableObjects.Character.Controller.AIState;
using ScriptableObjects.Event;
using Unity.VisualScripting;
using UnityEngine;

namespace Character.Controller
{
    public class AIController : ControllerBase, IStateMachineOwner<AIStateEnum>
    {
        [SerializeField] private GameCharacter playerCharacter;
        public GameCharacter PlayerCharacter
        {
            get => playerCharacter;
            set
            {
                if (playerCharacter != null)
                    playerCharacter.onCharacterDeath.RemoveListener(OnPlayerDeath);

                if (value != null)
                {
                    value.onCharacterDeath.AddListener(OnPlayerDeath);
                    StateMachine?.ChangeState(defaultState);
                }
                
                playerCharacter = value;
            }
        }

        #region State Machine

        [Tooltip("A list of AI states that will be registered to this AI")]
        [SerializeField] private List<AIState> states = new ();
        [SerializeField] private AIStateEnum defaultState = AIStateEnum.Idle;
        public StateMachine<AIStateEnum> StateMachine { get; private set; }
        
        #endregion
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine<AIStateEnum>(this);
            StateMachine.AddStates(states);
            StateMachine.ChangeState(defaultState);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (playerCharacter == null || playerCharacter.IsDestroyed())
            {
                OnPlayerDeath(playerCharacter);
            }
            else
            {
                playerCharacter.onCharacterDeath.AddListener(OnPlayerDeath);
                StateMachine?.ChangeState(defaultState);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StateMachine?.Stop();

            if (playerCharacter != null)
                playerCharacter.onCharacterDeath.RemoveListener(OnPlayerDeath);
        }

        // Update is called once per frame
        private void Update()
        {
            StateMachine.Update();
        }

        private void OnPlayerDeath(GameCharacter player)
        {
            PlayerCharacter = null;
            StateMachine.Stop();
        }
    }
}
