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
        private static MonoSingleton<T> s_instance;

        public static MonoSingleton<T> Instance
        {
            get
            {
                if (s_instance != null)
                    return s_instance;
                
                var gameObject = new GameObject(nameof(T));
                s_instance = gameObject.AddComponent<MonoSingleton<T>>();
                return s_instance;
            }
        }

        protected void Start()
        {
            if (s_instance == null)
                s_instance = this;
        }
    }
}
