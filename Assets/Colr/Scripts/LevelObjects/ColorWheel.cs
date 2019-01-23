using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Habtic.Managers;
using TMPro;

namespace Habtic.Games.Colr
{
    public class ColorWheel : MonoBehaviour
    {
		#region Variables and Properties

		[SerializeField]
		private GameObject _rotationHandle;

		[SerializeField]
		private float _rotationDuration;

		[SerializeField]
		private LeanTweenType _rotationEaseType;

		[SerializeField]
		private WheelColor[] _colorPrefabs;

		[SerializeField]
		private ColrColor _defaultColor;

		[SerializeField]
		public TMP_Text _colorText;

		[SerializeField]
		private ColrColor[] _colors; //= new ColrColor[];

		[SerializeField]
		private ColrColor[] _generatedColors;

		[SerializeField]
		private ColrColor[] _unusedColors;
 
		public ColrColor _properColor;	

		private Level _level;

		private LTDescr _tweenM;

        #endregion
        #region Unity Methods

        void Awake()
        {
            _level = Level.Instance;
        }

		#endregion

		#region Methods

		public WheelColor[] ColorPrefabs{ get { return _colorPrefabs; } }

        public void ComeIn(Level lvl)
        {
			foreach (WheelColor wc in _colorPrefabs)
			{
				wc.SetColor(ColrColor.ColourValue(_defaultColor.colorName), _defaultColor);
			}

			_colorText.text = string.Empty;

			if (_tweenM != null)
            {
                if (LeanTween.isTweening(_tweenM.id))
                {
                    LeanTween.cancel(_tweenM.id);
                }
            }

            transform.localPosition = Vector3.zero;

			LeanTween.rotateZ(_rotationHandle, 1080, 2).
				setEase(_rotationEaseType)
				.setOnComplete(() => {
					SelectColors(lvl);
					GameManager.Instance.GameTimer.TimerReset();
					GameManager.Instance.GameTimer.StartTimer(GameManager.Instance.AnswerTime);
					GameManager.Instance.GameTimer.ResumeTimer();

				});
		}

        public void StartNewLevel(Level lvl)
        {
            ComeIn(lvl);
			//SelectColors(lvl);
        }

		public void SelectColors(Level lvl)
		{
			ColrColor[] myColors = new ColrColor[3];
			List<ColrColor> unusedColors = new List<ColrColor>(_colors);
			List<int> myNumbers = new List<int>();
			System.Random randNum = new System.Random();
			for (int currIndex = 0; currIndex < 3; currIndex++)
			{
				// generate a candidate
				int randCandidate = randNum.Next(0, _colors.Length);

				// generate a new candidate as long as we don't get one that isn't in the array
				while (myNumbers.Contains(randCandidate))
				{
					randCandidate = randNum.Next(0, _colors.Length);
				}

				myNumbers.Add(randCandidate);
				unusedColors.Remove(_colors[randCandidate]);
				myColors[currIndex] = _colors[randCandidate];
			}

			_generatedColors = myColors;
			_unusedColors = unusedColors.ToArray();
			for (int i = 0; i < _colorPrefabs.Length; i++)
			{
				_colorPrefabs[i].SetColor(ColrColor.ColourValue(_generatedColors[i].colorName), _generatedColors[i]);
			}

			int properColorIndex = Random.Range(0, _generatedColors.Length);
			_properColor = _generatedColors[properColorIndex];

			if (lvl.Complexity == 1)
			{
				GameManager.Instance.QuestionPanel.SetQuestionText("game_level_instruction_printed_color");
				_colorText.text = _generatedColors[properColorIndex].colorName.ToString().ToUpper();
				_colorText.color = ColrColor.ColourValue(ColrColor.ColorNames.Black);
			}
			if (lvl.Complexity == 2)
			{
				if (HelperMethods.CoinFlip())
				{
					GameManager.Instance.QuestionPanel.SetQuestionText("game_level_instruction_printed_color");
					_colorText.text = _generatedColors[properColorIndex].colorName.ToString().ToUpper();
					_colorText.color = ColrColor.ColourValue(ColrColor.ColorNames.Black);
				}
				else
				{
					GameManager.Instance.QuestionPanel.SetQuestionText("game_level_instruction_text_color");
					_colorText.text = WordDictionary.Instance.GetShortWord();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
			}
			if (lvl.Complexity == 3)
			{
				GameManager.Instance.QuestionPanel.SetQuestionText("game_level_instruction_text_color");
				if (HelperMethods.CoinFlip())
				{
					_colorText.text = WordDictionary.Instance.GetShortWord();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
				else
				{
					_colorText.text = WordDictionary.Instance.GetLongWord();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
			}
			if (lvl.Complexity == 4)
			{
				GameManager.Instance.QuestionPanel.SetQuestionText("game_level_instruction_text_color");
				int check = Random.Range(1, 4);
				if (check == 1)
				{
					_colorText.text = WordDictionary.Instance.GetShortWord();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
				else if(check == 2)
				{
					_colorText.text = WordDictionary.Instance.GetLongWord();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				} else if(check == 3)
				{
					_colorText.text = _unusedColors[Random.Range(0, _unusedColors.Length)].colorName.ToString().ToUpper();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
			}
			if (lvl.Complexity >= 5)
			{
				int check = Random.Range(1, 5);
				GameManager.Instance.QuestionPanel.SetQuestionText(check == 4 ? "game_level_instruction_printed_color" : "game_level_instruction_text_color");
				if (check == 1)
				{
					_colorText.text = WordDictionary.Instance.GetShortWord();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
				else if(check == 2)
				{
					_colorText.text = WordDictionary.Instance.GetLongWord();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
				else if (check == 3)
				{
					_colorText.text = _unusedColors[Random.Range(0, _unusedColors.Length)].colorName.ToString().ToUpper();
					_colorText.color = ColrColor.ColourValue(_properColor.colorName);
				}
				else if (check == 4)
				{
					_colorText.text = _properColor.colorName.ToString().ToUpper();
					_colorText.color = ColrColor.ColourValue(_unusedColors[Random.Range(0, _unusedColors.Length)].colorName);
				}
			}
		}


		public void SelectIntroColors(int type)
		{
			ColrColor[] myColors = new ColrColor[3];
			List<ColrColor> unusedColors = new List<ColrColor>(_colors);
			List<int> myNumbers = new List<int>();
			System.Random randNum = new System.Random();
			for (int currIndex = 0; currIndex < 3; currIndex++)
			{
				// generate a candidate
				int randCandidate = randNum.Next(0, _colors.Length);

				// generate a new candidate as long as we don't get one that isn't in the array
				while (myNumbers.Contains(randCandidate))
				{
					randCandidate = randNum.Next(0, _colors.Length);
				}

				myNumbers.Add(randCandidate);
				unusedColors.Remove(_colors[randCandidate]);
				myColors[currIndex] = _colors[randCandidate];

			}

			_generatedColors = myColors;
			_unusedColors = unusedColors.ToArray();
			for (int i = 0; i < _colorPrefabs.Length; i++)
			{
				_colorPrefabs[i].SetColor(ColrColor.ColourValue(_generatedColors[i].colorName), _generatedColors[i]);
			}

			int properColorIndex = Random.Range(0, _generatedColors.Length);
			_properColor = _generatedColors[properColorIndex];

			if (type == 1)
			{
				_colorText.text = _generatedColors[properColorIndex].colorName.ToString().ToUpper();
				_colorText.color = ColrColor.ColourValue(ColrColor.ColorNames.Black);
			}
			if (type == 2)
			{
				_colorText.text = WordDictionary.Instance.GetShortWord();
				_colorText.color = ColrColor.ColourValue(_properColor.colorName);
			}
			if (type == 3)
			{
				_colorText.text = WordDictionary.Instance.GetLongWord();
				_colorText.color = ColrColor.ColourValue(_properColor.colorName);
			}
			if (type == 4)
			{
				_colorText.text = _unusedColors[Random.Range(0, _unusedColors.Length)].colorName.ToString().ToUpper();
				_colorText.color = ColrColor.ColourValue(_properColor.colorName);
			}
		}

		#endregion
	}
}
