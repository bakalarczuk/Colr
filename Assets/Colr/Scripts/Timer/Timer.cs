using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{

    public delegate void TimerEventHandler();

    public class Timer : MonoBehaviour
    {

        public float _secondsLeft;
        public Image timerIndicator;
        public event TimerEventHandler TimerFinished;
        // public event TimerEventHandler TimerStopped;

        public bool Running { get; set; }
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
            timerIndicator.fillAmount = 1;
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
				timerIndicator.fillAmount = 1;
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

        public void PauseTimer()
        {
            Running = false;
        }

		public void ResumeTimer()
        {
            Running = true;
        }

        // public void StopTimer()
        // {
        //     Running = false;
        //     OnTimerStopped();
        // }

        public void UpdateTimeText()
        {
			timerIndicator.fillAmount = _secondsLeft / Duration;
        }

        private void ResetTimeText()
        {
            _secondsLeft = 0;
			timerIndicator.fillAmount = _secondsLeft / Duration;
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
