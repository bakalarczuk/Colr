using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habtic.Managers;
using TMPro;

namespace Habtic.Games.Colr
{
    public class LevelQuestion : MonoBehaviour
    {
        [SerializeField]
        Game _game;
        private string _qDirection;
        private string _qMove;
        [SerializeField]
        private TMP_Text _questionText;


        void Awake()
        {
            GameManager.OnLevelStateChanged += LevelStateChanged;
            _qDirection = _game.LocalizedStrings["game_level_text_questionfacing"];
            _qMove = _game.LocalizedStrings["game_level_text_questionmoving"];
        }

        void OnDestroy()
        {
            GameManager.OnLevelStateChanged -= LevelStateChanged;
        }

        private void LevelStateChanged(LevelStates levelState)
        {
            switch (levelState)
            {
                case LevelStates.play:
                case LevelStates.pause:
                case LevelStates.resume:
                    this.gameObject.SetActive(true);
                    break;
                default:
                    this.gameObject.SetActive(false);
                    break;
            }
        }

        public void SetQuestionText(bool _dirOrMove)
        {
            if (_dirOrMove)
            {
                _questionText.text = _qMove;
            }
            else
            {
                {
                    _questionText.text = _qDirection;
                }
            }
        }

    }
}
