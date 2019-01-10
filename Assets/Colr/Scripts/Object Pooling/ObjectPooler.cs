using System.Collections.Generic;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class ObjectPooler : Singleton<ObjectPooler>
    {

        public struct PoolQueue
        {
            public Pool Pool;
            public Queue<GameObject> Queue;
        }

        private Dictionary<string, PoolQueue> _poolDictionary;

        // Use this for initialization
        void Awake()
        {
            _poolDictionary = new Dictionary<string, PoolQueue>();
        }

        void OnDisable()
        {
            Clear();
        }

        public void CreatePool(Pool pool)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.Size; i++)
            {

                GameObject obj = Instantiate(pool.Prefab, pool.Parent);

                IPooledObject pooledObject = obj.GetComponent<IPooledObject>();

                if (pooledObject != null)
                {
                    pooledObject.PoolTag = pool.Tag;
                }

                obj.SetActive(false);
                objectQueue.Enqueue(obj);
            }

            _poolDictionary.Add(pool.Tag, new PoolQueue { Pool = pool, Queue = objectQueue });
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            if (!Exists(tag))
                return null;

            Pool pool = _poolDictionary[tag].Pool;

            GameObject objectToSpawn = null;

            if (QueueCount(tag) < 1)
            {
                if (pool.Grow)
                    objectToSpawn = Instantiate(pool.Prefab, position, rotation, pool.Parent);
                else
                    return null;
            }
            else
            {
                objectToSpawn = _poolDictionary[tag].Queue.Dequeue();
            }

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

            if (pooledObject != null)
            {
                pooledObject.OnObjectSpawn();
            }

            return objectToSpawn;
        }

        public void EnqueueObject(string tag, GameObject obj)
        {
            if (!Exists(tag))
                return;

            obj.SetActive(false);
            _poolDictionary[tag].Queue.Enqueue(obj);
        }

        public int QueueCount(string tag)
        {
            return _poolDictionary[tag].Queue.Count;
        }

        public bool Exists(string tag)
        {
            if (!_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
                return false;
            }

            return true;
        }

        public void Remove(string tag)
        {
            if (!_poolDictionary.ContainsKey(tag)) return;

            PoolQueue pq = _poolDictionary[tag];
            ClearQueue(pq.Queue);
            ClearPool(pq.Pool);

            _poolDictionary.Remove(tag);
        }

        public void Clear()
        {
            foreach (PoolQueue pq in _poolDictionary.Values)
            {
                ClearQueue(pq.Queue);
                ClearPool(pq.Pool);
            }

            _poolDictionary.Clear();
        }

        private void ClearQueue(Queue<GameObject> queue)
        {
            int queueSize = queue.Count;

            for (int i = 0; i < queueSize; i++)
            {
                GameObject gobj = queue.Dequeue();
                Destroy(gobj);
            }
        }

        private void ClearPool(Pool pool)
        {
            Transform parent = pool.Parent;
            int childCount = parent.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(parent.GetChild(i).gameObject);
            }
        }
    }
}
