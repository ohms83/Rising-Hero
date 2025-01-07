using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Character.Controller
{
    [RequireComponent(typeof(GameCharacter))]
    public class ControllerBase : MonoBehaviour
    {
        public GameCharacter ControlledCharacter
        {
            get;
            private set;
        }

        protected virtual void Start()
        {
            ControlledCharacter = GetComponent<GameCharacter>();
            Assert.IsNotNull(ControlledCharacter);
        }
    }
}
