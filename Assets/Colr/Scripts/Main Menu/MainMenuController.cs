using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField]
        private Game _game;

        private Vector2 enableTransf;
        private Button _lastSelected;
        private LevelManager _levelManager;
        private Level _level;
        public TMP_Text highscore, score;

        public Button StartButton;
        public Button IntroButton;

        public GameObject DifficultyButtonsHolder;

        void Start()
        {
            _levelManager = LevelManager.Instance;
            _level = Level.Instance;
        }
        private void OnEnable()
        {
            enableTransf = new Vector2(-20, -20);
            StartButton.interactable = true;
            if (DataHolder.PlayedIntroLevel)
            {
                DifficultyButtonsHolder.SetActive(true);
                StartButton.GetComponentInChildren<TMP_Text>().text = _game.LocalizedStrings["game_mainmenu_buttontext_startgame"];
                IntroButton.gameObject.SetActive(true);
                IntroButton.interactable = true;
            }
            else
            {
                DifficultyButtonsHolder.SetActive(false);
                StartButton.GetComponentInChildren<TMP_Text>().text = _game.LocalizedStrings["game_mainmenu_buttontext_starttutorial"];
                IntroButton.gameObject.SetActive(false);
                IntroButton.interactable = false;

            }
            highscore.text = DataHolder.highscore.ToString();
            score.text = DataHolder.Score.ToString();
        }

        public void StartGame()
        {
            if (!DataHolder.PlayedIntroLevel)
            {
                StartIntroGame();
            }
            else
            {
                StartButton.interactable = false;
                IntroButton.interactable = false;
                _levelManager.LoadScene(1);
            }
        }

        public void StartIntroGame()
        {
            StartButton.interactable = false;
            IntroButton.interactable = false;
            DataHolder.PlayedIntroLevel = true;
            _levelManager.LoadScene(3);
        }

        public void DifficultyEasy(Button button)
        {
            _level.SetDifficulty(LevelDifficulty.EASY);
            SetSelected(button);
        }
        public void DifficultyMedium(Button button)
        {
            _level.SetDifficulty(LevelDifficulty.MEDIUM);
            SetSelected(button);
        }
        public void DifficultyHard(Button button)
        {
            _level.SetDifficulty(LevelDifficulty.HARD);
            SetSelected(button);
        }

        private void SetSelected(Button button)
        {
            button.transform.parent.GetChild(1).gameObject.SetActive(true);
            if (_lastSelected != null && button != _lastSelected)
                _lastSelected.transform.parent.GetChild(1).gameObject.SetActive(false);
            _lastSelected = button;
        }
    }
}
