using UnityEngine;
using UnityEngine.Pool;

namespace Effect.Battle
{
    public class DamageNumberPool : MonoBehaviour
    {
        [SerializeField] private DamageNumber objectPrefab;
        /// <summary>
        /// Names that will be given to the instantiated pool objects. 
        /// </summary>
        [SerializeField] private string objectName = "damage_number";
        /// <summary>
        /// Initial size of the pool.
        /// </summary>
        [SerializeField] private int defaultPoolSize = 50;
        
        private IObjectPool<DamageNumber> m_pool;
        public IObjectPool<DamageNumber> ObjectPool => m_pool ?? InitPool();

        private int m_objectCount = 0;

        private DamageNumber OnCreatePoolItem()
        {
            var newObject = Instantiate(objectPrefab, transform, true);
            newObject.name = $"{objectName}_{m_objectCount++}";
            newObject.Init();
            newObject.OnFinishedAnimate += ReturnToPool;
            return newObject;
        }

        private void OnReturnedToPool(DamageNumber damageNumber)
        {
            damageNumber.transform.parent = transform;
            damageNumber.gameObject.SetActive(false);
        }

        private void OnTakeFromPool(DamageNumber damageNumber)
        {
            damageNumber.gameObject.SetActive(true);
            damageNumber.Animate();
        }

        private void ReturnToPool(DamageNumber damageNumber)
        {
            ObjectPool.Release(damageNumber);
        }

        private IObjectPool<DamageNumber> InitPool()
        {
            return m_pool ??= new ObjectPool<DamageNumber>(
                OnCreatePoolItem,
                OnTakeFromPool,
                OnReturnedToPool,
                null,
                true,
                defaultPoolSize);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            InitPool();
        }
    }
}
