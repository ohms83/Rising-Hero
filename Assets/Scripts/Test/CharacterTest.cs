using System;
using Character;
using Gameplay.Equipment;
using UnityEngine;
using UnityEngine.Assertions;

namespace Test
{
    /// <summary>
    /// A utility script for testing GameCharacter.
    /// </summary>
    [RequireComponent(typeof(GameCharacter))]
    public class CharacterTest : MonoBehaviour
    {
        [SerializeField] private Equipment equipmentPrefab;

        private GameCharacter m_testCharacter;

        private void Start()
        {
            m_testCharacter = GetComponent<GameCharacter>();
            Assert.IsNotNull(m_testCharacter);

            if (equipmentPrefab != null)
            {
                var equipment = Instantiate(equipmentPrefab);
                m_testCharacter.Equip(equipment);
            }
        }
    }
}