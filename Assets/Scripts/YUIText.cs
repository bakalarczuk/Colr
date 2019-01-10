using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Habtic.Managers;
using TMPro;

namespace Habtic.YUI
{
    [RequireComponent(typeof(TMP_Text))]
    public class YUIText : YUIElement
    {
        #region Variables

        public TMP_Text TextComponent
        {
            get
            {
                if (!_textComponent)
                {
                    _textComponent = GetComponent<TMP_Text>();
                }

                return _textComponent;
            }
        }

        protected TMP_Text _textComponent;

        #endregion

        // #region Helper Methods

        // public string GetText()
        // {
        //     return TextComponent ? TextComponent.text : "NO TEXT COMPONENT FOUND!";
        // }

        // public void SetText(string text)
        // {
        //     TextComponent.text = text;
        // }

        // public void SetFontSize(int fontSize)
        // {
        //     TextComponent.fontSize = fontSize;
        // }

        // public void SetFontColor(Color32 color)
        // {
        //     TextComponent.color = color;
        // }

        // public void SetAnchorToCenterKeepSize()
        // {
        //     GetOriginalRectPixelValues();

        //     RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        //     RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        //     RectTransform.sizeDelta = new Vector2(_originalWidth, _originalHeight);
        // }

        // #endregion
    }
}
