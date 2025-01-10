using System;
using Character;
using Pattern;
using ScriptableObjects.Character;
using UnityEngine;

namespace ScriptableObjects.Event
{
    /// <summary>
    /// A generic character event.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterEvent", menuName = "Scriptable Objects/EventBus/Character/CharacterEvent")]
    public class CharacterEvent : EventBus<GameCharacter>
    {
    
    }
}
