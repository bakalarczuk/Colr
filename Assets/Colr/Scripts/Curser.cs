using System.Collections;
using System.Collections.Generic;
using Habtic.Managers;
using UnityEngine;

public class Curser : MonoBehaviour
{

    public Camera cam;
    private bool touching = false;

    void OnEnable()
    {
        InputManager.Instance.OnTouchDown += OnTouchDown;
        InputManager.Instance.OnTouchUp += OnTouchUp;
    }

    void OnDisable()
    {
        InputManager.Instance.OnTouchDown -= OnTouchDown;
        InputManager.Instance.OnTouchUp -= OnTouchUp;
    }

    void Update()
    {
        if (touching)
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

    private void OnTouchDown(YTouchEventArgs touchEventArgs)
    {
		touching = true;
    }

    private void OnTouchUp(YTouchEventArgs touchEventArgs)
    {
		touching = false;
    }
}
