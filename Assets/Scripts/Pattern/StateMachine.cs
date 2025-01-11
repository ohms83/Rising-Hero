using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Pattern
{
    public interface IStateMachineOwner<TEnum> where TEnum : Enum
    {
        public StateMachine<TEnum> StateMachine
        {
            get;
        }
    }
    
    public abstract class AbstractState<TEnum> : ScriptableObject where TEnum : Enum
    {
        public TEnum StateEnum
        {
            get => stateEnum;
            protected set => stateEnum = value;
        }
        [SerializeField] private TEnum stateEnum;
        
        public abstract void OnEnter(IStateMachineOwner<TEnum> owner);
        public abstract void OnExit(IStateMachineOwner<TEnum> owner);
        public abstract void OnUpdate(IStateMachineOwner<TEnum> owner);
    }
    
    public class StateMachine<TEnum> where TEnum : Enum
    {
        private readonly Dictionary<TEnum, AbstractState<TEnum>> m_states = new ();
        private AbstractState<TEnum> m_nextState;
        private bool m_needChangeState;
        
        private bool m_strictCheck = true;
        /// <summary>
        /// If set, it will raise an error when trying to register a state with the same StateEnum.
        /// </summary>
        public bool StrictCheck
        {
            set => m_strictCheck = value;
        }

        // Null check on a Unity.Object is expensive. This delegate is introduced
        // to avoid performance bottlenecks.
        private Action<IStateMachineOwner<TEnum>> UpdateFunc
        {
            get;
            set;
        }

        private IStateMachineOwner<TEnum> Owner
        {
            get;
        }

        public AbstractState<TEnum> NextState
        {
            get => m_nextState;
            private set => m_nextState = value;
        }

        public AbstractState<TEnum> CurrentState { get; private set; }

        public StateMachine(IStateMachineOwner<TEnum> owner)
        {
            Owner = owner;
            NextState = null;
            CurrentState = null;
        }

        public void AddState(AbstractState<TEnum> state)
        {
            var containsKey = m_states.ContainsKey(state.StateEnum);
            Assert.IsFalse(m_strictCheck && containsKey,
                $"State {state.StateEnum} is already registered!");
            m_states.Add(state.StateEnum, state);
        }

        public void AddStates(IEnumerable<AbstractState<TEnum>> states)
        {
            foreach (var state in states)
            {
                AddState(state);
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ChangeState(TEnum toState)
        {
            if (m_states.TryGetValue(toState, out m_nextState))
            {
                m_needChangeState = true;
                return;
            }

            m_nextState = null;
            // ReSharper disable once Unity.PerformanceCriticalCodeInvocation
            Debug.LogWarning($"The state {toState.ToString()} is not registered to {Owner.GetType()}");
        }

        [SuppressMessage("ReSharper", "Unity.PerformanceCriticalCodeNullComparison")]
        private void StateTransition()
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit(Owner);
                UpdateFunc -= CurrentState.OnUpdate;
            }

            CurrentState = NextState;
            CurrentState.OnEnter(Owner);
            UpdateFunc += CurrentState.OnUpdate;
            
            NextState = null;
            m_needChangeState = false;
        }

        public void Update()
        {
            if (m_needChangeState)
            {
                StateTransition();
            }
            UpdateFunc?.Invoke(Owner);
        }

        /// <summary>
        /// Stop the state machine. To resume, please call ChangeState function. 
        /// </summary>
        public void Stop()
        {
            NextState = null;
            CurrentState = null;
            m_needChangeState = false;
        }
    }
}