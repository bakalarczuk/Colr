using UnityEngine;

namespace Habtic.Games.Colr
{
    public abstract class MovingObject : Entity
    {

        [SerializeField]
        protected float speed = 0.5f;

        #region PROPERTIES
        public bool EnteredViewport { get { return _enteredViewPort; } set { _enteredViewPort = value; } }
        #endregion PROPERTIES

        protected ScreenBounds _screen;
        protected bool _enteredViewPort = false;
        protected bool _paused;

        void Awake()
        {
            _paused = false;
            _screen = ScreenBounds.Instance;
            GameManager.OnPauseStateChanged += setPaused;
        }

        private void setPaused(bool value){
            _paused = value;
        }

        protected abstract void Move();
        protected abstract bool IsInViewPort();
    }
}
