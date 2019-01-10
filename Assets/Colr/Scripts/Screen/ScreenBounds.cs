using UnityEngine;

namespace Habtic.Games.Colr
{
    public class ScreenBounds : Singleton<ScreenBounds>
    {

        public float Left { get; set; }
        public float Right { get; set; }
        public float Top { get; set; }
        public float Bottom { get; set; }

        public float SafeXMin {get; set;}
        public float SafeXMax {get; set;}
        public float SafeYMin {get;set;}
        public float SafeYMax {get; set;}

        private float distanceZ;

        public Camera gameCam;

        void Awake()
        {
            distanceZ = Mathf.Abs(gameCam.transform.position.z);
            Left = gameCam.ScreenToWorldPoint(new Vector3(0, 0, distanceZ)).x;
            Right = gameCam.ScreenToWorldPoint(new Vector3(Screen.width, 0, distanceZ)).x;
            Top = gameCam.ScreenToWorldPoint(new Vector3(0, Screen.height, distanceZ)).y;
            Bottom = gameCam.ScreenToWorldPoint(new Vector3(0, 0, distanceZ)).y;

            float _safeXMix = Screen.safeArea.x;
            float _safeXMax = Screen.width - Screen.safeArea.x - Screen.safeArea.width;
            float _safeYMin = Screen.safeArea.y;
            float _safeYMax = Screen.height - Screen.safeArea.y - Screen.safeArea.height;

            SafeXMin = gameCam.ScreenToViewportPoint(new Vector3(_safeXMix,0,0)).x;
            SafeXMax = gameCam.ScreenToViewportPoint(new Vector3(_safeXMax,0,0)).x;
            SafeYMin = gameCam.ScreenToViewportPoint(new Vector3(0,_safeYMax,0)).y;
            SafeYMax = gameCam.ScreenToViewportPoint(new Vector3(0,_safeYMax,0)).y;
        }
    }
}
