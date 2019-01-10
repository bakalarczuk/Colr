using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{
    [RequireComponent(typeof(Image))]
    public class Fade : MonoBehaviour
    {
    
        [SerializeField]
        private Color from;

        [SerializeField]
        private Color to;

        [SerializeField]
        private float duration;

        public float GetDuration{get { return duration;}}

        private Image _image;

        private bool _disableAfterFade = false;

        // Use this for initialization
        void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void In(bool shoudDisable = true, Action callback = null)
        {
            EnsureImageEnabled();
            _disableAfterFade = shoudDisable;
            StartCoroutine(IEFade(from, to, duration, callback));
        }

        public void Out(Action callback = null)
        {
            EnsureImageEnabled();
            _disableAfterFade = false;
            StartCoroutine(IEFade(to, from, duration, callback));
        }

        void EnsureImageEnabled()
        {
            if(_image == null) {
                _image = GetComponent<Image>();
            }
            _image.enabled = true;
        }

        IEnumerator IEFade(Color from, Color to, float duration, Action callback)
        {
            float t = 0f;

            while (t < 1)
            {
                t += Time.deltaTime / duration;
                _image.color = Color.Lerp(from, to, t);

                yield return null;
            }

            if (_disableAfterFade)
            {
                _image.enabled = false;
            }

            if (callback != null)
            {
                callback();
            }
        }
    }
}
