using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Habtic.Games.Colr
{
    public class ScoreMenuController : MonoBehaviour
    {
        [SerializeField]
        private Game _game;
        public TMP_Text ScoreText, messageText, levelText;
        public WheelColor StarColor;
        public Image[] Stars;

        private GameManager _gameManager;
        private LevelManager _levelManager;
        private Level _level;
        public Button replay;

        private const int ONE_STAR_SCORE = 200;
        private const int TWO_STAR_SCORE = 500;
        private const int THREE_STAR_SCORE = 1000;

        private LTDescr _tween;

        void OnEnable()
        {
            ResetStars();
            _gameManager = GameManager.Instance;
            _levelManager = LevelManager.Instance;
            _level = Level.Instance;
            messageText.text = "";
            replay.interactable = true;
            SetScore();
        }

        void OnDisable()
        {
            ResetStars();
        }

        public void Quit()
        {
            _levelManager.LoadScene(0);
        }

        public void Replay()
        {
            replay.interactable = false;
            _level.SetDifficulty(_level.Difficulty);
            _levelManager.LoadScene(1);
        }

        private void SetScore()
        {
            int score = _gameManager.Score;
            int nrOfStars = 0;

            DataHolder.Score = score;

            if (score > DataHolder.highscore)
            {
                DataHolder.highscore = score;
                messageText.text = _game.LocalizedStrings["game_scoremenu_text_messagenewhighscore"];
            }

            levelText.text = $"{_game.LocalizedStrings["game_flash_text_level"]} {_level.CurrentLevel.ToString()}!";

            ScoreText.text = score.ToString();

            nrOfStars = 1;

            if (score >= TWO_STAR_SCORE && score < THREE_STAR_SCORE)
            {
                nrOfStars = 2;
            }
            else if (score >= THREE_STAR_SCORE)
            {
                nrOfStars = 3;
            }

            for (int i = 0; i < Stars.Length; i++)
            {
                if (i < nrOfStars)
                {
                    tweenStar(i);
                }

            }
        }

        private void tweenStar(int star)
        {
            _tween = LeanTween.alpha(Stars[star].GetComponent<RectTransform>(), 255, 0.5f);
            _tween.setDelay(star * 1f);
            _tween.setOnComplete(() =>
            {
                _tween = LeanTween.scale(Stars[star].gameObject, new Vector3(2f, 2f, 2f), 0.2f);
                _tween.setLoopPingPong(1);
            });
        }

        private void ResetStars()
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                Image star = Stars[i];
                Stars[i].color = new Color(star.color.r, star.color.g, star.color.b, 0f);
            }
        }
    }
}
