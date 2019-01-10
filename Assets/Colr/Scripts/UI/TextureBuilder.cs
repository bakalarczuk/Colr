using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureBuilder : MonoBehaviour
{

    public RawImage rawImage;

    [SerializeField]
    private int width = 200;

    [SerializeField]
    private int height = 50;

    [SerializeField]
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        Texture2D temp = new Texture2D(width, height);
        Texture2D tex = CalculateTexture(temp.width, temp.height, temp.height / 2, temp.height / 2, temp.width / 2, temp);


        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        _image.sprite = sprite;
    }
 
 
    Texture2D CalculateTexture(int w, int h, float r, float cx, float cy, Texture2D sourceTex)
    {
        Color[] c = sourceTex.GetPixels(0, 0, sourceTex.width, sourceTex.height);
        Texture2D b = new Texture2D(h, w);
        for (int i = 0; i < (h * w); i++)
        {
            int y = Mathf.FloorToInt(((float)i) / ((float)w));
            int x = Mathf.FloorToInt(((float)i - ((float)(y * w))));
            if (r * r >= (x - cx) * (x - cx) + (y - cy) * (y - cy))
            {
                b.SetPixel(x, y, c[i]);
            }
            else
            {
                b.SetPixel(x, y, Color.clear);
            }
        }
        b.Apply();
        return b;
    }
}
