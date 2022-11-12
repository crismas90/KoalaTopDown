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

    void Awake()
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

    protected virtual void Death()
    {
        isAlive = false;
        Debug.Log(transform.name + " died.");
    }
}