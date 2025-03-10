﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Habtic.Managers;
using TMPro;

namespace Habtic.Games.Colr
{
    public class IntroLevelQuestion : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _questionText;


        void Awake()
        {
            IntroLevelManager.OnLevelStateChanged += LevelStateChanged;
        }

        void OnDestroy()
        {
            IntroLevelManager.OnLevelStateChanged -= LevelStateChanged;
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

        public void SetQuestionText(string questionText)
        {
            _questionText.text = questionText;
            Show();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

    }
}
