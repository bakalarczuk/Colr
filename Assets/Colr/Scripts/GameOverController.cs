using System.Collections;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class GameOverController : Singleton<GameOverController>
    {

        private LevelManager _levelManager;
        private LTDescr _tween;
        public GameObject gameOver;
		public TMPro.TMP_Text gameOverText;

        void Start()
        {
            _levelManager = LevelManager.Instance;
        }

        private void OnEnable()
        {
            gameOver.SetActive(false);
        }

        public void LoadScoreMenu(bool lifesOver = true)
        {
            StartCoroutine(LoadScoreMenuWithDelay(lifesOver));
        }

        IEnumerator LoadScoreMenuWithDelay(bool lifesOver)
        {
            GameManager.Instance.Pause();
			gameOverText.text = lifesOver ? GameManager.Instance.game.LocalizedStrings["game_gameover_text"] : GameManager.Instance.game.LocalizedStrings["game_challenge_over_text"];

			gameOver.SetActive(true);
            _tween = LeanTween.scale(gameOver, Vector3.one, 0.7f);
            yield return new WaitForSeconds(3f);
            _levelManager.LoadScene(2);
            yield return new WaitForSeconds(1f);
            gameOver.SetActive(false);
            gameOver.transform.localScale = Vector3.zero;
            GameManager.Instance.Resume();
        }
    }
}
