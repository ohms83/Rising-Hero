using System;
using Pattern;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character.Controller.AI
{
    public class IdleState : AIAbstractState
    {
        public float AggroDistance
        {
            set => m_aggroDistanceSqr = value * value;
        }

        private float m_aggroDistanceSqr = 0;
        // public float MinIdleTime = 0.5f;
        // public float MaxIdleTime = 4f;

        public override void OnUpdate()
        {
            var sqrDistance =
                (TargetCharacter.transform.position - AIOwner.transform.position).sqrMagnitude;  
            if (sqrDistance < m_aggroDistanceSqr)
            {
                Owner.StateMachine.ChangeState(AIStateEnum.Aggro);
            }
        }
    }
}