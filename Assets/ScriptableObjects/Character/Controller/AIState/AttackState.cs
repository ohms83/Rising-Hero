using System;
using Character;
using Character.Controller;
using Pattern;
using Skills;
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

        // ReSharper disable Unity.PerformanceAnalysis
        public override void OnEnter(IStateMachineOwner<AIStateEnum> owner)
        {
            var ownerAI = (AIController)owner;
            Assert.IsNotNull(ownerAI);
            
            // ownerAI.ControlledCharacter.CharacterAnimation.Attack();
            var skill = ownerAI.ControlledCharacter.SkillSystem.ActivateSkill(SkillType.MeleeAttack);
            if (skill == null)
                return;
            skill.onFinishedCooldown += @base =>
            {
                var aiController = (AIController)@base.Owner.Controller;
                if (aiController == null)
                    return;

                aiController.StateMachine.ChangeState(AIStateEnum.Idle);
            };
        }

        public override void OnExit(IStateMachineOwner<AIStateEnum> owner)
        {
        }

        public override void OnUpdate(IStateMachineOwner<AIStateEnum> owner)
        {
        }
    }
}