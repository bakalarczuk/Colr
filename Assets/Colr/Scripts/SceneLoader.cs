using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Habtic.Games.Colr
{
    public class SceneLoader : MonoBehaviour
    {

        public static event EventHandler<ProgressChangedEventArgs> ProgressChangedEvent;

        [SerializeField]
        private bool fadeOut = true;

        [SerializeField]
        private float minDuration = 2f;

        private Animator _animator;
        private int _sceneToLoad;
        private bool _doAsync;

        private const float LOAD_READY_PERCENTAGE = 0.9f;

        void Start()
        {
            if (fadeOut)
            {
                _animator = GetComponent<Animator>();

                if (!_animator)
                    Debug.LogError("Fading out requires an Animator.");
            }
        }

        public void LoadScene(int sceneIndex)
        {
            LoadScene(sceneIndex, false);
        }

        public void LoadSceneAsync(int sceneIndex)
        {
            LoadScene(sceneIndex, true);
        }

        private void LoadScene(int sceneIndex, bool async)
        {
            _doAsync = async;
            _sceneToLoad = sceneIndex;

            if (fadeOut)
            {
                _animator.SetTrigger("FadeOut");
            }

            else if (_doAsync)
            {
                StartCoroutine(LoadSceneAsync());
            }

            else
            {
                SceneManager.LoadScene(_sceneToLoad);
            }
        }

        public void LoadNextScene(bool async)
        {
            _doAsync = async;
            int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            LoadScene(sceneIndex, _doAsync);
        }

        private IEnumerator LoadSceneAsync()
        {
            AsyncOperation loadingSceneAO = SceneManager.LoadSceneAsync(_sceneToLoad);
            loadingSceneAO.allowSceneActivation = false;

            float progress = 0f;

            // mininum time it needs to take
            float minEndTime = Time.time + minDuration;

            while (!loadingSceneAO.isDone)
            {
                float minTimeLeft = minEndTime - Time.time;
                float mockNegativeProgress = Mathf.Clamp01(minTimeLeft / minDuration);

                // Show progress
                progress = loadingSceneAO.progress;

                if (progress + 0.1f - mockNegativeProgress >= 0)
                {
                    progress -= mockNegativeProgress;
                }

                progress = Mathf.Clamp01(progress / LOAD_READY_PERCENTAGE);

                if (progress == 1)
                {
                    loadingSceneAO.allowSceneActivation = true;
                }

                // Notify listeners with current progress
                OnProgressChanged(new ProgressChangedEventArgs { Progress = progress });

                yield return null;
            }
        }

        protected virtual void OnProgressChanged(ProgressChangedEventArgs e)
        {
            EventHandler<ProgressChangedEventArgs> handler = ProgressChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Event gets triggered at the end of Fade_Out animation
        public void OnFadeComplete()
        {
            if (_doAsync)
            {
                StartCoroutine(LoadSceneAsync());
            }
            else
            {
                SceneManager.LoadScene(_sceneToLoad);
            }
        }
    }

    public class ProgressChangedEventArgs : EventArgs
    {
        public float Progress { get; set; }
    }
}
