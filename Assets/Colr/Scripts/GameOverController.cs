using System.Collections;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class GameOverController : Singleton<GameOverController>
    {

        private LevelManager _levelManager;
        private LTDescr _tween;
        public GameObject gameOver;

        void Start()
        {
            _levelManager = LevelManager.Instance;
        }

        private void OnEnable()
        {
            gameOver.SetActive(false);
        }

        public void LoadScoreMenu()
        {
            StartCoroutine(LoadScoreMenuWithDelay());
        }

        IEnumerator LoadScoreMenuWithDelay()
        {
            GameManager.Instance.Pause();
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
