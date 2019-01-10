using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Animator))]
    public class PauseMenu : MonoBehaviour
    {

        private LevelManager _levelManager;

        private Animator _animator;

        private GameManager _gameManager;

        private Image _image;
        private Level _level;

        [SerializeField]
        private TMP_Text _levelText;

        // Use this for initialization
        void Start()
        {
            _levelManager = LevelManager.Instance;
            _gameManager = GameManager.Instance;
            _image = GetComponent<Image>();
            _animator = GetComponent<Animator>();
            Hide();
        }

        void OnEnable()
        {

            _level = Level.Instance;
        }

        public void TogglePauseMenu()
        {
            if (_gameManager.Paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Exit()
        {
            Resume();
            _levelManager.LoadScene(0);
        }

        public void Pause()
        {
            _levelText.text = "LEVEL " + _level.CurrentLevel.ToString();

            _gameManager.LevelState = LevelStates.pause;
            //MuteSound(); Don't think this is necessary because setting TimeScale influences sound aswell.
            _animator.SetBool("Paused", true);
        }

        public void Resume()
        {
            _gameManager.LevelState = LevelStates.resume;
            //UnmuteSound(); Don't think this is necessary because setting TimeScale influences sound aswell.
            _animator.SetBool("Paused", false);
        }

        public void Replay()
        {
            _level.SetDifficulty(_level.Difficulty);
            Resume();
            _levelManager.LoadScene(1);
        }

        public void MuteSound()
        {
            AudioListener.pause = true;
        }

        public void UnmuteSound()
        {
            AudioListener.pause = false;
        }

        public void Hide()
        {
            _image.enabled = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(false);
            }
        }

        public void Show()
        {
            _image.enabled = true;

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.gameObject.SetActive(true);
            }
        }
    }
}
