using Character;
using ScriptableObjects.Event;
using Unity.Cinemachine;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private CharacterSpawnedEvent playerSpawnedEvent;
        
        private CinemachineCamera m_camera;
        private GameObject m_followTarget;
        
        private void Awake()
        {
            enabled = false;
            playerSpawnedEvent.onEventRaised += OnPlayerSpawned;

            m_camera = GetComponent<CinemachineCamera>();
            m_followTarget = new GameObject("CameraTarget");
        }

        // TODO: Handle multiple player characters
        private void OnPlayerSpawned(SpawnedCharacterEventData spawnedCharacterEventData)
        {
            FollowPlayer(spawnedCharacterEventData.spawnedCharacter,
                spawnedCharacterEventData.characterData.cameraTarget);
        }

        private void FollowPlayer(GameCharacter player, Vector2 targetPosition)
        {
            m_followTarget.transform.parent = player.transform;
            m_followTarget.transform.localPosition = targetPosition;

            m_camera.Follow = m_followTarget.transform;
        }
    }
}
