using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Pattern
{
    [Serializable]
    public class ValueEvent<T>
    {
        [SerializeField] private T m_value;

        public T Value
        {
            get => m_value;
            set
            {
                if (!m_value.Equals(value))
                {
                    onValueChanged?.Invoke(m_value, value);
                }
                m_value = value;
            }
        }
        
        public UnityEvent<T, T> onValueChanged;

        public ValueEvent()
        {
        }
        public ValueEvent(T value)
        {
            Value = value;
        }
    }
    public abstract class Event<T> : ScriptableObject
    {
        private UnityAction<T> m_onEventRaised;

        public virtual void Broadcast(T param)
        {
            m_onEventRaised?.Invoke(param);
        }

        public void AddListener(UnityAction<T> listener)
        {
            m_onEventRaised += listener;
        }

        public void RemoveListener(UnityAction<T> listener)
        {
            m_onEventRaised -= listener;
        }
    }
    public abstract class Event<T1, T2> : ScriptableObject
    {
        private UnityAction<T1, T2> m_onEventRaised;

        public void Broadcast(T1 param0, T2 param1)
        {
            m_onEventRaised?.Invoke(param0, param1);
        }

        public void AddListener(UnityAction<T1, T2> listener)
        {
            m_onEventRaised += listener;
        }

        public void RemoveListener(UnityAction<T1, T2> listener)
        {
            m_onEventRaised -= listener;
        }
    }
}
