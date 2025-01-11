using System.Collections.Generic;
using Pattern;
using ScriptableObjects.Character.Controller.AIState;
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
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected void Awake()
        {
            StateMachine = new StateMachine<AIStateEnum>(this); 
        }

        protected override void Start()
        {
            base.Start();
            
            StateMachine.AddStates(states);
            StateMachine.ChangeState(defaultState);
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
    }
}
