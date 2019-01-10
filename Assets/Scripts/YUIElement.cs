using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
// using Habtic.Core;
using Habtic.Managers;

namespace Habtic.YUI
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class YUIElement : MonoBehaviour
    {
        #region Variables

        public RectTransform RectTransform
        {
            get
            {
                if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
                return _rectTransform;
            }
        }

        private RectTransform _rectTransform;

        public Vector2 OriginalAnchorMin
        {
            get
            {
                return _originalAnchorMin;
            }
        }

        public Vector2 OriginalAnchorMax
        {
            get
            {
                return _originalAnchorMax;
            }
        }

        private Vector2 _originalAnchorMin;
        private Vector2 _originalAnchorMax;

        protected Rect _originalPixelRect;

        protected float _originalWidth;
        protected float _originalHeight;

        #endregion
        // #region Unity Methods

        // private void Awake()
        // {
        //     UiElementSetup();
        // }

        // #endregion
        // #region Methods

        // protected virtual void UiElementSetup()
        // {
        //     _originalAnchorMin = RectTransform.anchorMin;
        //     _originalAnchorMax = RectTransform.anchorMax;
        // }

        // protected void GetOriginalRectPixelValues()
        // {
        //     _originalPixelRect = RectTransformUtility.PixelAdjustRect(RectTransform, ManagerLocator.UiManager.MainCanvas);
        //     _originalWidth = _originalPixelRect.width;
        //     _originalHeight = _originalPixelRect.height;
        // }

        // public void Hide()
        // {
        //     gameObject.SetActive(false);
        // }

        // public void Show()
        // {
        //     gameObject.SetActive(true);
        // }

        // #endregion

        // #region Console info

        // protected void ShowDebug(object debugObject)
        // {
        //     if (Debug.isDebugBuild) Debug.Log(this.GetType().Name + ", Message: " + debugObject);
        // }

        // #endregion

    }
}
