using UnityEngine;
using System.Collections;

public class BluetoGreen : MonoBehaviour
{
    public Sprite ;

    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseDown()
    {
        ColorChangered();
    }
    void ColorChangered()
    {
        SpriteRenderer.sprite = red;
    }
}