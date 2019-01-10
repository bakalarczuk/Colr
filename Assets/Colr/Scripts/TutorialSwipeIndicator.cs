using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class TutorialSwipeIndicator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _swipeIndicator;

        private LTDescr _tweenS;

        private Vector3 _swipeStart;

        void Awake()
        {
            _swipeIndicator.SetActive(false);
        }

        private void OnEnable()
        {
            StopSwipeTween();
        }

        private void OnDisable()
        {
            StopSwipeTween();
        }


        public void SwipeLeftRight()
        {
            _swipeIndicator.SetActive(true);
            Vector3 HorTweenStart = new Vector3(-(Screen.width * 0.5f), 0, 0);
            _swipeIndicator.transform.localPosition = HorTweenStart;
            _tweenS = LeanTween.moveLocalX(_swipeIndicator, (Screen.width * 0.5f), 0.9f)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                _swipeIndicator.SetActive(false);
                if (_tweenS != null)
                {
                    LeanTween.cancel(_tweenS.id);
                    _tweenS = null;
                }
                SwipeLeftRight();
            })
            .setDelay(2f);
        }

        public void SwipeDownUp()
        {
            _swipeIndicator.SetActive(true);
            Vector3 VerTweenStart = new Vector3(0, -(Screen.height * 0.5f), 0);
            _swipeIndicator.transform.localPosition = VerTweenStart;
            _tweenS = LeanTween.moveLocalY(_swipeIndicator, (Screen.height * 0.5f), 0.9f)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                _swipeIndicator.SetActive(false);
                if (_tweenS != null)
                {
                    LeanTween.cancel(_tweenS.id);
                    _tweenS = null;
                }
                SwipeDownUp();
            })
            .setDelay(2f);
        }

        public void StopSwipeTween()
        {
            if (_tweenS != null)
            {
                LeanTween.cancel(_tweenS.id);
                _swipeIndicator.SetActive(false);
            }
        }
    }
}
