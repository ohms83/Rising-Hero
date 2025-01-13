using System.Collections.Generic;
using Pattern;
using ScriptableObjects.Character.Controller.AIState;
using ScriptableObjects.Event;
using UnityEngine;

namespace Character.Controller
{
    public class AIController : ControllerBase, IStateMachineOwner<AIStateEnum>
    {
        [SerializeField] private GameCharacter playerCharacter;
        public GameCharacter PlayerCharacter
        {
            get => playerCharacter;
            set => playerCharacter = value;
        }

        #region State Machine

        [Tooltip("A list of AI states that will be registered to this AI")]
        [SerializeField] private List<AIState> states = new ();
        [SerializeField] private AIStateEnum defaultState = AIStateEnum.Idle;
        public StateMachine<AIStateEnum> StateMachine { get; private set; }
        
        #endregion
        
        [SerializeField] private CharacterEvent playerDeathEvent;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine<AIStateEnum>(this);
            StateMachine.AddStates(states);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            StateMachine.ChangeState(defaultState);

            if (playerDeathEvent != null)
                playerDeathEvent.onEventRaised += OnPlayerDeath;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            StateMachine.Stop();

            if (playerDeathEvent != null)
                playerDeathEvent.onEventRaised -= OnPlayerDeath;
        }

        // Update is called once per frame
        private void Update()
        {
            if (ControlledCharacter.Stats.IsDeath)
            {
                StateMachine.Stop();
                return;
            }
            StateMachine.Update();
        }

        private void OnPlayerDeath(GameCharacter player)
        {
            PlayerCharacter = null;
            StateMachine.Stop();
        }
    }
}
