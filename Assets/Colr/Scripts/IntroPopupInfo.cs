using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class IntroPopupInfo : Singleton<PopupInfo>
    {

        LTDescr _tween;
        bool showPopup = true;
        public GameObject popup;

        private void Start()
        {
            IntroLevelManager.OnGameSetup += OnLevelSetup;
            popup.SetActive(true);
        }

        private void OnDestroy()
        {
            IntroLevelManager.OnGameSetup -= OnLevelSetup;
        }

        public void valueChanged()
        {
            showPopup = !showPopup;
        }

        public void OnLevelSetup()
        {
                popup.SetActive(true);
                IntroLevelManager.Instance.Pause();
                _tween = LeanTween.moveLocalY(popup, 0, 0.5f).setEase(LeanTweenType.easeOutSine);
        }

        public void Continue()
        {
            IntroLevelManager.Instance.Resume();
            _tween = LeanTween.moveY(popup, transform.position.y - 2000, 1f).setEase(LeanTweenType.easeInSine);
            IntroLevelManager.Instance.LevelState = LevelStates.play;
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
