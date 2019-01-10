using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Habtic.Games.Colr
{
    public class LifesHUD : MonoBehaviour
    {
        private Level _level;

        public GameObject _lifeText;

        public Transform parent;
        // private Stack<GameObject> livesText = new Stack<GameObject>();
        // private List<GameObject> ActiveLives = new List<GameObject>();
        // public GameObject prefab;

        LTDescr _tween;
        LTDescr _tweenS;

        // Use this for initialization
        void Awake()
        {
            _level = Level.Instance;
        }

        void OnEnable()
        {
            Initialize();
            GameManager.OnLifesChanged += UpdateLifes;
            // GameManager.OnNextLevel += RemoveActive;
        }

        void OnDisable()
        {
            //Clear();
            GameManager.OnLifesChanged -= UpdateLifes;
            // GameManager.OnNextLevel -= RemoveActive;
        }

        void Initialize()
        {
            _lifeText.GetComponent<TMP_Text>().text = _level != null ? _level.TotalLifes.ToString() : "0";
        }

        private void UpdateLifes(int lifes)
        {
            // Vector3 pos = new Vector3(Screen.width/2, Screen.height/2);
            // GameObject currlifes;
            // if (livesText.Count == 0)
            // {
            //     currlifes = Instantiate(prefab, parent);
            // }
            // else
            // {
            //     currlifes = livesText.Pop();
            // }
            // ActiveLives.Add(currlifes);
            // currlifes.GetComponent<TextMesh>().color = Color.red;
            // currlifes.GetComponent<TextMesh>().text = "-" + 1.ToString();
            // currlifes.SetActive(true);
            // currlifes.transform.position = pos;
            // Vector3 dir = new Vector3(-1.3f, 4.80f, 1.085995f) - pos;
            // dir = dir.normalized;
            // Vector2 newpos = currlifes.transform.position + dir;
            // _tween = LeanTween.move(currlifes, newpos, 4).setEase(LeanTweenType.easeInSine);
            // _tweenS = LeanTween.scale(_lifeText, new Vector2(1.2f, 1.2f), 0.15f);
            // _tweenS.setOnComplete(() =>
            // {
                _tweenS = LeanTween.scale(_lifeText, new Vector2(1, 1), 0.15f);
                _lifeText.GetComponent<TMP_Text>().text = lifes.ToString();
            // });
            // _tween.setOnComplete(() =>
            // {
            //     currlifes.gameObject.SetActive(false);
            //     ActiveLives.Remove(currlifes);
            //     livesText.Push(currlifes);
            // });

        }

        // private void RemoveActive(NextLevelEventArgs args){
        //     for (int i = 0; i < ActiveLives.Count; i++)
        //     {
        //         ActiveLives[i].SetActive(false);
        //     }
        // }
    }
}
