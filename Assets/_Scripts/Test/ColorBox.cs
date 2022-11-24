using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBox : Fighter
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
        SetColor();
    }
   public void SetColor()
    {
        Color randomColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        spriteRenderer.color = randomColor;
        //Debug.Log(randomColor);        
    }
}
