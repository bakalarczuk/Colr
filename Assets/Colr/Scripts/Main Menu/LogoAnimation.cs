using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DentedPixel;

namespace Habtic.Games.Colr
{
    [RequireComponent(typeof(RectTransform))]
    public class LogoAnimation : MonoBehaviour
    {
        [SerializeField]
        private LeanTweenType easeType;

        [SerializeField]
        private float duration = 1f;

        [SerializeField]
        private float distanceToTop = 100f;

        public void OnEnable()
        {
            StartCoroutine(AnimationStart());
        }

        IEnumerator AnimationStart(){
            yield return new WaitForSeconds(LevelManager.Instance.GetFade.GetDuration);
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 destination = new Vector2(0, -distanceToTop);
            LeanTween.move(rectTransform, destination, duration).setEase(easeType);
        }
    }
}
