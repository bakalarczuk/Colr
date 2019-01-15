using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class AnswerIndicator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _correctIndicator;
        [SerializeField]
        private GameObject _incorrectIndicator;

        private LTDescr _tweenM;

        private void Awake()
        {
            GameManager.OnLevelStateChanged += ShowAnswerIndicator;
            GameManager.OnCorrectInput += ShowCorrectIndicator;
            GameManager.OnIncorrectInput += ShowIncorrectIndicator;
            _correctIndicator.transform.localScale = Vector3.zero;
            _incorrectIndicator.transform.localScale = Vector3.zero;
        }

        private void OnDestroy()
        {
            GameManager.OnLevelStateChanged -= ShowAnswerIndicator;
            GameManager.OnCorrectInput -= ShowCorrectIndicator;
            GameManager.OnIncorrectInput -= ShowIncorrectIndicator;
        }

        private void ShowAnswerIndicator(LevelStates levelState)
        {
            switch (levelState)
            {
                case LevelStates.setup:
                    StopAllTweens();
                    break;
                default:
                    break;
            }
        }

        private void ShowCorrectIndicator()
        {
            StopAllTweens();
            _tweenM = LeanTween.scale(_correctIndicator, Vector3.one, 0.3f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    _correctIndicator.transform.localScale = Vector3.zero;
                });
        }

        private void ShowIncorrectIndicator()
        {
            StopAllTweens();
            _tweenM = LeanTween.scale(_incorrectIndicator, Vector3.one, 0.3f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    _incorrectIndicator.transform.localScale = Vector3.zero;
                });
        }

        private void StopAllTweens()
        {
            if (_tweenM != null)
            {
                LeanTween.cancel(_tweenM.id);
                _correctIndicator.transform.localScale = Vector3.zero;
                _incorrectIndicator.transform.localScale = Vector3.zero;
            }
        }
    }
}
