using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pattern
{
    [Serializable]
    public class ValueEvent<T>
    {
        [SerializeField] private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    onValueChanged?.Invoke(_value, value);
                }
                _value = value;
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
        [Tooltip("Register to this call back to listening events broadcasting from this event bus")]
        public UnityAction<T> onEventRaised;

        public void Broadcast(T param)
        {
            onEventRaised?.Invoke(param);
        }
    }
    public abstract class Event<T1, T2> : ScriptableObject
    {
        [Tooltip("Register to this call back to listening events broadcasting from this event bus")]
        public UnityAction<T1, T2> onEventRaised;

        public void Broadcast(T1 param0, T2 param1)
        {
            onEventRaised?.Invoke(param0, param1);
        }
    }

    public abstract class EventBusListener<TEventChannel, TEventParam> : MonoBehaviour
        where TEventChannel : Event<TEventParam>
    {
        [SerializeField] protected TEventChannel eventSource;
        [SerializeField] protected UnityEvent<TEventParam> eventHandlers;

        protected void OnEnable()
        {
            if (eventSource != null)
            {
                eventSource.onEventRaised += HandleEvent;
            }
        }
        protected void OnDisable()
        {
            if (eventSource != null)
            {
                eventSource.onEventRaised -= HandleEvent;
            }
        }

        private void HandleEvent(TEventParam param)
        {
            eventHandlers?.Invoke(param);
        }
    }
}
