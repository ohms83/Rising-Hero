using System;
using Pattern;
using UnityEngine.Assertions;

namespace Character.Controller.AI
{
    public class AttackState : AIAbstractState
    {
        private CharacterAnimation m_animation;
        public override void OnRegistered(IStateMachineOwner owner, Enum stateEnum)
        {
            base.OnRegistered(owner, stateEnum);
            
            m_animation = AIOwner.ControlledCharacter.CharacterAnimation;
            Assert.IsNotNull(m_animation);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            m_animation.Attack();
        }
        
        public override void OnUpdate()
        {
            if (m_animation.IsAnimState(CharacterAnimation.AttackState))
                return;
            
            Owner.StateMachine.ChangeState(AIStateEnum.Idle);
        }
    }
}