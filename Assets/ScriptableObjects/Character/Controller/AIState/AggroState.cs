using Character.Controller;
using Pattern;
using Unity.VisualScripting;
using UnityEngine;

namespace ScriptableObjects.Character.Controller.AIState
{
    /// <summary>
    /// The AI will follow the target character until it reaches the desired distant before
    /// start attacking
    /// </summary>
    [CreateAssetMenu(fileName = "AggroState", menuName = "Scriptable Objects/Character/Controller/AIState/AggroState")]
    public class AggroState : AIState
    {
        [Tooltip("Enemy's attack range.")]
        [SerializeField] private float attackRange = 2;

        private void Awake()
        {
            StateEnum = AIStateEnum.Aggro;
        }

        public override void OnEnter(IStateMachineOwner<AIStateEnum> owner)
        {
        }

        public override void OnExit(IStateMachineOwner<AIStateEnum> owner)
        {
            var ownerAI = (AIController)owner;
            
            if (!ownerAI.ControlledCharacter.IsDestroyed())
                ownerAI.ControlledCharacter.Movement.MoveVector = Vector2.zero;
        }

        public override void OnUpdate(IStateMachineOwner<AIStateEnum> owner)
        {
            var ownerAI = (AIController)owner;
            var controlledCharacter = ownerAI.ControlledCharacter;
            var targetCharacter = ownerAI.PlayerCharacter;
            var targetVector = targetCharacter.transform.position - controlledCharacter.transform.position;
            var attackRangeSqr = attackRange * attackRange;
            if (targetVector.sqrMagnitude < attackRangeSqr)
            {
                owner.StateMachine.ChangeState(AIStateEnum.Attack);
                return;
            }

            controlledCharacter.Movement.MoveVector = targetVector.normalized;
        }
    }
}