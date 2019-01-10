using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Habtic.Managers;
using System;

namespace Habtic.Managers
{
    public class YTouchEventArgs
    {
        public Vector2 TouchStart;
        public Vector2 TouchEnd;
        public Vector2 SwipeDirection;

        public YTouchEventArgs(Vector2 touchStart, Vector2 touchEnd)
        {
            TouchStart = touchStart;
            TouchEnd = touchEnd;
            SwipeDirection = Vector2.zero;
        }
        public YTouchEventArgs(Vector2 touchStart, Vector2 touchEnd, Vector2 swipedirection)
        {
            TouchStart = touchStart;
            TouchEnd = touchEnd;
            SwipeDirection = swipedirection;
        }
    }

    public struct InputSwipeEvent
    {
        public SwipeDirection swipeDirection;

        public InputSwipeEvent(Vector2 directionVector)
        {
            swipeDirection = SwipeDirection.Up;

            if (directionVector == Vector2.left) swipeDirection = SwipeDirection.Left;
            else if (directionVector == Vector2.left) swipeDirection = SwipeDirection.Left;
            else if (directionVector == Vector2.right) swipeDirection = SwipeDirection.Right;
            else if (directionVector == Vector2.down) swipeDirection = SwipeDirection.Down;
        }
    }

    public enum SwipeDirection { Up, Down, Left, Right }

    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    public class InputManager : MonoBehaviour
    {
        #region Variables
        public static InputManager Instance { get; private set; }
        [SerializeField]
        private float _minimumDragTrigger = 1f;
        private InputStates _inputState = InputStates.None;
        private Vector2 _startTouch, _lastTouch, _moveDelta, _pos;
        private float _touchDuration, _minSwipeDistanceWidth;
        private readonly List<Vector2> _swipeDirections = new List<Vector2>() { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        private float _maxSwipeDuration = 1f;

        private float _swipeSpeed;
        private bool _isActive;

        public delegate void Touch(YTouchEventArgs touchEventArgs);
        public event Touch OnTouchDown;
        public event Touch OnTouchUp;
        public event Touch OnSwiped;
        public event Touch OnDragged;

        // public GameObject CursorDot;
        // public Canvas parentCanvas;

        private Canvas _canvas;
        private CanvasScaler _scaler;
        private float _scalerX;
        private float _scalerY;

        #endregion
        #region Unity Methods

        private void Awake()
        {
            if (Instance)
            {
                string className = GetType().Name;
                Debug.LogError($"ERROR: There already is an instance of singleton [{className}]");
                Destroy(this);
            }
            else
            {
                Instance = this;
            }

            _minSwipeDistanceWidth = Screen.safeArea.width / 5;
            _scaler = GetComponentInParent<CanvasScaler>();
            _canvas = GetComponentInParent<Canvas>();
            _scalerX = (_scaler.referenceResolution.x / Screen.width);
            _scalerY = (_scaler.referenceResolution.y / Screen.height);
            _isActive = true;
        }

        private void Start()
        {
            // gameObject.AddComponent<QuitBehavior>();
            _pos = Vector2.zero;

            // UniRx.MessageBroker.Default.Receive<ApplicationStateChanged>()
            //     .Subscribe(stateChange => OnApplicationStateChanged(stateChange));
        }

        private void Update()
        {
            bool _touchPresent = false;

            if (Application.isEditor)
            {
                _pos = Input.mousePosition;
                _touchPresent = Input.GetMouseButton(0);
            }
            else
            {
                if (Input.touches != null)
                {
                    _touchPresent = Input.touches.Length > 0;
                    if (_touchPresent) _pos = Input.touches[0].position;
                }

            }

            switch (_inputState)
            {
                case InputStates.None:
                    //Do nothing. Waiting for new touch down
                    if (_touchPresent)
                    {
                        _startTouch = _pos;
                        _lastTouch = _startTouch;
                        // ShowDebug("startTouch: " + _startTouch);
                        SetInputState(InputStates.TouchDown);
                        OnTouchDown?.Invoke(new YTouchEventArgs(_startTouch, _startTouch));
                    }
                    break;
                case InputStates.TouchDown:

                    if (!_touchPresent)
                    {
                        SetInputState(InputStates.TouchUp);
                        break;
                    }

                    if (Vector2.Distance(_pos, _lastTouch) > _minimumDragTrigger)
                    {
                        SetInputState(InputStates.Drag);
                    }
                    break;

                case InputStates.TouchUp:
                    CheckSwipe(); // always check for swipe, even while dragging. The subscriber should handle the difference
                    TouchReset();
                    OnTouchUp?.Invoke(new YTouchEventArgs(_startTouch, _lastTouch));
                    break;

                case InputStates.Drag:
                    _touchDuration += Time.deltaTime;
                    Vector2 _dragFrameStart = _lastTouch;
                    _lastTouch = _pos;

                    OnDragged?.Invoke(new YTouchEventArgs(_dragFrameStart, _lastTouch));

                    if (!_touchPresent) SetInputState(InputStates.TouchUp);

                    break;
            }
        }

        private void CheckSwipe()
        {
            // Calculate distance in pixels
            float distance = Vector2.Distance(_startTouch, _lastTouch);
            distance /= _scaler.dynamicPixelsPerUnit;

            // It's considered a swipe if the minimum distance is achieved withtin the maximum duration
            if (_touchDuration < _maxSwipeDuration && distance >= _minSwipeDistanceWidth)
            {
                // start with a max negative dot, so any dot calculation will exceed this
                float maxDot = -Mathf.Infinity;
                Vector2 direction = Vector2.zero;

                foreach (Vector2 dir in _swipeDirections)
                {
                    // calculate dot for each swipedirection allowed
                    // closest direction is saved in direction with the corresponding dot in maxdot
                    float dot = Vector2.Dot(_lastTouch - _startTouch, dir);
                    if (dot > maxDot)
                    {
                        direction = dir;
                        maxDot = dot;
                    }
                }
                if (direction != Vector2.zero)
                {
                    // Only invoke event if direction is not Vector2.zero
                    ShowDebug($"SWIPE DURATION: {_touchDuration}s, DISTANCE: {distance}, DIRECTION: {direction}");
                    OnSwiped?.Invoke(new YTouchEventArgs(_startTouch, _lastTouch, direction));

                    // UniRx.MessageBroker.Default.Publish(new InputSwipeEvent(direction));
                }
            }
        }

        #endregion
        #region Getters and Setters

        public void SetInputState(InputStates newInputState)
        {
            // Debug.Log("Debugging TOUCH state: " + newInputState + " set at " + Time.time);
            _inputState = newInputState;
        }

        public InputStates GetInputState()
        {
            return _inputState;
        }

        #endregion
        #region Methods

        public bool RectTransformContains(RectTransform rectTransform, Vector2 uiPosition)
        {
            if (!_isActive) return false;
            Vector2 uiPos = uiPosition;
            RectTransform rectT = rectTransform;
            bool contains = false;
            Vector2 pivot = rectT.pivot;
            float elWidth = rectT.rect.width / _scalerX;
            float elHeight = rectT.rect.height / _scalerY;
            Vector3 rectPos = _canvas.worldCamera.WorldToScreenPoint(rectT.position);

            float xMin = rectPos.x - (pivot.x * elWidth);
            float xMax = (rectPos.x + elWidth) - (pivot.x * elWidth);
            float yMin = rectPos.y - (pivot.y * elHeight);
            float yMax = (rectPos.y + elHeight) - (pivot.y * elHeight);

            if (uiPos.x > xMin && uiPos.x < xMax && uiPos.y > yMin && uiPos.y < yMax)
            {
                contains = true;
            }
            return contains;
        }

        #endregion
        #region Console info

        protected void ShowDebug(object debugObject)
        {
            if (Debug.isDebugBuild) Debug.Log(this.GetType().Name + ", Message: " + debugObject);
        }

        // private void OnApplicationStateChanged(ApplicationStateChanged stateChange)
        // {
        //     switch (stateChange.newState)
        //     {
        //         case ApplicationState.Boot2:
        //             _minSwipeDistanceWidth = Screen.safeArea.width / 3;
        //             _scaler = GetComponentInParent<CanvasScaler>();
        //             _canvas = GetComponentInParent<Canvas>();
        //             _scalerX = (_scaler.referenceResolution.x / Screen.width);
        //             _scalerY = (_scaler.referenceResolution.y / Screen.height);
        //             _isActive = true;
        //             break;
        //     }
        // }

        private void TouchReset()
        {
            _touchDuration = 0f;
            _moveDelta = Vector2.zero;
            SetInputState(InputStates.None);
        }

        #endregion

        // private void SetDot(Vector2 dotPos)
        // {
        //     if (Debug.isDebugBuild && _isActive)
        //     {
        //         RectTransform cursorDotRectT = CursorDot.GetComponent<RectTransform>();
        //         cursorDotRectT.anchoredPosition = new Vector2((dotPos.x - (Screen.safeArea.width / 2)) * (_scaler.referenceResolution.x / Screen.safeArea.width), (dotPos.y - (Screen.safeArea.height / 2)) * (_scaler.referenceResolution.y / Screen.safeArea.height));
        //         LeanTween.alpha(cursorDotRectT, 1, 0.15f).setOnComplete(() => { LeanTween.alpha(cursorDotRectT, 0, 0.01f); });

        //     }
        // }
    }

    public enum InputStates
    {
        None,
        TouchDown,
        TouchUp,
        Drag,
        Swipe
    }
}
