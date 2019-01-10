using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveScript : MonoBehaviour {

	private LTDescr _tween;
	private int _tweenID;
	private float YBasePos;

	private void OnEnable()
	{
		YBasePos = this.transform.position.y;
		_tween = LeanTween.moveY(this.gameObject, YBasePos + 0.15f, 5f).setLoopPingPong().setEase(LeanTweenType.easeInSine);
		_tweenID = _tween.id;
	}

	private void OnDisable() {
		LeanTween.cancel(_tweenID);
		this.transform.position = new Vector3(transform.position.x, YBasePos, transform.position.z);
	}

}
