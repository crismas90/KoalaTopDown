using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBox : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        SetColor();
    }

   public void SetColor()
    {
        Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        spriteRenderer.color = randomColor;
        //Debug.Log(randomColor);        
    }
}
