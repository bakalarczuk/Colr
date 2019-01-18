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
        [SerializeField]
        private TMP_Text _questionText;


        void Awake()
        {
            GameManager.OnLevelStateChanged += LevelStateChanged;
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

        public void SetQuestionText(string key)
        {
            _questionText.text = _game.LocalizedStrings[key];
        }

    }
}
