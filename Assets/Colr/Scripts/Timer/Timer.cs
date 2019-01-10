using System;
using UnityEngine;
using TMPro;

namespace Habtic.Games.Colr
{

    public delegate void TimerEventHandler();

    public class Timer : MonoBehaviour
    {

        private float _secondsLeft;
        public TMP_Text TimeText;
        private int _seconds = 59;
        private int _minutes = 1;
        public event TimerEventHandler TimerFinished;
        // public event TimerEventHandler TimerStopped;

        public bool Running { get; private set; }
        public float ElapsedTime { get; private set; }
        public float Duration { get; private set; }

        public bool Done
        {
            get
            {
                return ElapsedTime >= Duration;
            }
        }

        #region Unity Methods

        private void Update()
        {
            if (Running)
            {
                Tick();
                UpdateTimeText();
            }
        }

        private void OnDisable()
        {
            TimeText.text = "00:00";
        }

        #endregion

        #region Methods

        public void TimerReset()
        {
            if (Running)
            {
                GameManager.OnLevelStateChanged -= OnLevelStateChanged;
            }
            Running = false;
            ElapsedTime = 0f;
            Duration = 0f;
            ResetTimeText();
        }

        public void Tick()
        {
            if (Done)
            {
                Running = false;
                TimeText.text = "00:00";
                GameManager.OnLevelStateChanged -= OnLevelStateChanged;
                OnTimerFinished();
                return;
            }

            ElapsedTime += Time.deltaTime;
            _secondsLeft = Duration - ElapsedTime;
        }

        public void StartTimer(float minutes, float seconds, bool autoStart = false)
        {
            float duration = (minutes * 60) + seconds;
            StartTimer(duration, autoStart);
        }

        public void StartTimer(float seconds, bool autoStart = false)
        {
            Running = autoStart;
            Duration = seconds;
            ElapsedTime = 0f;
            UpdateTimeText();
            GameManager.OnLevelStateChanged += OnLevelStateChanged;
        }

        private void OnLevelStateChanged(LevelStates levelState)
        {
            switch (levelState)
            {
                case LevelStates.pause:
                    Running = false;
                    break;
                case LevelStates.play:
                case LevelStates.resume:
                    Running = true;
                    break;
                default:
                    break;
            }
        }

        private void PauseTimer()
        {
            Running = false;
        }

        private void ResumeTimer()
        {
            Running = true;
        }

        // public void StopTimer()
        // {
        //     Running = false;
        //     OnTimerStopped();
        // }

        private void UpdateTimeText()
        {
            _minutes = Mathf.FloorToInt(_secondsLeft / 60);
            if(_minutes < 0) {
                _minutes = 0;
            }
            float sl = _secondsLeft % 60;
            if (sl > 0)
            {
                _seconds = Mathf.FloorToInt(sl);
            }
            else if(sl < 0)
            {
                _seconds = 0;
            }
            else
            {
                _seconds = 0;
            }

            TimeText.text = _minutes.ToString("00") + ":" + ((int)_seconds).ToString("00");
        }

        private void ResetTimeText()
        {
            _minutes = 0;
            _secondsLeft = 0;
            TimeText.text = _minutes.ToString("00") + ":" + ((int)_seconds).ToString("00");
        }

        #endregion
        #region Events

        // private void OnTimerStopped()
        // {
        //     if (TimerStopped != null)
        //     {
        //         TimerStopped();
        //     }
        // }

        private void OnTimerFinished()
        {
            if (TimerFinished != null)
            {
                TimerFinished();
            }
        }

        #endregion
    }
}
