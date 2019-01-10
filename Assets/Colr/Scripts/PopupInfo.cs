using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class PopupInfo : Singleton<PopupInfo>
    {

        LTDescr _tween;
        bool showPopup = true;
        public GameObject popup;

        private void Start()
        {
            showPopup = DataHolder.ShowLevelMessage;
            GameManager.OnGameSetup += OnLevelSetup;
        }

        private void OnDestroy()
        {
            GameManager.OnGameSetup -= OnLevelSetup;
        }

        public void valueChanged()
        {
            showPopup = !showPopup;
            DataHolder.ShowLevelMessage = showPopup;
        }

        public void OnLevelSetup()
        {
            showPopup = DataHolder.ShowLevelMessage;
            if (showPopup)
            {
                popup.SetActive(true);
                GameManager.Instance.disableMenu(true);
                GameManager.Instance.Pause();
                _tween = LeanTween.moveLocalY(popup, 0, 0.5f).setEase(LeanTweenType.easeOutSine);
            }
            else
            {
                popup.SetActive(false);
                GameManager.Instance.Resume();
                GameManager.Instance.disableMenu(false);
                GameManager.Instance.LevelState = LevelStates.play;
            }
        }

        public void Continue()
        {
            GameManager.Instance.Resume();
            _tween = LeanTween.moveY(popup, transform.position.y - 2000, 1f).setEase(LeanTweenType.easeInSine);
            GameManager.Instance.disableMenu(false);
            GameManager.Instance.LevelState = LevelStates.play;
            _tween.setOnComplete(() =>
            {
                popup.SetActive(false);
            });
        }

        public void ContinueIntroLevel()
        {
            IntroLevelManager.Instance.Resume();
            _tween = LeanTween.moveY(popup, transform.position.y - 2000, 1f).setEase(LeanTweenType.easeInSine);
            // IntroLevelManager.Instance.disableMenu(false);
            IntroLevelManager.Instance.LevelState = LevelStates.play;
            _tween.setOnComplete(() =>
            {
                popup.SetActive(false);
            });
        }
    }
}
