using System;
using System.Diagnostics.CodeAnalysis;
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

        protected virtual void Awake()
        {
            ControlledCharacter = GetComponent<GameCharacter>();
            Assert.IsNotNull(ControlledCharacter);
        }

        protected virtual void OnEnable()
        {
            ControlledCharacter.onCharacterDeath.AddListener(OnCharacterDeath);
        }

        protected virtual void OnDisable()
        {
            ControlledCharacter.onCharacterDeath.RemoveListener(OnCharacterDeath);
        }

        protected virtual void OnCharacterDeath(GameCharacter controlledCharacter)
        {
            enabled = false;
        }
    }
}
