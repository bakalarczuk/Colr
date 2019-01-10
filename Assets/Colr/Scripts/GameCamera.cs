using UnityEngine;

namespace Habtic.Games.Colr
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {

        private Camera cam;

        // Use this for initialization
        void Start()
        {
            cam = GetComponent<Camera>();            
        }

        public Vector3 WorldToScreenPoint(Vector3 position)
        {
            return cam.WorldToScreenPoint(position);
        }

        public Vector3 ScreenToWorldPoint(Vector3 position)
        {
            return cam.ScreenToWorldPoint(position);
        }
    }
}
