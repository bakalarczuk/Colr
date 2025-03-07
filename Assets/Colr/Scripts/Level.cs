﻿using UnityEngine;

namespace Habtic.Games.Colr
{
    public struct LevelProgression
    {
        public float Start { get; set; }
        public int Max { get; set; }
        public float Increase { get; set; }
    }

    public enum LevelDifficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    public class Level
    {
        private static Level _instance;

        public static Level Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Level();
                }

                return _instance;
            }
        }

		#region PROPERTIES
		public int ScorePerCorrectAnswer { get; private set; }
        public int NextLevel { get; private set; }
        public int CorrectInARowCounter { get; set; }
		public int CorrectCounter { get; set; } = 0;
		public int IncorrectCounter { get; set; } = 0;
		public int TotalLifes { get; private set; }
        public int TotalChallenges { get; private set; }
        public int ChallengeCounter { get; set; }
        public int Complexity { get; set; }
		public LevelDifficulty Difficulty { get; private set; }
        public int CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            set
            {
                _currentLevel = value;
                UpdateLevelSettings(_currentLevel);
            }
        }

        #endregion PROPERTIES

        private int _currentLevel = 1;
        private LevelProgression _scoreProgression;
        private LevelProgression _nextLevelProgression;
        private LevelProgression _movingOutTime;

        private Level()
        {
            TotalLifes = 5;
			ChallengeCounter = 0;
            UpdateLevelSettings(_currentLevel);
			Complexity = 1;
			TotalChallenges = 15;
			CorrectCounter = 0;
			IncorrectCounter = 0;
		}

		public void SetDifficulty(LevelDifficulty difficulty)
        {
            Difficulty = difficulty;

            switch (difficulty)
            {
                case LevelDifficulty.EASY:
                    CurrentLevel = 1;
                    TotalLifes = 5;
					TotalChallenges = 15;
                    break;
                case LevelDifficulty.MEDIUM:
                    CurrentLevel = 5;
                    TotalLifes = 4;
					TotalChallenges = 30;
					break;
                case LevelDifficulty.HARD:
                    CurrentLevel = 8;
                    TotalLifes = 3;
					TotalChallenges = 60;
					break;
                default:
                    CurrentLevel = 1;
                    TotalLifes = 5;
					TotalChallenges = 15;
					break;
            }
        }

        private void UpdateLevelSettings(int level)
        {
            ScorePerCorrectAnswer = 50 * level;
            NextLevel = 3;
			CorrectInARowCounter = 0;
        }
    }
}
