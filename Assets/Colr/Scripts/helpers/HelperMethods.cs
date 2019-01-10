using UnityEngine;

namespace Habtic.Games.Colr
{
    public class HelperMethods
    {
        public static bool CoinFlip()
        {
            return Random.Range(0f, 1f) < 0.5f;
        }

        public static Quaternion DirectionToRotation(Vector3 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static Vector3 GetSpawnPointLeftOrRight()
        {
            float offset = ScreenBounds.Instance.Right / 2;
            float x = CoinFlip() ? ScreenBounds.Instance.Left - offset : ScreenBounds.Instance.Right + offset;
            float y = Random.Range(ScreenBounds.Instance.Bottom, ScreenBounds.Instance.Top);
            return new Vector2(x, y);
        }

        public static void ShowDebug(object debugObject, string className)
        {
            if (Debug.isDebugBuild) Debug.Log(className + ", Message: " + debugObject);
        }
    }
}
