using Character;
using Character.Data;
using Event;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace Gameplay
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private CharacterEvent m_playerSpawnedEvent;
        
        private CinemachineCamera m_camera;
        private GameObject m_followTarget;
        
        private void Awake()
        {
            enabled = false;
            m_playerSpawnedEvent.AddListener(OnPlayerSpawned);

            m_camera = GetComponent<CinemachineCamera>();
            m_followTarget = new GameObject("CameraTarget");
        }

        // TODO: Handle multiple player characters
        private void OnPlayerSpawned(GameCharacter spawnedCharacter)
        {
            var characterData = (PlayerCharacterData)spawnedCharacter.SharedData;
            Assert.IsNotNull(characterData, $"{characterData} is not a PlayerCharacterData type!");
            FollowPlayer(spawnedCharacter, characterData.cameraTarget);
        }

        private void FollowPlayer(Component player, Vector2 targetPosition)
        {
            m_followTarget.transform.parent = player.transform;
            m_followTarget.transform.localPosition = targetPosition;

            m_camera.Follow = m_followTarget.transform;
        }
    }
}
