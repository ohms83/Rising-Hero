using System;
using Pattern;
using UnityEngine.Assertions;

namespace Character.Controller.AI
{
    public abstract class AIAbstractState : AbstractState
    {
        protected AIController AIOwner
        {
            get;
            private set;
        }
        protected GameCharacter TargetCharacter
        {
            get;
            private set;
        }
        public override void OnRegistered(IStateMachineOwner owner, Enum stateEnum)
        {
            base.OnRegistered(owner, stateEnum);
            
            AIOwner = (AIController)Owner;
            Assert.IsNotNull(AIOwner);
        }

        public override void OnEnter()
        {
            TargetCharacter = AIOwner.PlayerCharacter;
            Assert.IsNotNull(TargetCharacter);
        }
        
        public override void OnExit()
        {
            TargetCharacter = null;
        }
    }
}