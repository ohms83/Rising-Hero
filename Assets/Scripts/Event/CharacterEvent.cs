using Character;
using Pattern;
using UnityEngine;

namespace Event
{
    /// <summary>
    /// A generic character event.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterEvent", menuName = "Scriptable Objects/EventBus/Character/CharacterEvent")]
    public class CharacterEvent : Event<GameCharacter>
    {
    
    }
}
