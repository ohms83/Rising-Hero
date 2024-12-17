using System;
using UnityEngine;

namespace Pattern
{
    public class Singleton<T> where T : new()
    {
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new T();
                return s_instance;
            }
        }

        private static T s_instance;
    }

    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;
                
                var gameObject = new GameObject(typeof(T).Name);
                s_instance = gameObject.AddComponent<T>();
                return s_instance;
            }
        }

        private void Start()
        {
            if (s_instance == null)
                s_instance = GetComponent<T>();

            OnStart();
        }
        
        protected virtual void OnStart() {}
    }
}
