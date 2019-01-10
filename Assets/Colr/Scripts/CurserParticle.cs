using System.Collections;
using System.Collections.Generic;
using Habtic.Managers;
using UnityEngine;

public class CurserParticle : MonoBehaviour
{

    public Camera cam;
    private bool dragging = false;

    private ParticleSystem myParticle;

    void OnEnable()
    {
        myParticle = this.gameObject.GetComponent<ParticleSystem>();
        InputManager.Instance.OnTouchDown += ParticlesOnTouchDown;
        InputManager.Instance.OnTouchUp += ParticlesOnTouchUp;
        myParticle.Stop();
    }

    void OnDisable()
    {
        InputManager.Instance.OnTouchDown -= ParticlesOnTouchDown;
        InputManager.Instance.OnTouchUp -= ParticlesOnTouchUp;
    }

    void Update()
    {
        if (dragging)
        {
            if (Input.touchCount > 0)
            {
                Vector2 MousePos = cam.ScreenToWorldPoint(Input.GetTouch(0).position);
                this.transform.position = new Vector3(MousePos.x, MousePos.y, this.transform.position.z);
            }

            if (Application.isEditor)
            {
                Vector2 MousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                this.transform.position = new Vector3(MousePos.x, MousePos.y, this.transform.position.z);
            }
        }
    }

    private void ParticlesOnTouchDown(YTouchEventArgs touchEventArgs)
    {
        dragging = true;
		myParticle.Play();
    }

    private void ParticlesOnTouchUp(YTouchEventArgs touchEventArgs)
    {
        myParticle.Stop();
        dragging = false;
    }
}
