using UnityEngine;

namespace Gameplay.Battle
{
    public class HitBox : MonoBehaviour
    {
        [SerializeField] private GameObject hitBox;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (hitBox != null)
                hitBox.SetActive(false);
        }

        private void OnHitBoxEnable()
        {
            if (hitBox != null)
                hitBox.SetActive(true);
        }

        private void OnHitBoxDisable()
        {
            if (hitBox != null)
                hitBox.SetActive(false);
        }
    }
}
