using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pattern
{
    public interface IStateMachineOwner
    {
        public StateMachine StateMachine
        {
            get;
        }
    }
    
    public abstract class AbstractState
    {
        protected IStateMachineOwner Owner { get; private set; }
        public Enum StateEnum { get; private set; }

        public virtual void OnRegistered(IStateMachineOwner owner, Enum stateEnum)
        {
            Owner = owner;
            StateEnum = stateEnum;
        }
        
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate();
    }
    
    public class StateMachine
    {
        private readonly Dictionary<Enum, AbstractState> m_states = new ();
        private AbstractState m_nextState;

        private IStateMachineOwner Owner
        {
            get;
        }

        public AbstractState NextState
        {
            get => m_nextState;
            private set => m_nextState = value;
        }

        public AbstractState CurrentState { get; private set; }

        public StateMachine(IStateMachineOwner owner)
        {
            Owner = owner;
            NextState = null;
            CurrentState = null;
        }

        public void AddState(Enum stateEnum, AbstractState state)
        {
            state.OnRegistered(Owner, stateEnum);
            m_states.Add(stateEnum, state);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ChangeState(Enum toState)
        {
            if (m_states.TryGetValue(toState, out m_nextState))
                return;
            
            m_nextState = null;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Debug.LogWarning($"The state {toState.ToString()} is not registered to {Owner.GetType()}");
        }

        private void StateTransition()
        {
            CurrentState?.OnExit();
            CurrentState = NextState;
            CurrentState.OnEnter();
            NextState = null;
        }

        public void Update()
        {
            if (NextState != null)
            {
                StateTransition();
            }
            CurrentState?.OnUpdate();
        }

        /// <summary>
        /// Stop the state machine. To resume, please call ChangeState function. 
        /// </summary>
        public void Stop()
        {
            NextState = null;
            CurrentState = null;
        }
    }
}