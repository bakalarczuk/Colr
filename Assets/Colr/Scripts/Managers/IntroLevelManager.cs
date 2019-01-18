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

		private string TutorialOne, TutorialTwo, TutorialThree, TutorialFour;

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
		private TMPro.TMP_Text _notifications;

		[SerializeField]
		private MessagePanelIntro _introEndMassagePanel;

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
			TutorialOne = _game.LocalizedStrings["game_introlevel_text_tutorialone"];
			TutorialTwo = _game.LocalizedStrings["game_introlevel_text_tutorialtwo"];
			TutorialThree = _game.LocalizedStrings["game_introlevel_text_tutorialthree"];
			TutorialFour = _game.LocalizedStrings["game_introlevel_text_tutorialfour"];
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
					break;
				case LevelStates.IncorrectInput:
					if (OnInputChecked != null)
					{
						OnInputChecked();
					}
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
			_notifications.gameObject.SetActive(false);
		}

		public void LevelStart()
		{
			StartTutorialOne();
		}

		public void StartTutorialOne()
		{
			_timer.TimerReset();
			_timer.StartTimer(5);
			_timer.ResumeTimer();
			InputManager.Instance.OnTouchDown += OnTouchDownTutorialOne;
			_timer.TimerFinished += OnTimerFinishedT1;
			_questionPanel.SetQuestionText(TutorialOne);
			_colorWheel.SelectIntroColors(1);
		}

		private void OnTimerFinishedT1()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT1;
				_timer.PauseTimer();
				StartCoroutine(StepEndRoutine(1, "game_notify_time_out"));
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
					StartCoroutine(StepEndRoutine(1, "game_notify_good"));
				}
				else
				{
					StartCoroutine(StepEndRoutine(1, "game_notify_wrong_answer"));
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
			_questionPanel.SetQuestionText(TutorialTwo);
			_colorWheel.SelectIntroColors(2);
		}

		private void OnTimerFinishedT2()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT2;
				_timer.PauseTimer();
				StartCoroutine(StepEndRoutine(2, "game_notify_time_out"));
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
					StartCoroutine(StepEndRoutine(2, "game_notify_good"));
				}
				else
				{
					StartCoroutine(StepEndRoutine(2, "game_notify_wrong_answer"));
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
			_questionPanel.SetQuestionText(TutorialThree);
			_colorWheel.SelectIntroColors(3);
		}

		private void OnTimerFinishedT3()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT3;
				_timer.PauseTimer();
				StartCoroutine(StepEndRoutine(3, "game_notify_time_out"));
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
					StartCoroutine(StepEndRoutine(3, "game_notify_good"));
				}
				else
				{
					StartCoroutine(StepEndRoutine(3, "game_notify_wrong_answer"));
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
			_questionPanel.SetQuestionText(TutorialFour);
			_colorWheel.SelectIntroColors(4);
		}

		private void OnTimerFinishedT4()
		{
			if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
			{
				_timer.TimerFinished -= OnTimerFinishedT4;
				_timer.PauseTimer();
				StartCoroutine(StepEndRoutine(4, "game_notify_time_out"));
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
					StartCoroutine(StepEndRoutine(4, "game_notify_good"));
				}
				else
				{
					StartCoroutine(StepEndRoutine(4, "game_notify_wrong_answer"));
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
				for (int i = 0; i < _colorWheel.ColorPrefabs.Length; i++)
				{
					RaycastResult result = results.Find(r => r.gameObject.GetComponent<WheelColor>() == _colorWheel.ColorPrefabs[i]);
					if (result.gameObject != null)
						if (result.gameObject.GetComponent<WheelColor>().GetColor.Equals(_colorWheel._properColor))
							return true;
				}
			}
			return false;
		}

		private IEnumerator StepEndRoutine(int tutorial, string notificationType)
		{
			yield return new WaitForSeconds(0.25f);

			_notifications.gameObject.SetActive(true);
			_notifications.text = _game.LocalizedStrings[notificationType];

			yield return new WaitForSeconds(2f);
			_notifications.gameObject.SetActive(false);

			yield return new WaitForSeconds(0.25f);
			if (tutorial == 1)
			{
				StartTutorialTwo();
			}
			if (tutorial == 2)
			{
				StartTutorialThree();
			}
			if (tutorial == 3)
			{
				StartTutorialFour();
			}
			if (tutorial == 4)
			{
				StartTutorialFive();
			}

			StopCoroutine("StepEndRoutine");
		}
	}
}
