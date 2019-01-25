using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class AnswerIndicator : MonoBehaviour
    {
		[SerializeField]
		private Game _game;

		[SerializeField]
        private GameObject _correctIndicator;
		[SerializeField]
		private GameObject _incorrectIndicator;
		[SerializeField]
		private TMP_Text _correctRandomText;
		[SerializeField]
		private TMP_Text _incorrectRandomText;

		private LTDescr _tweenM;

        private void Awake()
        {
            GameManager.OnLevelStateChanged += ShowAnswerIndicator;
            GameManager.OnCorrectInput += ShowCorrectIndicator;
            GameManager.OnIncorrectInput += ShowIncorrectIndicator;
            _correctIndicator.transform.localScale = Vector3.zero;
            _incorrectIndicator.transform.localScale = Vector3.zero;

			_correctRandomText.text = string.Empty;
			_incorrectRandomText.text = string.Empty;
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
			GameManager.Instance.ColorWheel.ComeOut();

			_correctRandomText.text = WordDictionary.Instance.GetGoodAnswerText;

			_tweenM = LeanTween.scale(_correctIndicator, Vector3.one, 1f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    _correctIndicator.transform.localScale = Vector3.zero;
					GameManager.Instance.LevelStart();
                });
        }

        private void ShowIncorrectIndicator()
        {
            StopAllTweens();
			GameManager.Instance.ColorWheel.ComeOut();

			_incorrectRandomText.text = WordDictionary.Instance.GetWrongAnswerText;

			_tweenM = LeanTween.scale(_incorrectIndicator, Vector3.one, 1f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
                    _incorrectIndicator.transform.localScale = Vector3.zero;
					GameManager.Instance.LevelStart();
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
