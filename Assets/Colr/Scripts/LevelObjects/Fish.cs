using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Habtic.Managers;

namespace Habtic.Games.Colr
{
    public class Fish : MonoBehaviour
    {
        #region Variables and Properties

        public delegate void StartDirectionChanged(SwipeDirection direction);
        public static event StartDirectionChanged OnStartDirectionChanged;

        [SerializeField]
        private GameObject[] fishprefabs;
        private bool _isFishRecyclable;
        [SerializeField]
        private Image _fishSprite;
        [SerializeField]
        private int fishX;
        [SerializeField]
        private int fishY;
        private bool _currentlyMoving;
        private SwipeDirection _originDirection = SwipeDirection.Right;
        private SwipeDirection _startDirection = SwipeDirection.Right;
        private Vector3 spawnPoint;

        public SwipeDirection StartDirection
        {
            get { return _startDirection; }
            set
            {
                _startDirection = value;
                OnStartDirectionChanged?.Invoke(value);
            }
        }

        private LTDescr _tweenB;
        private LTDescr _tweenC;

        private int frame = 0;
        private float deltaTime = 0;

        [SerializeField]
        private AnimationScript _currentAnimationSprites;


        #endregion
        #region Unity Methods

        void Awake()
        {
            GameManager.OnGameSetup += SetFishOutside;
            GameManager.OnInputChecked += SetFishOutside;
        }

        private void Update()
        {
            AnimateFish();
        }

        void OnDestroy()
        {
            GameManager.OnGameSetup -= SetFishOutside;
            GameManager.OnInputChecked -= SetFishOutside;
        }

        #endregion

        #region Methods

        void AnimateFish()
        {
            //Keep track of the time that has passed
            deltaTime += Time.deltaTime;
            if (deltaTime > (1f / _currentAnimationSprites.framesPerSecond))
            {
                float restDeltaTime = 0;
                restDeltaTime %= (1f / _currentAnimationSprites.framesPerSecond);

                int newFrame = frame + Mathf.FloorToInt(deltaTime * _currentAnimationSprites.framesPerSecond);
                newFrame %= _currentAnimationSprites.sprites.Length;
                frame = newFrame;
                deltaTime = restDeltaTime;
            }

            //Animate sprite with selected frame
            _fishSprite.sprite = _currentAnimationSprites.sprites[frame];
        }

        public void SetFishOutside()
        {
            StopBobbing();
            spawnPoint = HelperMethods.GetSpawnPointLeftOrRight();
            _fishSprite.transform.position = spawnPoint;
        }

        public void SetRandomSprite()
        {
            _currentAnimationSprites = fishprefabs[Random.Range(0, (fishprefabs.Length - 1))].GetComponent<AnimationScript>();
            frame = 0;
        }

        public void ComeIn()
        {
            TweenFish();
        }

        private void TweenFish()
        {
            Vector3 saveRot = _fishSprite.transform.eulerAngles;
            Vector3 saveScale = _fishSprite.transform.localScale;
            if (_fishSprite.transform.position.x > 0)
            {
                _fishSprite.transform.localScale = new Vector3(1, -1, 1);
            }
            else
            {
                _fishSprite.transform.localScale = new Vector3(1, 1, 1);
            }
            Vector3 mypos = transform.position;
            mypos.z = _fishSprite.transform.position.z;
            _fishSprite.transform.right = (mypos - _fishSprite.transform.position);
            _tweenC = LeanTween.moveLocal(_fishSprite.gameObject, Vector3.zero, 0.5f)
            .setEase(LeanTweenType.easeInOutSine)
            .setOnComplete(() =>
            {
                _fishSprite.transform.localScale = saveScale;
                _tweenC = LeanTween.rotateZ(_fishSprite.gameObject, saveRot.z, 0.3f);
                _tweenC.setOnComplete(() =>
                {
                    StartBobbing();
                });
            });
        }

        private void StartBobbing()
        {
            _tweenB = LeanTween.moveLocalY(_fishSprite.gameObject, 40, UnityEngine.Random.Range(1.5f, 3f))
            .setEase(LeanTweenType.easeInOutSine)
            .setLoopPingPong();
        }

        private void StopBobbing()
        {
            if (_tweenB == null) return;
            if (LeanTween.isTweening(_tweenB.id))
            {
                LeanTween.cancel(_tweenB.id);
                LeanTween.moveLocalY(_fishSprite.gameObject, 0, 0.5f);
            }
        }

        public bool FreeFish()
        {
            return _isFishRecyclable;
        }

        public bool IsMoving()
        {
            return _currentlyMoving;
        }

        private void SetSpriteDirection(GameObject sprite, SwipeDirection dir)
        {
            float angle = 0;
            Vector3 scale = Vector3.one;
            switch (dir)
            {
                case SwipeDirection.Down:
                    angle = -90;
                    scale = Vector3.one;
                    break;
                case SwipeDirection.Left:
                    angle = 0;
                    scale = new Vector3(-1, 1, 1);
                    break;
                case SwipeDirection.Right:
                    angle = 0;
                    scale = Vector3.one;
                    break;
                case SwipeDirection.Up:
                    angle = 90;
                    scale = Vector3.one;
                    break;
                default:
                    break;
            }

            sprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            sprite.transform.localScale = scale;
        }

        public void ResetFishDirection()
        {
            SwipeDirection sd = GetRandomSwipeDirection();
            StartDirection = sd;
            ChangeSpriteDirection(sd);
        }

        public void SetFishDirection(SwipeDirection sd)
        {
            StartDirection = sd;
            ChangeSpriteDirection(sd);
        }

        public bool IsFishVisible()
        {
            if (_fishSprite == null) return false;

            bool reply = true;

            if (_fishSprite.transform.position.x > Screen.width)
            {
                reply = false;
            }
            if (_fishSprite.transform.position.x < 0)
            {
                reply = false;
            }

            return reply;
        }

        public void SetFishVisible(bool visible)
        {
            _fishSprite.enabled = visible;
        }

        public Vector2 GetFishCoordinates()
        {
            return new Vector2(fishX, fishY);
        }

        private SwipeDirection GetRandomSwipeDirection()
        {
            System.Array values = System.Enum.GetValues(typeof(SwipeDirection));
            SwipeDirection sd = (SwipeDirection)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            return sd;
        }

        private void ChangeSpriteDirection(SwipeDirection swipeDirection)
        {
            SetSpriteDirection(_fishSprite.gameObject, swipeDirection);
        }

        #endregion


    }

}
