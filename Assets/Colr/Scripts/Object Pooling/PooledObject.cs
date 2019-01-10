﻿
namespace Habtic.Games.Colr
{
    public abstract class PooledObject : Entity, IPooledObject
    {
        public string PoolTag { get; set; }

        public abstract void OnObjectSpawn();

        public void ReEnqueueObject()
        {
            ObjectPooler.Instance.EnqueueObject(PoolTag, gameObject);
        }

        public override void RemoveSelf()
        {
            ReEnqueueObject();
        }
    }
}
