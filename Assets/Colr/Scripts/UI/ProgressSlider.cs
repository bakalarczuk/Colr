using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Habtic.Games.Colr
{
    public class ProgressSlider : MonoBehaviour
    {

        [SerializeField]
        private Slider _slider;

        [SerializeField]
        private TMP_Text _progressText;

        void Start()
        {
            if (!_slider)
                Debug.LogWarning("Reference to Slider is missing.");
        }

        void OnEnable()
        {
            LevelManager.OnLevelLoadProgressChanged += OnProgressChanged;
        }

        void OnDisable()
        {
            LevelManager.OnLevelLoadProgressChanged -= OnProgressChanged;
        }

        private void OnProgressChanged(float progress)
        {
            if (_slider.gameObject.activeSelf == false)
            {
                _slider.gameObject.SetActive(true);
            }

            _slider.value = progress;
            _progressText.text = Mathf.RoundToInt((progress * 100)) + "%";

            if (progress >= 1)
            {
                _slider.gameObject.SetActive(false);
            }
        }
    }
}
