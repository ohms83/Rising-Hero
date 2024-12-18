using System;
using System.Collections.Generic;
using Pattern;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace Effect.Battle
{
    public class DamageNumberPool : MonoSingleton<DamageNumberPool>
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
            Assert.IsNotNull(objectPrefab);
            
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
            m_pool ??= new ObjectPool<DamageNumber>(
                OnCreatePoolItem,
                OnTakeFromPool,
                OnReturnedToPool,
                null,
                true,
                defaultPoolSize);

            // Warm up the pool by pre-allocating items
            List<DamageNumber> items = new(); 
            for (int i = 0; i < defaultPoolSize; ++i)
            {
                var item = ObjectPool.Get();
                item.SetDamage(999999);
                items.Add(item);
            }
            
            foreach (var item in items)
            {
                ObjectPool.Release(item);
            }

            return m_pool;
        }
        
        protected override void OnStart()
        {
            InitPool();
        }
    }
}
