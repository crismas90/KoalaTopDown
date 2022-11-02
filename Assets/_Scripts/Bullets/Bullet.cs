using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int damage;
    [HideInInspector] public float pushForce;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {        
        Destroy(gameObject);
    }
}
