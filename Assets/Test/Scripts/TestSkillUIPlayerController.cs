using System.Collections.Generic;
using Character.Controller;
using Event;
using Gameplay.System.LevelUp;
using UnityEngine;
using UnityEngine.InputSystem;
using Stats = Gameplay.Stats;

namespace Test.Scripts
{
    public class TestSkillUIPlayerController : ControllerBase
    {
        [SerializeField] private LevelUpEvent m_levelUpEvent;
        
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (m_levelUpEvent == null)
            {
                Debug.LogWarning("Level up event can't be null");
                return;
            }

            if (!context.started)
                return;
            
            var levelUpRewards = new List<LevelUpReward>
            {
                new ()
                {
                    name = "Attack + 10",
                    stats = new Stats()
                    {
                        Attack = 10
                    }
                },
                new ()
                {
                    name = "Health + 10",
                    stats = new Stats()
                    {
                        MaxHealth = 10
                    }
                },
            };
                
            m_levelUpEvent.Broadcast(new LevelUpEventData
            {
                newLevel = 2,
                oldLevel = 1,
                rewards = levelUpRewards
            });
        }
    }
}
