using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [Header("Параметры")]
    public bool isAlive = true;
    public int currentHealth;
    public int maxHealth;
    [HideInInspector] public Rigidbody2D rb2D;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
        rb2D = GetComponent<Rigidbody2D>();
    }



    // All fighters can ReceiveDamage / Die
    public virtual void TakeDamage(int dmg)
    {
        
        currentHealth -= dmg; 
                       
        //Death
        if (currentHealth <= 0)
            {
                currentHealth = 0;
                isAlive = false;
                Death();
            }       
    }
    public virtual void Heal(int healingAmount)
    {
        if (currentHealth == maxHealth)
            return;

        currentHealth += healingAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        //GameManager.instance.ShowText("+" + healingAmount.ToString() + "hp", 25, Color.green, transform.position, Vector3.up * 30, 1.5f);
        //GameManager.instance.OnHitpointChange();
    }

    protected virtual void Death()
    {
        isAlive = false;
        Debug.Log(transform.name + " died.");
    }
}