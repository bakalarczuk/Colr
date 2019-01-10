using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public enum SafeAreaDir
    {
        none,
        top,
        right,
        bottom,
        left,
        top_right,
        top_left,
        bottom_right,
        Bottom_left
    }

    public class SafeAreaFitter : MonoBehaviour
    {

        [SerializeField]
        private SafeAreaDir safeAreaDir;

        [SerializeField]
        private Rect fakeSafeArea;

        [SerializeField]
        private bool UseFakeSafe;

        private void Awake()
        {

            Rect safeArea = UseFakeSafe ? fakeSafeArea : Screen.safeArea;
            float rightOffset = Screen.width - safeArea.x - safeArea.width;
            float downOffset = Screen.height - safeArea.y - safeArea.height;

            switch (safeAreaDir)
            {
                case SafeAreaDir.none:
                    break;
                case SafeAreaDir.top:
                    this.transform.localPosition += Vector3.down * safeArea.y;
                    break;
                case SafeAreaDir.right:
                    this.transform.localPosition += Vector3.left * rightOffset;
                    break;
                case SafeAreaDir.bottom:
                    this.transform.localPosition += Vector3.up * downOffset;
                    break;
                case SafeAreaDir.left:
                    this.transform.localPosition += Vector3.right * safeArea.x;
                    break;
                case SafeAreaDir.top_left:
                    this.transform.localPosition += Vector3.down * safeArea.y;
                    this.transform.localPosition += Vector3.right * safeArea.x;
                    break;
                case SafeAreaDir.top_right:
                    this.transform.localPosition += Vector3.down * safeArea.y;
                    this.transform.localPosition += Vector3.left * rightOffset;
                    break;
                case SafeAreaDir.Bottom_left:
                    this.transform.localPosition += Vector3.right * safeArea.x;
                    this.transform.localPosition += Vector3.up * downOffset;
                    break;
                case SafeAreaDir.bottom_right:
                    this.transform.localPosition += Vector3.left * rightOffset;
                    this.transform.localPosition += Vector3.up * downOffset;
                    break;
            }

        }
    }
}
