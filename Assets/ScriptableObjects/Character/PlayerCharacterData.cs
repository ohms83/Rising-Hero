using UnityEngine;

namespace ScriptableObjects.Character
{
    [CreateAssetMenu(fileName = "PlayerCharacterData", menuName = "Scriptable Objects/Character/PlayerCharacterData", order = 1)]
    public class PlayerCharacterData : GameCharacterData
    {
        [Tooltip("Camera's tracking target.")]
        public Vector2 cameraTarget;
    }
}