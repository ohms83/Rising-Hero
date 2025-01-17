using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.System.LevelUp
{
    [CreateAssetMenu(fileName = "LevelUpData", menuName = "Scriptable Objects/System/Level/LevelUpData")]
    public class LevelUpData : ScriptableObject
    {
        [Tooltip("The curve data about how many experience points is needed per level.")]
        public AnimationCurve xpCurve;
        [Tooltip("Accumulated player's experience point. This can only be modified from a LevelUpSystem component.")]
        public float xp = 0;
        public int level = 1;

        public float ToNextLevel
        {
            get
            {
                if (xpCurve == null)
                    return 0;
                var key = Mathf.Clamp(level, 1, xpCurve.length);
                return xpCurve[key].value;
            }
        }

        public bool IsLevelUp => ToNextLevel > 0 && xp >= ToNextLevel;
    }
}
