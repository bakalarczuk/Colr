
namespace Habtic.Games.Colr
{
    public interface IPooledObject
    {

        string PoolTag { get; set; }

        void OnObjectSpawn();
        void ReEnqueueObject();
    }
}
