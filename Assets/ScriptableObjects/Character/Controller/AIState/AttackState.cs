using System;
using Character;
using Character.Controller;
using Pattern;
using UnityEngine;
using UnityEngine.Assertions;

namespace ScriptableObjects.Character.Controller.AIState
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "Scriptable Objects/Character/Controller/AIState/AttackState")]
    public class AttackState : AIState
    {
        private void Awake()
        {
            StateEnum = AIStateEnum.Attack;
        }

        public override void OnEnter(IStateMachineOwner<AIStateEnum> owner)
        {
            var ownerAI = (AIController)owner;
            Assert.IsNotNull(ownerAI);
            
            ownerAI.ControlledCharacter.CharacterAnimation.Attack();
        }

        public override void OnExit(IStateMachineOwner<AIStateEnum> owner)
        {
        }

        public override void OnUpdate(IStateMachineOwner<AIStateEnum> owner)
        {
            var ownerAI = (AIController)owner;
            Assert.IsNotNull(ownerAI);
            
            var animator = ownerAI.ControlledCharacter.CharacterAnimation;
            if (animator.IsAnimState(CharacterAnimation.AttackState))
                return;
            
            owner.StateMachine.ChangeState(AIStateEnum.Idle);
        }
    }
}