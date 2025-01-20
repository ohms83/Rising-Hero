using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.System.LevelUp
{
    [CreateAssetMenu(fileName = "LevelUpData", menuName = "Scriptable Objects/Gameplay/Level Up/LevelUpData")]
    public class LevelUpData : ScriptableObject
    {
        [Tooltip("A list of experience points required to level up.")]
        public List<float> xpSteps = new ();
        [Tooltip("Accumulated player's experience point. This can only be modified from a LevelUpSystem component.")]
        public float xp = 0;
        public int level = 1;

        public float ToNextLevel
        {
            get
            {
                if (xpSteps.Count <= 0)
                    return 0;
                // level starts with one but the list index is zero-base
                var key = Mathf.Clamp(level-1, 0, xpSteps.Count);
                return xpSteps[key];
            }
        }

        public bool IsLevelUp => ToNextLevel > 0 && xp >= ToNextLevel;

        public void Reset()
        {
            xp = 0;
            level = 1;
        }
    }
}
