using Character.Controller;
using Pattern;
using UnityEngine;
using UnityEngine.Assertions;

namespace ScriptableObjects.Character.Controller.AIState
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "Scriptable Objects/Character/Controller/AIState/IdleState")]
    public class IdleState : AIState
    {
        [Tooltip("The AI will enter aggro state when the target is within this range.")]
        [SerializeField] private float aggroDistance = 10;
        // public float MinIdleTime = 0.5f;
        // public float MaxIdleTime = 4f;

        private void Awake()
        {
            StateEnum = AIStateEnum.Idle;
        }

        public override void OnEnter(IStateMachineOwner<AIStateEnum> owner)
        {
        }

        public override void OnExit(IStateMachineOwner<AIStateEnum> owner)
        {
        }

        public override void OnUpdate(IStateMachineOwner<AIStateEnum> owner)
        {
            var aiController = (AIController)owner;
            Assert.IsNotNull(aiController);
            
            var aggroDistanceSqr = aggroDistance * aggroDistance;
            var sqrDistance =
                (aiController.PlayerCharacter.transform.position - aiController.transform.position).sqrMagnitude;  
            if (sqrDistance < aggroDistanceSqr)
            {
                owner.StateMachine.ChangeState(AIStateEnum.Aggro);
            }
        }
    }
}