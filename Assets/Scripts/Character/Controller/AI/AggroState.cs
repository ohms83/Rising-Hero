using System;
using Character.Behaviour;
using Pattern;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character.Controller.AI
{
    /// <summary>
    /// The AI will follow the target character until it reaches the desired distant before
    /// start attacking
    /// </summary>
    public class AggroState : AIAbstractState
    {
        private Movement m_movement;
        /// <summary>
        /// The shortest distance between the AI character and the target before start attacking.
        /// </summary>
        public float DesiredDistance
        {
            set => m_desiredDistanceSqr = value * value;
        }
        private float m_desiredDistanceSqr;

        public override void OnRegistered(IStateMachineOwner owner, Enum stateEnum)
        {
            base.OnRegistered(owner, stateEnum);
            
            m_movement = AIOwner.GetComponent<Movement>();
            Assert.IsNotNull(m_movement);
        }

        public override void OnExit()
        {
            base.OnExit();
            m_movement.MoveVector = Vector2.zero;
        }

        public override void OnUpdate()
        {
            var targetVector = TargetCharacter.transform.position - AIOwner.transform.position;
            if (targetVector.sqrMagnitude < m_desiredDistanceSqr)
            {
                Owner.StateMachine.ChangeState(AIStateEnum.Attack);
            }

            m_movement.MoveVector = targetVector.normalized;
        }
    }
}