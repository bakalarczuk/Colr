using UnityEngine;

namespace Habtic.Games.Colr
{
    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject[] Prefabs;
        public int Size;
        public Transform Parent;
        public bool Grow;
        public bool SpawnOnAwake;

        public GameObject Prefab
        {
            get
            {
                if (Prefabs.Length == 0) return null;
                return Prefabs.Length == 1 ? Prefabs[0] : Prefabs[Random.Range(0, Prefabs.Length)];
            }
        }
    }
}
