using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public class TextureScroller : MonoBehaviour
{
    #region VisableProperties
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private Image myImage;
    [SerializeField]
    private Direction direction;
    [SerializeField]
    private bool setMaterial = true;
	[SerializeField]
	private bool startOnAwake = true;
    #endregion

	private bool _scrollTexture;
    private Vector2 _myDir;

    void Start()
    {
        if (myImage == null)
            myImage = this.gameObject.GetComponent<Image>();
        if (setMaterial)
            SetMaterial();	
		if(startOnAwake)
			TSStart();
        CheckDirection();
    }

	public void TSStart(){
		_scrollTexture = true;
	}

	public void TSStop(){
		_scrollTexture = false;
	}

	void Update()
    {
		if(_scrollTexture){
        	myImage.materialForRendering.mainTextureOffset += new Vector2(_myDir.x * speed * Time.deltaTime, _myDir.y * speed * Time.deltaTime);
		}
    }

    private void CheckDirection()
    {
        switch (direction)
        {
            case Direction.Up:
                _myDir = new Vector2(0, -1);
                break;
            case Direction.Right:
                _myDir = new Vector2(-1, 0);
                break;
            case Direction.Down:
                _myDir = new Vector2(0, 1);
                break;
            case Direction.Left:
                _myDir = new Vector2(1, 0);
                break;
            default:
                _myDir = Vector2.zero;
                break;
        }
    }

    private void SetMaterial()
    {
        Material scrollMat = new Material(Shader.Find("Unlit/Transparent"));
        scrollMat.name = "ScrollMaterial";
        scrollMat.mainTexture = myImage.sprite.texture;
        myImage.material = scrollMat;
    }

    private enum Direction
    {
        None,
        Up,
        Right,
        Down,
        Left
    }
}
