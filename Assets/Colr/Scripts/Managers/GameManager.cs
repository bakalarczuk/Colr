using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Habtic.Managers;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class GameManager : Singleton<GameManager>
    {
        public delegate void LevelStateChanged(LevelStates levelState);
        public static event LevelStateChanged OnLevelStateChanged;

        public LevelStates LevelState
        {
            get
            {
                return _levelState;
            }
            set
            {
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
        private LevelQuestion _questionPanel;
        [SerializeField]
        private Timer _timer;					

		public bool Paused
        {
            get
            {
                return _paused;
            }
            private set
            {
                _paused = value;
                if (OnPauseStateChanged != null)
                {
                    OnPauseStateChanged(value);
                }
            }
        }
        public int TotalLifes { get { return _totalLifes; } set { _totalLifes = value; } }
        public int Lifes
        {
            get
            {
                return _lifes;
            }
            set
            {
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
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
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
        }

        void Start()
        {
        }

        void OnEnable()
        {
            _timer.TimerFinished += TimerGameOver;
            OnGameSetup += Initialize;
            OnGameOver += GameOverMessage;
            OnGameStarted += LevelStart;
            OnLevelStateChanged += InitLevelStateChanged;
            LevelState = LevelStates.setup;
        }

        void OnDisable()
        {
            _timer.TimerFinished -= TimerGameOver;
            OnGameSetup -= Initialize;
            OnGameOver -= GameOverMessage;
            OnGameStarted -= LevelStart;
            OnLevelStateChanged -= InitLevelStateChanged;
        }

        void OnDestroy()
        {
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
            _timer.TimerReset();
            _timer.StartTimer(59);
        }

        private void LevelStart()
        {
            _questionPanel.SetQuestionText(_level.CurrentDirOrMove);

            _colorWheel.StartNewLevel(_level.CurrentLevel);
        }

        private void OnSwipedCheck(YTouchEventArgs touchEventArgs)
        {
            if (LevelState == LevelStates.play || LevelState == LevelStates.resume)
            {
                if (_level.CurrentDirOrMove)
                {
                    Vector2 _fishSchoolDirection = Vector2.zero;

                    switch (_colorWheel.MovingDirection)
                    {
                        case SwipeDirection.Down:
                            _fishSchoolDirection = new Vector2(0, -1);
                            break;
                        case SwipeDirection.Left:
                            _fishSchoolDirection = new Vector2(-1, 0);
                            break;
                        case SwipeDirection.Right:
                            _fishSchoolDirection = new Vector2(1, 0);
                            break;
                        case SwipeDirection.Up:
                            _fishSchoolDirection = new Vector2(0, 1);
                            break;
                        default:
                            break;
                    }

                    if (_fishSchoolDirection == touchEventArgs.SwipeDirection)
                    {
                        LevelState = LevelStates.correctInput;
                    }
                    else
                    {
                        LevelState = LevelStates.IncorrectInput;
                    }
                }
                //else
                //{
                //    WheelColor _f = _colorWheel.GrabCentralFish();

                //    Vector2 _fishDirection = Vector2.zero;

                //    switch (_f.StartDirection)
                //    {
                //        case SwipeDirection.Down:
                //            _fishDirection = new Vector2(0, -1);
                //            break;
                //        case SwipeDirection.Left:
                //            _fishDirection = new Vector2(-1, 0);
                //            break;
                //        case SwipeDirection.Right:
                //            _fishDirection = new Vector2(1, 0);
                //            break;
                //        case SwipeDirection.Up:
                //            _fishDirection = new Vector2(0, 1);
                //            break;
                //        default:
                //            break;
                //    }

                //    if (_fishDirection == touchEventArgs.SwipeDirection)
                //    {
                //        LevelState = LevelStates.correctInput;
                //    }
                //    else
                //    {
                //        LevelState = LevelStates.IncorrectInput;
                //    }
                //}
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
            _level.CorrectCounter = _level.CorrectCounter + 1;
            AddScore(_level.ScorePerFish);

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
            _level.CorrectCounter = 0;
            if (Lifes > 0)
            {
                _colorWheel.StartNewLevel(_level.CurrentLevel);
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

            if (_level.CorrectCounter >= _level.NextLevel)
            {
                IncreaseLevel();
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
                    HorFishes = _level.HorFishes,
                    VerFishes = _level.VerFishes,
                    DirAndMove = _level.DirAndMove,
                    ScorePerFish = _level.ScorePerFish,
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

        private void TimerGameOver()
        {
            LevelState = LevelStates.gameOver;
        }
    }

    public class NextLevelEventArgs : EventArgs
    {
        public int HorFishes { get; set; }
        public int VerFishes { get; set; }
        public int DirAndMove { get; set; }
        public int ScorePerFish { get; set; }
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
        pause,
        resume
    }

    public class BroadcastMessageEventArgs : EventArgs
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }
    }
}
