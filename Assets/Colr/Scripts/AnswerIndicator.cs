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
		[SerializeField]
		private TMP_Text _correctCount;
		[SerializeField]
		private TMP_Text _incorrectCount;
		[SerializeField]
		private ParticleSystem _colorWheelBlast;

		public GameObject CorrectIndicator { get { return _correctIndicator; } }
		public GameObject IncorrectIndicator { get { return _incorrectIndicator; } }
		public TMP_Text CorrectText { get { return _correctRandomText; } set { _correctRandomText = value; } }
		public TMP_Text IncorrectText { get { return _incorrectRandomText; } set { _incorrectRandomText = value; } }
		public TMP_Text CorrectCount { get { return _correctCount; } set { _correctCount = value; } }
		public TMP_Text IncorrectCount { get { return _incorrectCount; } set { _incorrectCount = value; } }

		private LTDescr _tweenM;

        private void Awake()
        {
            GameManager.OnLevelStateChanged += ShowAnswerIndicator;
            GameManager.OnCorrectInput += ShowCorrectIndicator;
            GameManager.OnIncorrectInput += ShowIncorrectIndicator;
			if (_colorWheelBlast)
				_colorWheelBlast.Stop();
			_correctIndicator.transform.localScale = Vector3.zero;
            _incorrectIndicator.transform.localScale = Vector3.zero;

			_correctRandomText.text = string.Empty;
			_incorrectRandomText.text = string.Empty;

			if(_correctCount != null)
				_correctCount.text = "0";

			if(_incorrectCount != null)
				_incorrectCount.text = "0";
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
			if (_colorWheelBlast)
				_colorWheelBlast.Play();
			_tweenM = LeanTween.scale(_correctIndicator, Vector3.one, 0.5f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
					_correctCount.text = GameManager.Instance._level.CorrectCounter.ToString();
					if(this.gameObject.activeSelf)
						CoroutineHandler.Instance.StartCoroutine(CoroutineHandler.Instance.HideIndicator(_correctIndicator));
                });
        }

		private void ShowIncorrectIndicator()
        {
            StopAllTweens();
			GameManager.Instance.ColorWheel.ComeOut();
			_incorrectRandomText.text = WordDictionary.Instance.GetWrongAnswerText;
			if (_colorWheelBlast)
				_colorWheelBlast.Play();
			_tweenM = LeanTween.scale(_incorrectIndicator, Vector3.one, 1f)
                .setEase(LeanTweenType.easeInOutSine)
                .setOnComplete(() =>
                {
					_incorrectCount.text = GameManager.Instance._level.IncorrectCounter.ToString();
					if(this.gameObject.activeSelf)
						CoroutineHandler.Instance.StartCoroutine(CoroutineHandler.Instance.HideIndicator(_incorrectIndicator));
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
