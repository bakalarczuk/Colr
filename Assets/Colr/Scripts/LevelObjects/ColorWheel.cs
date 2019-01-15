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
		private WheelColor[] _colorPrefabs;

		[SerializeField]
		public TMP_Text _colorText;

        private SwipeDirection _movingDirection;

        public SwipeDirection MovingDirection
        {
            get { return _movingDirection; }
            private set
            {
                _movingDirection = value;
            }
        }

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

		public void Cleanupfishes()
        {

        }

        public void ComeIn()
        {
            if (_tweenM != null)
            {
                if (LeanTween.isTweening(_tweenM.id))
                {
                    LeanTween.cancel(_tweenM.id);
                }
            }

            transform.localPosition = Vector3.zero;
        }

        public void SnapOut()
        {

        }


        private void MoveWheelOut(float _movingTime = 5f)
        {
            if (_tweenM != null)
            {
                if (LeanTween.isTweening(_tweenM.id))
                {
                    LeanTween.cancel(_tweenM.id);
                }
            }

            bool leftOrRightDirection = HelperMethods.CoinFlip();
            if (leftOrRightDirection)
            {
                MovingDirection = SwipeDirection.Right;
            }
            else
            {
                MovingDirection = SwipeDirection.Left;
            }

			MoveWheelOutTo(MovingDirection, _movingTime);
        }

        public void MoveWheelOutTo(SwipeDirection _movingDirection, float _movingTime = 5f)
        {
            float _schoolMovingTime = _movingTime;
            float _moveTo = 0f;

            switch (_movingDirection)
            {
                case SwipeDirection.Left:
                    _moveTo = -Screen.width * 1f;
                    break;
                case SwipeDirection.Right:
                    _moveTo = Screen.width * 1f;
                    break;
                default:

                    break;
            }
        }

        public void StartNewLevel(int levelNR)
        {
            ComeIn();
			SelectColors();
        }

        public void StartTutorialOne(int levelNR)
        {
            ComeIn();
        }

        public void StartTutorialTwo(int levelNR)
        {
            ComeIn();
        }

		public void SelectColors()
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
			_colorText.text = _generatedColors[properColorIndex].colorName.ToString();
			_properColor = _generatedColors[properColorIndex];
		}

		#endregion
	}
}
