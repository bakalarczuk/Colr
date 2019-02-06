using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Habtic.Games.Colr
{
	public class CoroutineHandler :	 Singleton<CoroutineHandler>
	{	  
		public IEnumerator HideIndicator(GameObject indicator)
		{
			yield return new WaitForSeconds(1f);
			LeanTween.scale(indicator, Vector3.zero, 0.5f)
				.setEase(LeanTweenType.easeInOutSine)
				.setOnComplete(() => {
					GameManager.Instance.LevelStart();
				});

		}		   			
	}
}
