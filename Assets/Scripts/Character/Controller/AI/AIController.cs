using Pattern;
using UnityEngine;

namespace Character.Controller.AI
{
    public class AIController : ControllerBase, IStateMachineOwner
    {
        [SerializeField] private GameCharacter m_playerCharacter;
        public GameCharacter PlayerCharacter
        {
            get => m_playerCharacter;
            set => m_playerCharacter = value;
        }

        #region State Machine
        
        public StateMachine StateMachine { get; private set; }
        
        #endregion
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            
            StateMachine = new StateMachine(this);

            StateMachine.AddState(AIStateEnum.Idle, new IdleState {
                AggroDistance = 4
            });
            StateMachine.AddState(AIStateEnum.Aggro, new AggroState {
                DesiredDistance = 1
            });
            StateMachine.AddState(AIStateEnum.Attack, new AttackState());
            
            StateMachine.ChangeState(AIStateEnum.Idle);
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
