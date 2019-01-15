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

		[SerializeField]
		private bool waitForFadeEnd = true;

        public void OnEnable()
        {
            StartCoroutine(AnimationStart());
        }

        IEnumerator AnimationStart(){
			if (waitForFadeEnd)
				yield return new WaitForSeconds(LevelManager.Instance.GetFade.GetDuration);
			else
				yield return null;
            RectTransform rectTransform = GetComponent<RectTransform>();
            Vector2 destination = new Vector2(0, -distanceToTop);
            LeanTween.move(rectTransform, destination, duration + (waitForFadeEnd ? 0: LevelManager.Instance.GetFade.GetDuration)).setEase(easeType);
        }
    }
}
