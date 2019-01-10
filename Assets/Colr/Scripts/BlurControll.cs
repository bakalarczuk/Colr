using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{
    public class BlurControll : MonoBehaviour
    {

        private LTDescr _tween;
        private CanvasGroup myBlur;

        private void Awake()
        {
            myBlur = this.gameObject.GetComponent<CanvasGroup>();
            myBlur.alpha = 0f;
        }

        public void BlurEnabled(bool isEnable)
        {
            int setValue = isEnable ? 1 : 0;
            _tween = LeanTween.alphaCanvas(myBlur, setValue, 1);
        }
    }
}
