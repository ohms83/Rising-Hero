using Character.Controller;
using Pattern;
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
            ownerAI.ControlledCharacter.Movement.MoveVector = Vector2.zero;
        }

        public override void OnUpdate(IStateMachineOwner<AIStateEnum> owner)
        {
            var ownerAI = (AIController)owner;
            var targetCharacter = ownerAI.PlayerCharacter;
            var targetVector = targetCharacter.transform.position - ownerAI.transform.position;
            var attackRangeSqr = attackRange * attackRange;
            if (targetVector.sqrMagnitude < attackRangeSqr)
            {
                owner.StateMachine.ChangeState(AIStateEnum.Attack);
                return;
            }

            ownerAI.ControlledCharacter.Movement.MoveVector = targetVector.normalized;
        }
    }
}