using UnityEngine;

namespace Gameplay.Battle
{
    public static class DamageCalculator
    {
        public static int CalculateDamage(Stats attacker, Stats defender)
        {
            var variant = attacker.Attack * 0.15f;
            var attack = attacker.Attack + (int)Random.Range(-variant, variant);
            return Mathf.Max(1, attack - defender.Defence);
        }
    }
}
