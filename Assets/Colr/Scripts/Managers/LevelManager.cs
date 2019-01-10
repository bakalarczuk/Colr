using System.Collections;
using UnityEngine;

namespace Habtic.Games.Colr
{
    public class LevelManager : Singleton<LevelManager>
    {

        public delegate void LevelLoadProgressChanged(float progress);
        public static event LevelLoadProgressChanged OnLevelLoadProgressChanged;

        [System.Serializable]
        public class Scene
        {
            public GameObject gameObject;
            public bool fadeIn = true;
            public bool fadeOut = true;
            public bool showLoadScreen = false;
            public float minLoadTime = 3f;
        }

        [SerializeField]
        private Fade _fade;

        public Fade GetFade { get{return _fade;} }

        [SerializeField]
        private Scene[] _scenes;

        private Scene _activeScene;

        private Scene _sceneToLoad;


        // Use this for initialization
        void Start()
        {

            // Deactivate every scene
            for (int i = 0; i < _scenes.Length; i++)
            {
                _scenes[i].gameObject.SetActive(false);
            }

            SetActiveScene(_scenes[0]);
        }

        private void SetActiveScene(Scene scene)
        {
            if (_activeScene != null)
            {
                _activeScene.gameObject.SetActive(false);
            }

            scene.gameObject.SetActive(true);
            _activeScene = scene;

            if (scene.fadeIn)
            {
                _fade.In();
            }
        }

        public void LoadScene(int index)
        {
            _sceneToLoad = _scenes[index];

            if (_sceneToLoad.fadeOut)
            {
                _fade.Out(OnFadeOutComplete);
            }
            else if (_sceneToLoad.showLoadScreen)
            {
                StartCoroutine(LoadSceneAsync());
            }
            else
            {
                SetActiveScene(_sceneToLoad);
            }
        }

        private IEnumerator LoadSceneAsync()
        {
            float progress = 0f;

            // mininum time it needs to take
            float duration = _sceneToLoad.minLoadTime;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                // Show progress
                elapsed += Time.deltaTime;
                progress = Mathf.Clamp01(elapsed / duration);

                // Notify listeners with current progress
                if (OnLevelLoadProgressChanged != null)
                    OnLevelLoadProgressChanged(progress);

                yield return null;
            }

            SetActiveScene(_sceneToLoad);
        }

        public void OnFadeOutComplete()
        {
            if (_sceneToLoad.showLoadScreen)
            {
                StartCoroutine(LoadSceneAsync());
            }
            else
            {
                SetActiveScene(_sceneToLoad);
            }
        }
    }
}
