using Gameplay.System.LevelUp;
using Pattern;
using UnityEngine;

namespace Event
{
    [CreateAssetMenu(fileName = "LevelUpEvent", menuName = "Scriptable Objects/Event/LevelUpEvent")]
    public class LevelUpEvent : Event<LevelUpEventData>
    {
    }
}
