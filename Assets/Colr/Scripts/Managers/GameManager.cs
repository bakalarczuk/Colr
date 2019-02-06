using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Habtic.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Habtic.Games.Colr
{
	public class GameManager : Singleton<GameManager>
	{
		public delegate void LevelStateChanged(LevelStates levelState);
		public static event LevelStateChanged OnLevelStateChanged;

		public LevelStates LevelState {
			get {
				return _levelState;
			}
			set {
				// Debug.Log("GameLevelState: " + value.ToString());
				_levelState = value;
				if (OnLevelStateChanged != null)
				{
					OnLevelStateChanged(value);
				}
			}
		}
		private LevelStates _levelState;

		public delegate void GameSetup();
		public static event GameStarted OnGameSetup;

		public delegate void GameStarted();
		public static event GameStarted OnGameStarted;

		public delegate void InputChekced();
		public static event GameStarted OnInputChecked;
		public delegate void InputCorrect();
		public static event GameStarted OnCorrectInput;
		public delegate void InputIncorrect();
		public static event GameStarted OnIncorrectInput;

		public delegate void LifesChanged(int lifes);
		public static event LifesChanged OnLifesChanged;

		public delegate void ScoreChanged(int score, int realscore);
		public static event ScoreChanged OnScoreChanged;

		public delegate void MessageBroadcasted(BroadcastMessageEventArgs args);
		public static event MessageBroadcasted OnMessageBroadcasted;

		public delegate void NextLevel(NextLevelEventArgs args);
		public static event NextLevel OnNextLevel;

		public delegate void ChallengeOver();
		public static event ChallengeOver OnChallengeOver;

		public delegate void GameOver();
		public static event GameOver OnGameOver;

		public delegate void PauseState(bool isPaused);
		public static event PauseState OnPauseStateChanged;

		#region PROPERTIES
		public Game game;
		public Image progress;
		public Button menuButton;
		private GameObject _selected;

		[SerializeField]
		private ColorWheel _colorWheel;
		[SerializeField]
		private LevelQuestion _questionPanel;
		[SerializeField]
		private Timer _timer;
		[SerializeField]
		private GameObject _rightWrongIndicator;

		private float answerTime = 5;

		public float AnswerTime { get { return answerTime; } set { answerTime = value; } }
		public ColorWheel ColorWheel { get { return _colorWheel; } set { _colorWheel = value; } }

		public bool Paused {
			get {
				return _paused;
			}
			private set {
				_paused = value;
				if (OnPauseStateChanged != null)
				{
					OnPauseStateChanged(value);
				}
			}
		}
		public int TotalLifes { get { return _totalLifes; } set { _totalLifes = value; } }
		public int Lifes {
			get {
				return _lifes;
			}
			set {
				_lifes = value;

				// Notify listeners
				if (OnLifesChanged != null)
					OnLifesChanged(_lifes);

				if (_lifes <= 0)
				{
					_lifes = 0;

					DataHolder.Score = Score;

					// GameOver
					LevelState = LevelStates.gameOver;
				}

			}
		}
		public int Score {
			get {
				return _score;
			}
			set {
				int oldScore = _score;
				_score = value;

				if (OnScoreChanged != null)
				{
					OnScoreChanged(oldScore, value);
				}
			}
		}

		public LevelQuestion QuestionPanel { get { return _questionPanel; } }
		#endregion PROPERTIES

		public Level _level;
		private int _totalLifes = 5;
		private int _lifes = 5;
		private int _score = 0;
		private bool _paused = false;

		public Timer GameTimer { get { return _timer; } set { _timer = value; } }

        void Awake()
        {
            _level = Level.Instance;
        }

        void Start()
        {
			InputManager.Instance.OnTouchDown += OnTouchDown;
		}

		void OnEnable()
        {
            _timer.TimerFinished += TimerGameOver;
            OnGameSetup += Initialize;
            OnGameOver += GameOverMessage;
            OnChallengeOver += ChallengeOverMessage;
            OnLevelStateChanged += InitLevelStateChanged;
            LevelState = LevelStates.setup;
        }

        void OnDisable()
        {
            _timer.TimerFinished -= TimerGameOver;
            OnGameSetup -= Initialize;
            OnGameOver -= GameOverMessage;
            OnChallengeOver -= ChallengeOverMessage;
			OnLevelStateChanged -= InitLevelStateChanged;
        }

        void OnDestroy()
        {
			InputManager.Instance.OnTouchDown -= OnTouchDown;
		}

		private void InitLevelStateChanged(LevelStates levelState)
        {
            switch (levelState)
            {
                case LevelStates.setup:
                    if (OnGameSetup != null)
                    {
                        OnGameSetup();
                    }
                    break;
                case LevelStates.play:
                    if (OnGameStarted != null)
                        OnGameStarted();
                    break;
                case LevelStates.gameOver:
                    if (OnGameOver != null)
                        OnGameOver();
                    break;
                case LevelStates.challengeend:
                    if (OnChallengeOver != null)
						OnChallengeOver();
                    break;
                case LevelStates.correctInput:
                    if (OnInputChecked != null)
                    {
                        OnInputChecked();
                    }
                    OnCorrectTouch();
                    break;
                case LevelStates.IncorrectInput:
                    if (OnInputChecked != null)
                    {
                        OnInputChecked();
                    }
                    OnIncorrectTouch();
                    break;
                case LevelStates.pause:
                    Pause();
                    break;
                case LevelStates.resume:
                    Resume();
                    break;
                default:
                    break;
            }
        }

        void Initialize()
        {
            TotalLifes = _level.TotalLifes;
            Lifes = _totalLifes;
            Score = 0;
			_colorWheel.transform.LeanScale(new Vector3(0, 0, 0), 0);
			_rightWrongIndicator.transform.LeanScale(new Vector3(0, 0, 0), 0);
			progress.fillAmount = 0;
		}

		public void LevelStart()
        {
			progress.fillAmount = (float)(_level.ChallengeCounter+1) / (float)_level.TotalChallenges;
			_rightWrongIndicator.transform.LeanScale(new Vector3(1, 1, 1), 0.2f);
			if (!_colorWheel.tutorialMode)
			{
				_colorWheel.ToDefaultColor(0, 0);
				_colorWheel.ToDefaultColor(1, 0);
				_colorWheel.ToDefaultColor(2, 0);
			}

			_colorWheel.transform.LeanScale(new Vector3(1, 1, 1), 0.2f)
				.setOnComplete(()=> {
					_colorWheel.StartNewLevel(_level);
				});
		}

		private void OnTouchDown(YTouchEventArgs touchEventArgs)
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{		 
				_timer.TimerReset();
				if (CheckColorMatchAtPosition(touchEventArgs))
				{
					LevelState = LevelStates.correctInput;
				}
				else
				{
					LevelState = LevelStates.IncorrectInput;
				}
			}
		}


		private void OnCorrectTouch()
        {
            Debug.Log("Correct touch detected");
            if (OnCorrectInput != null)
            {
                OnCorrectInput();
            }
            float _currentLevel = _level.CurrentLevel;
            _level.CorrectInARowCounter = _level.CorrectInARowCounter + 1;
            _level.ChallengeCounter = _level.ChallengeCounter + 1;
			_level.CorrectCounter = _level.CorrectCounter + 1;

			AddScore(_level.ScorePerCorrectAnswer);
			CheckChallege();
            if (_currentLevel == _level.CurrentLevel)
            {
                LevelState = LevelStates.play;
            }
        }

        private void OnIncorrectTouch()
        {
            Debug.Log("Incorrect touch detected");
            if (OnIncorrectInput != null)
            {
                OnIncorrectInput();
            }
            RemoveLife();
			_level.CorrectInARowCounter = 0;
			_level.IncorrectCounter = _level.IncorrectCounter + 1;
            _level.ChallengeCounter = _level.ChallengeCounter + 1;
			CheckChallege();
			if (Lifes > 0)
            {
                LevelState = LevelStates.play;
            }
        }

        public void Pause()
        {
            Paused = true;
            LeanTween.pauseAll();
        }

        public void Resume()
        {
            Paused = false;
            LeanTween.resumeAll();
        }

        public void AddScore(int score)
        {
            Score = Score + score;

            if (_level.CorrectInARowCounter >= _level.NextLevel && _level.ChallengeCounter < _level.TotalChallenges)
            {
                IncreaseLevel();
            }
        }


		public void CheckChallege()
		{
			progress.fillAmount = (float)(_level.ChallengeCounter+1) / (float)_level.TotalChallenges;
			if (_level.ChallengeCounter >= _level.TotalChallenges)
			{
				LevelState = LevelStates.challengeend;
			}
		}


		public void RemoveLife()
        {
            Lifes = _lifes - 1;
        }

        private void IncreaseLevel()
        {
            _level.CurrentLevel++;

			// Notify listeners
			if (OnNextLevel != null)
            {
                OnNextLevel(new NextLevelEventArgs
                {
                    ScorePerCorrectAnswer = _level.ScorePerCorrectAnswer,
                    TotalLifes = _level.TotalLifes
                });
            }
        }

        private void BroadCastMessage(MessageType type, string message)
        {
            if (OnMessageBroadcasted != null)
                OnMessageBroadcasted(new BroadcastMessageEventArgs { Type = type, Message = message });
        }

        public void disableMenu(bool value)
        {
            menuButton.interactable = !value;
        }

        private void GameOverMessage()
        {
            GameOverController.Instance.LoadScoreMenu();
        }

        private void ChallengeOverMessage()
        {
            GameOverController.Instance.LoadScoreMenu(false);
        }

        private void TimerGameOver()
        {
            LevelState = LevelStates.IncorrectInput;
        }

		private bool CheckColorMatchAtPosition(YTouchEventArgs touchEventArgs)
		{
			PointerEventData pointerData = new PointerEventData(EventSystem.current);
			pointerData.position = touchEventArgs.TouchStart;
			List<RaycastResult> results = new List<RaycastResult>();
			EventSystem.current.RaycastAll(pointerData, results);
			if (results.Count > 0)
			{
				if (results[0].gameObject != null)
				{
					if (results[0].gameObject.GetComponent<WheelColor>() != null)
					{
						if (results[0].gameObject.GetComponent<WheelColor>().GetColor.Equals(_colorWheel._properColor))
						{
							return true;
						}
					}
				}
			}
			return false;
		}	  
	}

	public class NextLevelEventArgs : EventArgs
    {
        public int ScorePerCorrectAnswer { get; set; }
        public int TotalLifes { get; set; }
    }

    public enum MessageType
    {
        INFO,
        WARNING,
        POSITIVE,
        NEGATIVE
    }

    public enum LevelStates
    {
        none,
        setup,
        play,
        correctInput,
        IncorrectInput,
        nextLevel,
        replay,
        gameOver,
		challengeend,
        pause,
        resume
    }

    public class BroadcastMessageEventArgs : EventArgs
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }
    }
}
