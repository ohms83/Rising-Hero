using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pattern
{
    public abstract class EventBus<T> : ScriptableObject
    {
        [Tooltip("Register to this call back to listening events broadcasting from this event bus")]
        public UnityAction<T> onEventRaised;

        public void Broadcast(T param)
        {
            onEventRaised?.Invoke(param);
        }
    }

    public abstract class EventBusListener<TEventChannel, TEventParam> : MonoBehaviour
        where TEventChannel : EventBus<TEventParam>
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
