using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Habtic.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Habtic.Games.Colr
{
	public class IntroLevelManager : Singleton<IntroLevelManager>
	{
		[SerializeField]
		private Game _game;

		[SerializeField]
		private Image _progress;

		private string TutorialPrinted, TutorialTextColor;

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

		public delegate void GameOver();
		public static event GameOver OnGameOver;

		public delegate void PauseState(bool isPaused);
		public static event PauseState OnPauseStateChanged;

		#region PROPERTIES
		public Button menuButton;
		private GameObject _selected;

		[SerializeField]
		private ColorWheel _colorWheel;
		[SerializeField]
		private IntroLevelQuestion _questionPanel;
		[SerializeField]
		private Timer _timer;

		[SerializeField]
		private MessagePanelIntro _introEndMassagePanel;

		[SerializeField]
		private AnswerIndicator _notifications;

		private int correctCount = 0;
		private int incorrectCount = 0;

		[SerializeField]
		private GameObject _rightWrongIndicator;

		private LTDescr _tweenM;

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
		#endregion PROPERTIES

		private Level _level;
		private int _totalLifes = 5;
		private int _lifes = 5;
		private int _score = 0;
		private bool _paused = false;

		void Awake()
		{
			_level = Level.Instance;
			TutorialPrinted = _game.LocalizedStrings["game_level_instruction_printed_color"];
			TutorialTextColor = _game.LocalizedStrings["game_level_instruction_text_color"];
		}

		void Start()
		{
			// InputManager.Instance.OnSwiped += OnSwipedCheck;
		}

		void OnEnable()
		{
			OnGameSetup += Initialize;
			OnGameStarted += LevelStart;
			OnLevelStateChanged += InitLevelStateChanged;
			LevelState = LevelStates.setup;
		}

		void OnDisable()
		{
			OnGameSetup -= Initialize;
			OnGameStarted -= LevelStart;
			OnLevelStateChanged -= InitLevelStateChanged;
		}

		void OnDestroy()
		{
			// InputManager.Instance.OnSwiped -= OnSwipedCheck;
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
					break;
				case LevelStates.resume:
					;
					break;
				default:
					break;
			}
		}

		void Initialize()
		{
			_level.CurrentLevel = 5;
			TotalLifes = _level.TotalLifes;
			Lifes = _totalLifes;
			Score = 0;
			_introEndMassagePanel.Hide();
			_questionPanel.Hide();
			_progress.fillAmount = 0.25f;
			_colorWheel.gameObject.SetActive(false);
			_rightWrongIndicator.SetActive(false);
		}

		public void LevelStart()
		{
			correctCount = 0;
			incorrectCount = 0;
			_notifications.CorrectCount.text = correctCount.ToString();
			_notifications.IncorrectCount.text = incorrectCount.ToString();
			StartTutorialFive();// One();
		}

		public void StartTutorialOne()
		{
			_colorWheel.gameObject.SetActive(true);
			_rightWrongIndicator.SetActive(true);
			_timer.TimerReset();
			_timer.StartTimer(5);
			_timer.ResumeTimer();
			InputManager.Instance.OnTouchDown += OnTouchDownTutorialOne;
			_timer.TimerFinished += OnTimerFinishedT1;
			_questionPanel.SetQuestionText(TutorialPrinted);
			_colorWheel.SelectIntroColors(1);
		}

		private void OnTimerFinishedT1()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT1;
				_timer.PauseTimer();
				incorrectCount += 1;
				StartCoroutine(StepEndRoutine(1, "game_notify_time_out", false));
			}
		} 

		private void OnTouchDownTutorialOne(YTouchEventArgs touchEventArgs)
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				InputManager.Instance.OnTouchDown -= OnTouchDownTutorialOne;
				_timer.PauseTimer();
				if (CheckColorMatchAtPosition(touchEventArgs))
				{
					correctCount += 1;
					StartCoroutine(StepEndRoutine(1, "game_notify_good", true));
				}
				else
				{
					incorrectCount += 1;
					StartCoroutine(StepEndRoutine(1, "game_notify_wrong_answer", false));
				}
			}
		}

		public void StartTutorialTwo()
		{
			_timer.TimerReset();
			_timer.StartTimer(5);
			_timer.ResumeTimer();
			InputManager.Instance.OnTouchDown += OnTouchDownTutorialTwo;
			_timer.TimerFinished += OnTimerFinishedT2;
			_questionPanel.SetQuestionText(TutorialTextColor);
			_colorWheel.SelectIntroColors(2);
		}

		private void OnTimerFinishedT2()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT2;
				_timer.PauseTimer();
				incorrectCount += 1;
				StartCoroutine(StepEndRoutine(2, "game_notify_time_out", false));
			}
		}

		private void OnTouchDownTutorialTwo(YTouchEventArgs touchEventArgs)
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				InputManager.Instance.OnTouchDown -= OnTouchDownTutorialTwo;
				_timer.PauseTimer();
				if (CheckColorMatchAtPosition(touchEventArgs))
				{
					correctCount += 1;
					StartCoroutine(StepEndRoutine(2, "game_notify_good", true));
				}
				else
				{
					incorrectCount += 1;
					StartCoroutine(StepEndRoutine(2, "game_notify_wrong_answer", false));
				}
			}
		}

		private void StartTutorialThree()
        {
			_timer.TimerReset();
			_timer.StartTimer(5);
			_timer.ResumeTimer();
			InputManager.Instance.OnTouchDown += OnTouchDownTutorialThree;
			_timer.TimerFinished += OnTimerFinishedT3;
			_questionPanel.SetQuestionText(TutorialTextColor);
			_colorWheel.SelectIntroColors(3);
		}

		private void OnTimerFinishedT3()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT3;
				_timer.PauseTimer();
				incorrectCount += 1;
				StartCoroutine(StepEndRoutine(3, "game_notify_time_out", false));
			}
		}

		private void OnTouchDownTutorialThree(YTouchEventArgs touchEventArgs)
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				InputManager.Instance.OnTouchDown -= OnTouchDownTutorialThree;
				_timer.PauseTimer();
				if (CheckColorMatchAtPosition(touchEventArgs))
				{
					correctCount += 1;
					StartCoroutine(StepEndRoutine(3, "game_notify_good", true));
				}
				else
				{
					incorrectCount += 1;
					StartCoroutine(StepEndRoutine(3, "game_notify_wrong_answer", false));
				}
			}
		}

		private void StartTutorialFour()
		{
			_timer.TimerReset();
			_timer.StartTimer(5);
			_timer.ResumeTimer();
			InputManager.Instance.OnTouchDown += OnTouchDownTutorialFour;
			_timer.TimerFinished += OnTimerFinishedT4;
			_questionPanel.SetQuestionText(TutorialTextColor);
			_colorWheel.SelectIntroColors(4);
		}

		private void OnTimerFinishedT4()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT4;
				_timer.PauseTimer();
				incorrectCount += 1;
				StartCoroutine(StepEndRoutine(4, "game_notify_time_out", false));
			}
		}

		private void OnTouchDownTutorialFour(YTouchEventArgs touchEventArgs)
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				InputManager.Instance.OnTouchDown -= OnTouchDownTutorialFour;
				_timer.PauseTimer();
				if (CheckColorMatchAtPosition(touchEventArgs))
				{
					correctCount += 1;
					StartCoroutine(StepEndRoutine(4, "game_notify_good", true));
				}
				else
				{
					incorrectCount += 1;
					StartCoroutine(StepEndRoutine(4, "game_notify_wrong_answer", false));
				}
			}
		}											  

		private void StartTutorialFive()
        {
            _introEndMassagePanel.Show();
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

		private IEnumerator StepEndRoutine(int tutorial, string notificationType, bool correct)
		{
			yield return new WaitForSeconds(0.25f);
			_notifications.CorrectCount.text = correctCount.ToString();
			_notifications.IncorrectCount.text = incorrectCount.ToString();
			if (correct)
			{
				_notifications.CorrectText.text = _game.LocalizedStrings[notificationType];
				_tweenM = LeanTween.scale(_notifications.CorrectIndicator, Vector3.one, 1f)
					.setEase(LeanTweenType.easeInOutSine)
					.setOnComplete(() => {
						_notifications.CorrectIndicator.transform.localScale = Vector3.zero;
						GameManager.Instance.LevelStart();
						if (tutorial == 1)
						{
							_progress.fillAmount += 0.25f;
							StartTutorialTwo();
						}
						if (tutorial == 2)
						{
							_progress.fillAmount += 0.25f;
							StartTutorialThree();
						}
						if (tutorial == 3)
						{
							_progress.fillAmount += 0.25f;
							StartTutorialFour();
						}
						if (tutorial == 4)
						{
							StartTutorialFive();
						}

						StopCoroutine("StepEndRoutine");
					});
			} else
			{
				_notifications.IncorrectText.text = _game.LocalizedStrings[notificationType];
				_tweenM = LeanTween.scale(_notifications.IncorrectIndicator, Vector3.one, 1f)
					.setEase(LeanTweenType.easeInOutSine)
					.setOnComplete(() => {
						_notifications.IncorrectIndicator.transform.localScale = Vector3.zero;
						GameManager.Instance.LevelStart();
						if (tutorial == 1)
						{
							_progress.fillAmount += 0.25f;
							StartTutorialTwo();
						}
						if (tutorial == 2)
						{
							_progress.fillAmount += 0.25f;
							StartTutorialThree();
						}
						if (tutorial == 3)
						{
							_progress.fillAmount += 0.25f;
							StartTutorialFour();
						}
						if (tutorial == 4)
						{
							StartTutorialFive();
						}

						StopCoroutine("StepEndRoutine");
					});
			}
		}

		private void OnCorrectTouch()
		{
			Debug.Log("Correct touch detected");
			if (OnCorrectInput != null)
			{
				OnCorrectInput();
			}
		}

		private void OnIncorrectTouch()
		{
			Debug.Log("Incorrect touch detected");
			if (OnIncorrectInput != null)
			{
				OnIncorrectInput();
			}
		}
	}
}
