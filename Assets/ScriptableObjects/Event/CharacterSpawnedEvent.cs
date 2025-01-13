using System;
using Character;
using Pattern;
using ScriptableObjects.Character;
using UnityEngine;

namespace ScriptableObjects.Event
{
    [Serializable]
    public struct SpawnedCharacterEventData
    {
        public GameCharacterData characterData;
        public GameCharacter spawnedCharacter;
    }

    [CreateAssetMenu(fileName = "CharacterSpawnedEvent", menuName = "Scriptable Objects/EventBus/Character/CharacterSpawnedEvent")]
    public class CharacterSpawnedEvent : Event<SpawnedCharacterEventData>
    {
        
    }
}
