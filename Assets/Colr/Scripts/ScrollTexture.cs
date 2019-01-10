using UnityEngine;

namespace Habtic.Games.Colr
{
    [RequireComponent(typeof(MeshRenderer))]
    public class ScrollTexture : MonoBehaviour
    {

        [SerializeField]
        private float scrollSpeed = 2f;

        private MeshRenderer r;

        // Use this for initialization
        void Start()
        {
            r = GetComponent<MeshRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            r.material.SetFloat("Tiling", (Time.time * scrollSpeed % 1) * -1);
        }
    }
}
