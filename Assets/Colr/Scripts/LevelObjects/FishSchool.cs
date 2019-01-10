using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Habtic.Managers;

namespace Habtic.Games.Colr
{
    public class FishSchool : MonoBehaviour
    {
        #region Variables and Properties

        public delegate void FishesMovedOut();
        public static event FishesMovedOut OnFishesMovedOut;

        [SerializeField]
        private GameObject[] _fishPrefabs;

        private Sprite _defaultFishSprite;
        private Sprite _highLightedFishSprite;

        private SwipeDirection _movingDirection;

        public SwipeDirection MovingDirection
        {
            get { return _movingDirection; }
            private set
            {
                _movingDirection = value;
            }
        }

        [SerializeField]
        private Fish[] _fishes;

        private Vector2 _centreOffset = new Vector2(156, 156);

        private Level _level;


        private LTDescr _tweenM;

        #endregion
        #region Unity Methods

        void Awake()
        {
            _level = Level.Instance;
        }

        #endregion

        #region Methods

        public void Cleanupfishes()
        {

        }

        public void ComeIn()
        {
            if (_tweenM != null)
            {
                if (LeanTween.isTweening(_tweenM.id))
                {
                    LeanTween.cancel(_tweenM.id);
                }
            }

            transform.localPosition = Vector3.zero;
        }

        public void SnapOut()
        {

        }

        public bool IsMovin()
        {
            Fish f = GrabExistingFish();

            if (f == null) return false;

            return f.IsMoving();
        }

        private Fish GrabFreeFish()
        {
            for (int i = 0; i < _fishes.Length; i++)
            {
                Fish f = _fishes[i];
                if (f.FreeFish())
                {
                    return f;
                }
            }
            return null;
        }

        private Fish GrabExistingFish()
        {
            for (int i = 0; i < _fishes.Length; i++)
            {
                Fish f = _fishes[i];
                if (!f.FreeFish())
                {
                    return f;
                }
            }
            return null;
        }

        public Fish GrabCentralFish()
        {
            for (int i = 0; i < _fishes.Length; i++)
            {
                Fish f = _fishes[i];
                if (f.GetFishCoordinates() == Vector2.zero)
                {
                    return f;
                }
            }
            return null;
        }

        public bool AreAllFishesOffScreen()
        {
            bool reply = true;
            for (int i = 0; i < _fishes.Length; i++)
            {
                Fish f = _fishes[i];
                if (!f.IsFishVisible())
                {
                    reply = false;
                }
            }
            return reply;
        }

        private void MoveSchoolOut(float _movingTime = 5f)
        {
            if (_tweenM != null)
            {
                if (LeanTween.isTweening(_tweenM.id))
                {
                    LeanTween.cancel(_tweenM.id);
                }
            }

            bool leftOrRightDirection = HelperMethods.CoinFlip();
            if (leftOrRightDirection)
            {
                MovingDirection = SwipeDirection.Right;
            }
            else
            {
                MovingDirection = SwipeDirection.Left;
            }

            MoveSchoolOutTo(MovingDirection, _movingTime);
        }

        public void MoveSchoolOutTo(SwipeDirection _movingDirection, float _movingTime = 5f)
        {
            float _schoolMovingTime = _movingTime;
            float _moveTo = 0f;

            switch (_movingDirection)
            {
                case SwipeDirection.Left:
                    _moveTo = -Screen.width * 1f;
                    break;
                case SwipeDirection.Right:
                    _moveTo = Screen.width * 1f;
                    break;
                default:

                    break;
            }

            LeanTween.moveLocalX(gameObject, 0f, 1f)
            .setOnComplete(() =>
            {
                _tweenM = LeanTween.moveLocalX(gameObject, _moveTo, _schoolMovingTime)
                    .setEase(LeanTweenType.linear)
                    .setOnComplete(() =>
                    {
                        if (OnFishesMovedOut != null)
                        {
                            OnFishesMovedOut();
                        }
                    });
            });
        }

        public void StartNewLevel(int levelNR)
        {
            ComeIn();

            // Set movement
            if (_level.DirAndMove > 0)
            {
                MoveSchoolOut(_level.MovingOutTime);
            }

            // Set other fishes
            for (int i = 0; i < _fishes.Length; i++)
            {
                Fish f = _fishes[i];
                Vector2 _fishCoordinates = f.GetFishCoordinates();
                f.SetFishVisible(false);
                if (
                        (_fishCoordinates.x >= -(_level.HorFishes)
                        && _fishCoordinates.x <= (_level.HorFishes)
                        && _fishCoordinates.y == 0
                        && _fishCoordinates.x != 0)
                            ||
                        (_fishCoordinates.y >= -(_level.VerFishes)
                        && _fishCoordinates.y <= (_level.VerFishes)
                        && _fishCoordinates.x == 0
                        && _fishCoordinates.y != 0)
                    )
                {
                    if (_level.RandomFish == 1)
                    {
                        f.SetRandomSprite();
                    }
                    f.ResetFishDirection();
                    f.SetFishVisible(true);
                    f.ComeIn();
                }

            }

            // Set central Fish
            Fish fish = GrabCentralFish();
            if (_level.RandomCentralFish == 1)
            {
                fish.SetRandomSprite();
            }
            fish.ResetFishDirection();
            fish.SetFishVisible(true);
            fish.ComeIn();

        }

        public void StartTutorialOne(int levelNR)
        {
            ComeIn();

            // Set other fishes
            for (int i = 0; i < _fishes.Length; i++)
            {
                Fish f = _fishes[i];
                f.SetFishVisible(false);
                Vector2 _fishCoordinates = f.GetFishCoordinates();
                if (
                        (_fishCoordinates.x >= -(_level.HorFishes)
                        && _fishCoordinates.x <= (_level.HorFishes)
                        && _fishCoordinates.y == 0
                        && _fishCoordinates.x != 0)
                            ||
                        (_fishCoordinates.y >= -(_level.VerFishes)
                        && _fishCoordinates.y <= (_level.VerFishes)
                        && _fishCoordinates.x == 0
                        && _fishCoordinates.y != 0)
                    )
                {
                    f.ResetFishDirection();
                    f.SetFishVisible(true);
                    f.ComeIn();
                }
            }

            // Set central Fish
            Fish fish = GrabCentralFish();
            fish.SetFishDirection(SwipeDirection.Up);
            fish.SetFishVisible(true);
            fish.ComeIn();
        }

        public void StartTutorialTwo(int levelNR)
        {
            ComeIn();

            // Set movement
            MovingDirection = SwipeDirection.Right;
            MoveSchoolOutTo(MovingDirection, 10f);

            // Set other fishes
            for (int i = 0; i < _fishes.Length; i++)
            {
                Fish f = _fishes[i];
                Vector2 _fishCoordinates = f.GetFishCoordinates();
                if (
                        (_fishCoordinates.x >= -(_level.HorFishes)
                        && _fishCoordinates.x <= (_level.HorFishes)
                        && _fishCoordinates.y == 0
                        && _fishCoordinates.x != 0)
                            ||
                        (_fishCoordinates.y >= -(_level.VerFishes)
                        && _fishCoordinates.y <= (_level.VerFishes)
                        && _fishCoordinates.x == 0
                        && _fishCoordinates.y != 0)
                    )
                {
                    f.ResetFishDirection();
                    f.SetFishVisible(true);
                    f.ComeIn();
                }
            }

            // Set central Fish
            Fish fish = GrabCentralFish();
            fish.SetFishDirection(SwipeDirection.Down);
            fish.SetFishVisible(true);
            fish.ComeIn();
        }

        #endregion
    }
}
