using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Habtic.Managers;

namespace Habtic.Games.Colr
{
    public class WheelColor : MonoBehaviour
	{
        #region Variables and Properties

        public delegate void StartDirectionChanged(SwipeDirection direction);
        public static event StartDirectionChanged OnStartDirectionChanged;

		[SerializeField]
		private Image colorSprite;

		[SerializeField]
		private ColrColor color;

		private SwipeDirection _originDirection = SwipeDirection.Right;
        private SwipeDirection _startDirection = SwipeDirection.Right;

        public SwipeDirection StartDirection
        {
            get { return _startDirection; }
            set
            {
                _startDirection = value;
                OnStartDirectionChanged?.Invoke(value);
            }
        }



        #endregion
        #region Unity Methods

        void Awake()
        {
			colorSprite.alphaHitTestMinimumThreshold = 0.8f;
            //GameManager.OnGameSetup += SetFishOutside;
            //GameManager.OnInputChecked += SetFishOutside;
        }

        void OnDestroy()
        {
            //GameManager.OnGameSetup -= SetFishOutside;
            //GameManager.OnInputChecked -= SetFishOutside;
        }
		#endregion

		#region Methods


		public void SetColor(Color color, ColrColor colr)
        {
			colorSprite.color = color;
			this.color = colr;
        }

		public ColrColor GetColor {
			get { return color; }
		}

		#endregion


	}

}
