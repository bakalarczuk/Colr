using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Habtic.Games.Colr
{
    [RequireComponent(typeof(Fade))]
    public class FlashController : MonoBehaviour
    {
        [SerializeField]
        private Game _game;
        private Fade _fade;
        public TMP_Text levelUpText;
        public GameObject Popup;

        LTDescr _tween;

        private void Awake()
        {
            _fade = GetComponent<Fade>();
        }

        private void OnEnable()
        {
            GameManager.OnNextLevel += Flash;
        }

        private void OnDisable()
        {
            GameManager.OnNextLevel -= Flash;
        }

        private void Flash(NextLevelEventArgs args)
        {
            StartCoroutine(NextLevel());
        }

        IEnumerator NextLevel()
        {
            GameManager.Instance.LevelState = LevelStates.pause;
            levelUpText.text = $"{_game.LocalizedStrings["game_flash_text_level"]} {Level.Instance.CurrentLevel.ToString()}!";
            _fade.In(false, () =>
            {
                _fade.Out();
            });
            yield return new WaitForSeconds(0.5f);

            _tween = LeanTween.moveLocalX(Popup, 0 - (Popup.GetComponent<RectTransform>().rect.width / 2), 1).setEase(LeanTweenType.easeOutSine);
            yield return new WaitForSeconds(1.5f);
            _tween = LeanTween.moveLocalX(Popup, -1000, 1).setEase(LeanTweenType.easeInSine);
            _tween.setOnComplete(() =>
            {
                GameManager.Instance.LevelState = LevelStates.play;
                Popup.transform.localPosition = new Vector2(1000, 0);
            });
        }
    }
}
