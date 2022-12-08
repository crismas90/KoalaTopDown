using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyShield : MonoBehaviour
{
    public bool shieldOn;
    public int shieldHp;
    public int shieldHpMax;
    SpriteRenderer spriteRenderer;

    GameObject floatinText;

    private void Awake()
    {
        floatinText = GameAssets.instance.floatingText;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (shieldHp > 25 && !shieldOn)
        {
            ShieldOnOff(true);
        }       
    }

    public void TakeDamage(int dmg)
    {
        shieldHp -= dmg;
        ShowDamageShield("-" + dmg.ToString());
        
        if (shieldHp <= 0)
        {
            shieldHp = 0;            
            ShieldOnOff(false);
        }
    }

    public void HealShield(int heal)
    {
        shieldHp += heal;
        ShowDamageShield("+" + heal.ToString() + " shield");
    }

    void ShowDamageShield(string text)
    {
        int floatType = Random.Range(0, 3);

        GameObject textPrefab = Instantiate(floatinText, transform.position, Quaternion.identity);
        textPrefab.GetComponentInChildren<TextMesh>().text = text;
        textPrefab.GetComponentInChildren<TextMesh>().color = Color.blue;
        textPrefab.GetComponentInChildren<Animator>().SetFloat("FloatType", floatType);
    }

    void ShieldOnOff(bool on)
    {
        if (on)
        {
            spriteRenderer.enabled = true;
            shieldOn = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            shieldOn = false;
        }
    }
}
