using System.Collections.Generic;
using Character;
using Event;
using Gameplay.System.LevelUp;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Gameplay
{
    public class SkillPickerControl : MonoBehaviour
    {
        [SerializeField] private UIDocument m_uiDocument;
        [SerializeField] private CharacterEvent m_playerSpawnedEvent;
        [SerializeField] private LevelUpEvent m_levelUpEvent;

        private List<GameCharacter> m_players = new();
        
        // private struct LevelUpEvent
        // {
        //     internal GameCharacter character;
        //     internal LevelUpReward levelUpReward;
        // }

        /// <summary>
        /// A list of unprocessed level-up events
        /// </summary>
        // private List<LevelUpEvent> m_levelUpEvents;

        private void Awake()
        {
            if (m_playerSpawnedEvent != null)
                m_playerSpawnedEvent.AddListener(OnPlayerSpawned);
            
            if (m_levelUpEvent != null)
                m_levelUpEvent.AddListener(OnLevelUp);
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            // TODO: Don't use timeScale to pause the game.
            Time.timeScale = 0;
            m_uiDocument.enabled = true;
        }

        private void OnDisable()
        {
            // TODO: Don't use timeScale to pause the game.
            Time.timeScale = 1;
            m_uiDocument.enabled = false;
        }

        private void SetupButton(Button button, LevelUpReward reward)
        {
            button.text = reward.name;
            button.clicked += () =>
            {
                var character = m_players[0];
                reward.ApplyReward(character);
                character.ComputeStats();
                character.health.Value = character.Stats.MaxHealth;
                enabled = false;
            };
        }

        private void OnPlayerSpawned(GameCharacter player)
        {
            m_players.Add(player);
        }

        private void OnLevelUp(LevelUpEventData data)
        {
            m_uiDocument.enabled = true;
            enabled = true;
            
            var button1 = m_uiDocument.rootVisualElement.Query<Button>("Skill1_Btn").AtIndex(0);
            var button2 = m_uiDocument.rootVisualElement.Query<Button>("Skill2_Btn").AtIndex(0);
            SetupButton(button1, data.rewards[0]);
            SetupButton(button2, data.rewards[1]);
        }
    }
}
