using UnityEngine;

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
        public int CorrectCounter { get; set; }
        public int TotalLifes { get; private set; }
        public int MovingOutTime { get; private set; }
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
            _scoreProgression = new LevelProgression
            {
                Start = 0f,
                Max = 200,
                Increase = 10f
            };


            _nextLevelProgression = new LevelProgression
            {
                Start = 2f,
                Max = 15,
                Increase = 0.8f
            };


            _movingOutTime = new LevelProgression
            {
                Start = 27f,
                Max = 4,
                Increase = -2f
            };

            TotalLifes = 5;

            UpdateLevelSettings(_currentLevel);
        }

        public void SetDifficulty(LevelDifficulty difficulty)
        {
            Difficulty = difficulty;

            switch (difficulty)
            {
                case LevelDifficulty.EASY:
                    CurrentLevel = 1;
                    TotalLifes = 5;
                    break;
                case LevelDifficulty.MEDIUM:
                    CurrentLevel = 5;
                    TotalLifes = 4;
                    break;
                case LevelDifficulty.HARD:
                    CurrentLevel = 8;
                    TotalLifes = 3;
                    break;
                default:
                    CurrentLevel = 1;
                    TotalLifes = 5;
                    break;
            }
        }

        private void UpdateLevelSettings(int level)
        {
            ScorePerCorrectAnswer = CalculateLevelSetting(_scoreProgression, level);
            NextLevel = CalculateLevelSetting(_nextLevelProgression, level);
            MovingOutTime = CalculateLevelSetting(_movingOutTime, level);
            CorrectCounter = 0;
        }

        private int CalculateLevelSetting(LevelProgression settings, int level)
        {
            float calc = settings.Start + (level * settings.Increase);
            return Mathf.FloorToInt(Mathf.Clamp(Mathf.FloorToInt(calc), settings.Start, settings.Max));
        }
    }
}
