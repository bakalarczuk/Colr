using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{
    public class ScoreHUD : MonoBehaviour
    {
        private Stack<GameObject> PointText = new Stack<GameObject>();
        private List<GameObject> ActivePoints = new List<GameObject>();
        public GameObject _scoreText;
        private TMP_Text scoretext;
        private int _realScore;

        LTDescr _tween;
        LTDescr _tweenS;
        private int _tweenID;
        private int _tweenSID;

        void Awake()
        {
            scoretext = _scoreText.GetComponent<TMP_Text>();
            GameManager.OnGameSetup += ResetScore;
            GameManager.OnNextLevel += RemoveActive;
            GameManager.OnScoreChanged += UpdateScore;
        }

        void OnDestroy()
        {
            GameManager.OnScoreChanged -= UpdateScore;
            GameManager.OnNextLevel -= RemoveActive;
            GameManager.OnGameSetup -= ResetScore;
        }

        public void ResetScore()
        {
            scoretext.text = 0.ToString();
        }

        private void UpdateScore(int score, int realscore)
        {
            _realScore = realscore;
            scoretext.text = _realScore.ToString();

        }

        private void RemoveActive(NextLevelEventArgs args)
        {
            StopCoroutine("addscore");
            LeanTween.cancel(_tweenID);
            LeanTween.cancel(_tweenSID);
            for (int i = 0; i < ActivePoints.Count; i++)
            {
                ActivePoints[i].SetActive(false);
            }
            scoretext.text = _realScore.ToString();
        }
    }
}
